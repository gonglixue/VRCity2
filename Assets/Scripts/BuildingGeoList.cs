using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
public class BuildingGeoList : MonoBehaviour
{
    public List<List<buildingInfo>> buildingList = new List<List<buildingInfo>>();
    public List<TileInfo> tileList = new List<TileInfo>();

    // 参考原点tile
    #region referenceTile
    static private Vector2 _referenceTileMeter;
    static private Vector2 _tms;
    static private Rect _referenceTileRect;
    #endregion

    // temp
    private double _north = 52.542316087433;  // lat
    private double _south = 52.54121947358615;
    private double _west = 13.396366385489324;  // lon
    private double _east = 13.398223088795774;

    static private double _worldScaleFactor;   // 墨卡托坐标->unity坐标
    static private double _scaleFactor;        // 数据库tileSize / MapboxTileSize

    private GameObject _root;           // 建筑物组父元素
    private int _tileCount = 0;

    #region atalas
    public Shader buildingShader;
    public bool useAtlas = false;
    #endregion

    // 修正高度差
    public float heightCorrect = -8f;

    #region weather
    public bool toggleSnow = true;
    public Shader SnowShader;
    #endregion

    void Awake()
    {
        InitReference();
        InitRoot();
        
        loadKML();  // 加载总的KML数据，解析得到TileList
        //for (int i = 0; i < tileList.Count; i++)  // 循环每个tile
        foreach(TileInfo tile in tileList)
        {
            loadTileKML(tile);  // 加载一个Tile的KML，解析得到buildingList[i],并绘制该tile
        }

        
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    void loadTileKML()
    {
        List<buildingInfo> buildingsOfATile = new List<buildingInfo>();

        XmlDocument xml = new XmlDocument();
        XmlReaderSettings set = new XmlReaderSettings();
        set.IgnoreComments = true;
        //xml.Load(XmlReader.Create((Application.dataPath + "/data/tile88.kml"), set));
        xml.Load(XmlReader.Create((Application.dataPath + "/data/Layer22.kml"), set));

        // 命名空间设置
        XmlNamespaceManager nsMgr = new XmlNamespaceManager(xml.NameTable);
        nsMgr.AddNamespace("ns", "http://www.opengis.net/kml/2.2");

        XmlNode xdn = xml.DocumentElement;

        //XmlNodeList placeMark = xml.GetElementsByTagName("kml:Placemark");
        XmlNodeList placeMark = xdn.SelectNodes("//ns:Placemark", nsMgr);
        Debug.Log("placemark list length: " + placeMark.Count);
        int length = placeMark.Count;

        //foreach(XmlNode place in placeMark)
        for (int i = 0; i < length; i++)
        {
            XmlNode place = placeMark[i];
            //Debug.Log(place.InnerXml);

            string name = place.SelectSingleNode(".//ns:name", nsMgr).InnerText;
            double latitude = double.Parse(place.SelectSingleNode(".//ns:latitude", nsMgr).InnerText);
            double longitude = double.Parse(place.SelectSingleNode(".//ns:longitude", nsMgr).InnerText);
            double heading = double.Parse(place.SelectSingleNode(".//ns:heading", nsMgr).InnerText);
            string modelHref = place.SelectSingleNode(".//ns:href", nsMgr).InnerText;
            double altitude = double.Parse(place.SelectSingleNode(".//ns:altitude", nsMgr).InnerText);
            Debug.Log(name);

            buildingInfo building = new buildingInfo(latitude, longitude, heading, name, modelHref, altitude);
            buildingsOfATile.Add(building);
        }

        buildingList.Add(buildingsOfATile);
    }

    void loadTileKML(TileInfo singleTile)
    {
        List<buildingInfo> buildingsOfATile = new List<buildingInfo>();

        //TileInfo singleTile = tileList[i];
        string tileColladaKmlPath = "\\" + singleTile.href;
        int lastFolderIndex = singleTile.href.LastIndexOf('\\');
        string singleTilePathPrefix = singleTile.href.Substring(0,lastFolderIndex);
        //Debug.Log(singleTilePathPrefix);

        XmlDocument xml = new XmlDocument();
        XmlReaderSettings set = new XmlReaderSettings();
        set.IgnoreComments = true;
        xml.Load(XmlReader.Create((Application.dataPath + "\\Resources"+tileColladaKmlPath), set));

        // 命名空间设置
        XmlNamespaceManager nsMgr = new XmlNamespaceManager(xml.NameTable);
        nsMgr.AddNamespace("ns", "http://www.opengis.net/kml/2.2");

        XmlNode xdn = xml.DocumentElement;
        XmlNodeList placeMark = xdn.SelectNodes("//ns:Placemark", nsMgr);
        //Debug.Log("placemark list length: " + placeMark.Count);
        int length = placeMark.Count;

        //foreach(XmlNode place in placeMark)
        for (int j = 1; j < length; j++)
        {
            // 每次循环读取tile中的某一个模型数据
            XmlNode place = placeMark[j];

            string name = place.SelectSingleNode(".//ns:name", nsMgr).InnerText;
            double latitude = double.Parse(place.SelectSingleNode(".//ns:latitude", nsMgr).InnerText);
            double longitude = double.Parse(place.SelectSingleNode(".//ns:longitude", nsMgr).InnerText);
            double heading = double.Parse(place.SelectSingleNode(".//ns:heading", nsMgr).InnerText);
            string modelHref = singleTilePathPrefix + '/' + place.SelectSingleNode(".//ns:href", nsMgr).InnerText;
            double altitude = double.Parse(place.SelectSingleNode(".//ns:altitude", nsMgr).InnerText);

            buildingInfo building = new buildingInfo(latitude, longitude, heading, name, modelHref, altitude);
            buildingsOfATile.Add(building);
        }

        // TODO: 每次循环就增加了一个tile信息，即可绘制该tile
        drawATile(buildingsOfATile);
        buildingList.Add(buildingsOfATile);
    }

    void loadKML()
    {
        XmlDocument xml = new XmlDocument();
        XmlReaderSettings set = new XmlReaderSettings();
        set.IgnoreComments = true;
        xml.Load(XmlReader.Create((Application.dataPath + "/data/Layer2.kml"), set));

        // 命名空间设置
        XmlNamespaceManager nsMgr = new XmlNamespaceManager(xml.NameTable);
        nsMgr.AddNamespace("ns", "http://www.opengis.net/kml/2.2");
        nsMgr.AddNamespace("xal", "urn:oasis:names:tc:ciq:xsdschema:xAL:2.0");
        nsMgr.AddNamespace("atom", "http://www.w3.org/2005/Atom");
        nsMgr.AddNamespace("gx", "http://www.google.com/kml/ext/2.2");

        XmlNode xdn = xml.DocumentElement;

        //XmlNodeList Folder = xml.GetElementsByTagName("Folder");
        XmlNodeList Folder = xdn.SelectNodes(".//ns:Folder",nsMgr);
        //Debug.Log("folderplacemark list length: " + Folder.Count);
        int length = Folder.Count;

        foreach(XmlNode tileNode in Folder)
        {
            string name = tileNode.SelectSingleNode("./ns:name", nsMgr).InnerText;
            double north = double.Parse(tileNode.SelectSingleNode(".//ns:north", nsMgr).InnerText);
            double south = double.Parse(tileNode.SelectSingleNode(".//ns:south", nsMgr).InnerText);
            double west = double.Parse(tileNode.SelectSingleNode(".//ns:west", nsMgr).InnerText);
            double east = double.Parse(tileNode.SelectSingleNode(".//ns:east", nsMgr).InnerText);
            string href = tileNode.SelectSingleNode(".//ns:href", nsMgr).InnerText;
            
            //Debug.Log(name + ":" + href);
            TileInfo tile = new TileInfo(name, north, south, east, west, href);
            tileList.Add(tile);
        }
    }

    void drawATile(List<buildingInfo> buildingsOfAtile)
    {
        GameObject tile = new GameObject("Tile-" + _tileCount);
        Material buildingMaterial = new Material(buildingShader);
        Texture2D[] atlasTexture = new Texture2D[buildingsOfAtile.Count];
        Rect[] rects = new Rect[buildingsOfAtile.Count];

        tile.transform.localScale = _root.transform.localScale;  // !!!
        tile.transform.SetParent(_root.transform);

        int i = 0;
        foreach(buildingInfo buildingItem in buildingsOfAtile)
        {
            Vector2 v2 = Mapbox.Conversions.LatLonToMeters(buildingItem.latitude, buildingItem.longitude);
            // 建筑物距离参考tile中点的墨卡托距离
            double deltax = v2.x - _referenceTileRect.center.x;
            double deltay = v2.y - _referenceTileRect.center.y;

            // TODO：建筑物的高度位置计算， absolute in meters
            float posYInUnity =(float)( buildingItem.altitude * _worldScaleFactor );

            Vector3 position = new Vector3((float)(deltax * _worldScaleFactor), posYInUnity, (float)(deltay * _worldScaleFactor));
            Quaternion rotate = Quaternion.AngleAxis(-89.8f, Vector3.right) * (Quaternion.AngleAxis(180, Vector3.forward));
            string path = buildingItem.modelHref.Split('.')[0];
            path = path.Replace('\\', '/');
            //Debug.Log(position);
            GameObject buildingInstance = Instantiate(Resources.Load(path, typeof(GameObject)), position, rotate, tile.transform) as GameObject;
            buildingInstance.transform.Translate(new Vector3(0, heightCorrect*(float)_scaleFactor, 0), Space.World);
            // 为创建的GameObject添加组件
            buildingInstance.AddComponent<BuildingIntro>();  // 添加脚本
            buildingInstance.GetComponent<BuildingIntro>().setBuildingInfo(buildingItem.name, buildingItem.latitude, buildingItem.longitude, buildingItem.altitude);
            buildingInstance.AddComponent<MeshCollider>().convex = true;

            // Shader 气象模拟
            // 设置buildingInstance的shader
            if(toggleSnow)
            {
                buildingInstance.GetComponent<MeshRenderer>().material.shader = SnowShader;
            }
            // 获取buildingInstance的texture
            Texture2D singleTexture = (Texture2D)buildingInstance.GetComponent<MeshRenderer>().material.GetTexture("_MainTex");
            atlasTexture[i] = singleTexture;
            i++;
        }

        // TODO：生成atlas
        if (useAtlas)
        {       
            // 需要修改Texture import setting!!!
            Texture2D atlas = new Texture2D(512, 512);
            rects = atlas.PackTextures(atlasTexture, 0, 512, false);

            // TODO: 材质合并
            // 遍历该tile下的每个building，重新设置他们的material；
        
            int j = 0;
            foreach(Transform childTransform in tile.transform)
            {
                childTransform.GetComponent<MeshRenderer>().materials = new Material[1];

                childTransform.GetComponent<MeshRenderer>().sharedMaterial = buildingMaterial;
                Material m = childTransform.GetComponent<MeshRenderer>().material;
            
                Rect r = rects[j];
                Debug.Log(m.name);
                m.mainTexture = atlas;
                m.SetTexture("_MainTex", atlas);
                m.SetTextureOffset("_MainTex", new Vector2(r.x, r.y));
                m.SetTextureScale("_MainTex", new Vector2(r.width, r.height));

                j++;
            }
        }

        _tileCount++;
    }

    void InitReference()
    {
        _referenceTileMeter = Mapbox.Conversions.LatLonToMeters(Config.latitude, Config.longitude);     // 给定参考原点的经纬度对应的墨卡托坐标
        _tms = Mapbox.Conversions.MetersToTile(_referenceTileMeter, Config.zoom);                       // 给定参考原点所在tile的ID
        _referenceTileRect = Mapbox.Conversions.TileBounds(_tms, Config.zoom);                          // 参考tile在墨卡托坐标下的Rect

        _scaleFactor = calTileScaleFactor(_north, _south, _west, _east, _referenceTileRect);
        _worldScaleFactor = Config.tileSize / _referenceTileRect.width;
        Config.worldFactor = _worldScaleFactor;
        Config.referenceRect = _referenceTileRect;
        Config.referenceTileMeter = _referenceTileMeter;
        Config.tms = _tms;
        //Debug.Log("referenceTileRect width:" + _referenceTileRect.width);
        //Debug.Log("world scale factor: " + _worldScaleFactor);
    }

    void InitRoot()
    {
        _root = new GameObject("Building-Root");
        _root.transform.localScale = Vector3.one * (float)_scaleFactor;
        
    }

    // 计算数据库tileSize / MapboxTileSize
    double calTileScaleFactor(double north, double south, double west, double east, Rect referenceTileRect)
    {
        var north_east = Mapbox.Conversions.LatLonToMeters(north, east);
        var south_west = Mapbox.Conversions.LatLonToMeters(south, west);
        double factor = (north_east.y - south_west.y) / referenceTileRect.width;
        return factor;
    }

    static public Vector2 GetReferenceCenterInMeters()
    {
        return _referenceTileRect.center;
    }
    static public float GetWorldScaleFactor()
    {
        return (float)_worldScaleFactor;
    }
    static public float tileSizeInMeters()
    {
        return _referenceTileRect.width;
    }
    static public Vector2 GetRerenceLeftBottomInMeters()
    {
        return _referenceTileRect.position;
    }
    static public Rect GetRerenceRect()
    {
        return _referenceTileRect;
    }
}

public struct buildingInfo
{
    public double latitude;
    public double longitude;
    public double heading;
    public string name;
    public string modelHref;
    public double altitude;

    public buildingInfo(double _latitude, double _longitude, double _heading, string _name, string _href, double _altitude)
    {
        latitude = _latitude;
        longitude = _longitude;
        heading = _heading;
        name = _name;
        modelHref = _href;
        altitude = _altitude;
    }
}

public struct TileInfo
{
    public string name;
    // border WGS84
    public double north;
    public double south;
    public double east;
    public double west;
    public string href;  // look up certain tile kml;

    public TileInfo(string _name, double _north, double _south, double _east, double _west, string _href)
    {
        name = _name;
        north = _north;
        south = _south;
        east = _east;
        west = _west;
        href = _href;
    }
}
