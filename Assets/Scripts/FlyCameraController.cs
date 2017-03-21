using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FlyCameraController : MonoBehaviour
{

    public Vector4 Range;   // 决定当前tile显示范围
    public Vector3 UpMax;
    public Vector3 DownMax;
    public double longitude = 13.3905676;
    public double latitude = 52.5387557;
    public float cameraHeightInWorld = 15.0f;


    private CharacterController _charController;
    public float speed = 6.0f;  // 移动速度
    public float gravity = -9.8f;

    private float _fieldOfView;
    private float _height;

    private double _worldScaleFactor;
    private double _scaleFactor;
    // 参考原点tile
    private Vector2 _referencTileMeter;
    private Vector2 _tms;
    private Rect _referenceTileRect;

    // 相机墨卡托坐标参数
    [SerializeField]
    private Vector2 positionMeter;  // 相机在墨卡托坐标下的位置
    [SerializeField]
    private Rect cameraRect;    // 相机墨卡托坐标Rect
    private float RectWidth;    // 相机在墨卡托坐标下Rect的宽度

    public GameObject FlyMapController;
    private bool[] alreadyUpdateZoom = { false, false, false };
    private int[] UpdateZoomBorder = { 100, 200, 300 };
    //private GameObject rectPlane;
    void Awake()
    {
        _fieldOfView = GetComponent<Camera>().fieldOfView;
        _height = transform.position.y;
        InitReference();

        InitCameraAttrib();

    }

    // Use this for initialization
    void Start()
    {
        _charController = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        // keyboard control
        move();

        if(transform.position.y > 100 && transform.position.y < 200)
        {
            if(!alreadyUpdateZoom[0])
            {
                FlyMapController.GetComponent<Mapbox.MeshGeneration.FlyMapController>().setZoom();
                alreadyUpdateZoom[0] = true;
            }
        }
        else if(transform.position.y >200 && transform.position.y <300)
        {
            if(!alreadyUpdateZoom[1])
            {
                FlyMapController.GetComponent<Mapbox.MeshGeneration.FlyMapController>().setZoom();
                alreadyUpdateZoom[1] = true;
            }
        }
        else if(transform.position.y > 300)
        {
            if(!alreadyUpdateZoom[2])
            {
                FlyMapController.GetComponent<Mapbox.MeshGeneration.FlyMapController>().setZoom();
                alreadyUpdateZoom[2] = true;
            }
        }
        
    }

    void move()
    {
        Vector3 oldPos = transform.position;
        // once move, update the rect in Meters of camera
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaY = Input.GetAxis("Vertical") * speed;
        Vector3 movement = new Vector3(deltaX, 0, deltaY);
        movement = Vector3.ClampMagnitude(movement, speed);

        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);
        _charController.Move(movement);

        Vector3 newPos = transform.position;

    }


    void InitReference()
    {
        _referencTileMeter = Mapbox.Conversions.LatLonToMeters(Config.latitude, Config.longitude);
        _tms = Mapbox.Conversions.MetersToTile(_referencTileMeter, Config.zoom);
        _referenceTileRect = Mapbox.Conversions.TileBounds(_tms, Config.zoom);

        _worldScaleFactor = Config.tileSize / _referenceTileRect.width;  // 墨卡托坐标转unity

        RectWidth = _referenceTileRect.width;
    }



    // 墨卡托坐标转unity坐标
    Vector3 MetersToUnity(Vector2 posInMeter)
    {
        Vector2 referenceCenterMeter = _referenceTileRect.center;
        Vector2 resultPos = new Vector2(posInMeter.x - referenceCenterMeter.x, posInMeter.y - referenceCenterMeter.y);
        resultPos = resultPos * (float)_worldScaleFactor;

        Vector3 posUnity = new Vector3(resultPos.x, cameraHeightInWorld, resultPos.y);
        return posUnity;
    }

    // unity坐标转墨卡托坐标
    Vector2 UnityToMeters(Vector3 posInUnity)
    {
        Vector2 referenceCenterMeter = _referenceTileRect.center;
        float xOffset = (float)(posInUnity.x / _worldScaleFactor + referenceCenterMeter.x);
        float yOffset = (float)(posInUnity.z / _worldScaleFactor + referenceCenterMeter.y);
        return new Vector2(xOffset, yOffset);
    }

    // 初始化相机的位置
    void InitCameraAttrib()
    {
        //positionMeter = Mapbox.Conversions.LatLonToMeters(latitude, longitude);     // 相机在墨卡托坐标下的位置
        //positionMeter = _referenceTileRect.center;
        positionMeter = _referenceTileRect.position;
        transform.position = MetersToUnity(positionMeter);                     // 相机在unity中的世界坐标

        // 相机的Rect width = 地图tile的Rect width
        this.cameraRect = new Rect(positionMeter.x, positionMeter.y, RectWidth, -RectWidth);     // 相机在墨卡托坐标系下的Rect 用右下角定义


    }

    void UpdateRect(Vector2 unityPosition)
    {
        positionMeter = UnityToMeters(this.transform.position);  // 相机Rectcenter
        this.cameraRect = new Rect(positionMeter.x - RectWidth / 2.0f, positionMeter.y - RectWidth / 2.0f, RectWidth, RectWidth);  // 新的相机Rect，用左上角定义

        //Vector3 temp = MetersToUnity(cameraRect.center);
        //rectPlane.transform.position = new Vector3(temp.x, 10, temp.z);

    }

    void UpdateRect(Vector3 oldPos, Vector3 newPos)
    {
        float xMove = newPos.x - oldPos.x;
    }

    void UpdateZomm()
    {
        Debug.Log("update zoom!");
        FlyMapController.GetComponent<Mapbox.MeshGeneration.FlyMapController>().setZoom();
    }
}
