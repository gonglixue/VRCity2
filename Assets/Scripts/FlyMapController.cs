﻿using UnityEngine;
using System.Collections.Generic;
using Mapbox.MeshGeneration.Data;
using Mapbox.Scripts.Utilities;
using Mapbox.Scripts.UI;

namespace Mapbox.MeshGeneration
{
    public class FlyMapController : MonoBehaviour
    {
        private IFileSource _fileSource;
        public static Rect ReferenceTileRect { get; set; }
        public static float WorldScaleFactor { get; set; }

        public MapVisualization MapVisualization;
        public float TileSize = 100;  // 一片tiles在unity坐标中的size是100,墨卡托坐标系611

        [SerializeField]
        private bool _snapYToZero = true;

        [Geocode]
        public string LatLng;
        public int Zoom;
        public Vector4 Range;

        private GameObject _root;
        private Dictionary<Vector2, UnityTile> _tiles;


        public void Awake()
        {

            _fileSource = MapboxConvenience.Instance.FileSource;
            MapVisualization.Initialize(this, _fileSource);
            _tiles = new Dictionary<Vector2, UnityTile>();

        }

        public void Start()
        {
            Execute();
        }

        public void Update()
        {
            if (_snapYToZero)
            {
                var ray = new Ray(new Vector3(0, 1000, 0), Vector3.down);
                RaycastHit rayhit;
                if (Physics.Raycast(ray, out rayhit))
                {
                    _root.transform.position = new Vector3(0, -rayhit.point.y, 0);
                    _snapYToZero = false;
                }
            }
        }

        public void Execute()
        {
            var parm = LatLng.Split(',');
            Execute(double.Parse(parm[0]), double.Parse(parm[1]), Zoom, Range);
        }

        public void Execute(double lat, double lng, int zoom, Vector4 frame)  // frame = (1,1,1,1)
        {
            bool hasOldRoot = false;
            GameObject _oldRoot = _root;
            //frame goes like left-top-right-bottom here
            if (_root != null)
            {
                hasOldRoot = true;

            }

            _root = new GameObject("worldRoot");

            var v2 = Conversions.LatLonToMeters(lat, lng);  //经纬坐标转墨卡托坐标
            var tms = Conversions.MetersToTile(v2, zoom);   //在被放大2^zoom次方的地图上，找到v2坐标对应的Tiles ID
            ReferenceTileRect = Conversions.TileBounds(tms, zoom);  // 返回的Rect是基于墨卡托坐标. RefernceTile:指定经纬度所在的tile
            WorldScaleFactor = TileSize / ReferenceTileRect.width;  // 一片tiles在unity坐标系中的宽度/一片tiles在墨卡托坐标中的宽度，用于从墨卡托坐标scale到unity坐标
            _root.transform.localScale = Vector3.one * WorldScaleFactor;
            Debug.Log("mapcontroller: reference tile rect width: " + ReferenceTileRect.width);
            Debug.Log("mapcontroller: reference:" + (ReferenceTileRect.max.x - ReferenceTileRect.min.x));
            //creating tiles on demand, we can use something like Thiago's slippy map here as well
            for (int i = (int)(tms.x - frame.x); i <= (tms.x + frame.z); i++)
            {
                for (int j = (int)(tms.y - frame.y); j <= (tms.y + frame.w); j++)
                {
                    GameObject tileObject = new GameObject("Tile - " + i + " | " + j);
                    var tile = tileObject.AddComponent<UnityTile>();

                    _tiles.Add(new Vector2(i, j), tile);
                    tile.Zoom = zoom;
                    tile.RelativeScale = Conversions.GetTileScaleInMeters(0, Zoom) / Conversions.GetTileScaleInMeters((float)lat, Zoom);
                    tile.TileCoordinate = new Vector2(i, j);
                    tile.Rect = Conversions.TileBounds(tile.TileCoordinate, zoom);  // 墨卡托坐标
                    tile.transform.position = new Vector3(tile.Rect.center.x - ReferenceTileRect.center.x, 0, tile.Rect.center.y - ReferenceTileRect.center.y);
                    tile.transform.SetParent(_root.transform, false);


                    tileObject.AddComponent<TileIntro>().setTileInfo(new Vector2(i, j), tile.Rect, tile.Zoom, tile.RelativeScale);

                    if (i == tms.x && j == tms.y)
                    {
                        // 如果是参考tile
                        tileObject.GetComponent<TileIntro>().setRefernceTile();
                    }

                    tile.depth = 3;

                    //tileObject.GetComponent<MeshRenderer>().enabled = false;
                    Config.tilesDic.Add(new Vector2(i, j), tileObject);
                    MapVisualization.ShowTile(tile);
                }
            }

            if(hasOldRoot)
            {
                foreach (Transform t in _oldRoot.transform)
                {
                    Destroy(t.gameObject);
                }
            }
        }

        public void Execute(double lat, double lng, int zoom, Vector2 frame)
        {
            Execute(lat, lng, zoom, new Vector4(frame.x, frame.y, frame.x, frame.y));
        }

        public void Execute(double lat, double lng, int zoom, int range)
        {
            Execute(lat, lng, zoom, new Vector4(range, range, range, range));
        }

        public void Request(Vector2 pos, int zoom)
        {
            if (!_tiles.ContainsKey(pos))
            {
                var tile = new GameObject("Tile - " + pos.x + " | " + pos.y).AddComponent<UnityTile>();
                _tiles.Add(pos, tile);
                tile.transform.SetParent(_root.transform, false);
                tile.Zoom = zoom;
                tile.TileCoordinate = new Vector2(pos.x, pos.y);
                tile.Rect = Conversions.TileBounds(tile.TileCoordinate, zoom);
                tile.RelativeScale = Conversions.GetTileScaleInMeters(0, Zoom) /
                Conversions.GetTileScaleInMeters((float)Conversions.MetersToLatLon(tile.Rect.center).Latitude, Zoom);
                tile.transform.localPosition = new Vector3(tile.Rect.center.x - ReferenceTileRect.center.x,
                                                           0,
                                                           tile.Rect.center.y - ReferenceTileRect.center.y);
                MapVisualization.ShowTile(tile);
            }
        }

        public void setZoom()
        {
            Zoom -= 1;
            Execute();
        }



    }
}