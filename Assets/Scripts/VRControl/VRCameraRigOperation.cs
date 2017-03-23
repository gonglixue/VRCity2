using UnityEngine;
using System.Collections;

public class VRCameraRigOperation : MonoBehaviour {

    #region movement control     
    private bool activeMovement = false;
    private Vector3 movement = new Vector3(0, 0, 0);
    public float speed = 20.0f;
    public GameObject eyeCamera;
    #endregion

    #region reference
    private double _worldScaleFactor;
    private Vector2 _referenceTileMeter;  // 参考原点Tile坐标
    private Vector2 _tms;   //参考原点tileID
    private Rect _referenceTileRect;
    #endregion

    #region 相机墨卡托坐标
    private Vector2 positionMeter;  // 相机在墨卡托坐标下的位置
    private Rect cameraRect;        // 相机墨卡托坐标Rect
    private Rect[] cameraRectArray;
    private float RectWidth;  // 相机在墨卡托坐标下Rect宽度
    public GameObject MapController;
    #endregion

    // Use this for initialization
    void Start () {
        InitReference();
        
	}
	
	// Update is called once per frame
	void Update () {
	    if(activeMovement)
        {
            transform.position += movement;
            StartCoroutine(UpdateCoroutine());
        }
	}

    public void move(Vector3 m)
    {
        movement = m;
    }

    public void activeMove(Vector3 m)
    {
        m = m * speed;
        m = Vector3.ClampMagnitude(m, speed);
        m *= Time.deltaTime;
        movement = eyeCamera.transform.TransformDirection(m);

        activeMovement = true;
    }

    public void inactiveMove()
    {
        activeMovement = false;
    }

    IEnumerator UpdateCoroutine()
    {
        UpdateRect(new Vector2(transform.position.x, transform.position.z));
        MapController.GetComponent<Mapbox.MeshGeneration.MapController>().UpdateMapMesh(cameraRectArray);
        yield return new WaitForSeconds(0);
    }

    void InitReference()
    {
        _referenceTileMeter = Mapbox.Conversions.LatLonToMeters(Config.latitude, Config.longitude);
        _tms = Mapbox.Conversions.MetersToTile(_referenceTileMeter, Config.zoom);
        _referenceTileRect = Mapbox.Conversions.TileBounds(_tms, Config.zoom);

        _worldScaleFactor = Config.tileSize / _referenceTileRect.width;

        RectWidth = _referenceTileRect.width / 2.0f;
    }

    void InitCameraAttrib()
    {
        positionMeter = _referenceTileRect.position;  // 初始时相机在墨卡托坐标系下的位置
        transform.position = MetersToUnity(positionMeter);  //  初始时相机在unity中的世界坐标

        this.cameraRect = new Rect(positionMeter.x, positionMeter.y, RectWidth, RectWidth);  // 相机在墨卡托坐标系下的Rect
        this.cameraRectArray = new Rect[3];
        this.cameraRectArray[0] = this.cameraRect;
        this.cameraRectArray[1] = new Rect(this.cameraRect.x - RectWidth / 2.0f, this.cameraRect.y - RectWidth / 2.0f, RectWidth * 2, RectWidth * 2);
        this.cameraRectArray[2] = new Rect(this.cameraRect.x - RectWidth, this.cameraRect.y - RectWidth, RectWidth * 3, RectWidth * 3);
    }

    void UpdateRect(Vector2 unityPosition)
    {
        positionMeter = UnityToMeters(this.transform.position);
        this.cameraRect = new Rect(positionMeter.x - RectWidth / 2.0f, positionMeter.y - RectWidth / 2.0f, RectWidth, RectWidth);  // 新的相机Rect，用左上角定义
        this.cameraRectArray[0] = this.cameraRect;
        this.cameraRectArray[1] = new Rect(this.cameraRect.x - RectWidth / 2.0f, this.cameraRect.y - RectWidth / 2.0f, RectWidth * 2, RectWidth * 2);
        this.cameraRectArray[2] = new Rect(this.cameraRect.x - RectWidth, this.cameraRect.y - RectWidth, RectWidth * 3, RectWidth * 3);
    }

    Vector3 MetersToUnity(Vector2 posInMeter)
    {
        Vector2 referenceCenterMeter = _referenceTileRect.center;
        Vector2 resultPos = new Vector2(posInMeter.x - referenceCenterMeter.x, posInMeter.y - referenceCenterMeter.y);
        resultPos = resultPos * (float)_worldScaleFactor;

        Vector3 posUnity = new Vector3(resultPos.x, 15.0f, resultPos.y);
        return posUnity;
    }

    Vector3 UnityToMeters(Vector3 posInUnity)
    {
        Vector2 referenceCenterMeter = _referenceTileRect.center;
        float xOffset = (float)(posInUnity.x / _worldScaleFactor + referenceCenterMeter.x);
        float yOffset = (float)(posInUnity.z / _worldScaleFactor + referenceCenterMeter.y);
        return new Vector2(xOffset, yOffset);
    }
}
