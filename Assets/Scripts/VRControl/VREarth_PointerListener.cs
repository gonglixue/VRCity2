namespace VRTK.Examples
{
    using UnityEngine;

    public class VREarth_PointerListener : MonoBehaviour
    {

        #region GameObject Reference
        public Transform light = null;
        public MeshRenderer earthRenderer = null;
        public MeshRenderer atmosphereRender = null;
        public Transform eyeCamera = null;
        public Transform cameraRig = null;
        public Transform earth;
        public Transform padScreen;
        #endregion

        #region ctrParameters
        float MIN_DIST = 200;
        float MAX_DIST = 5000;
        float EARTH_RADIUS = 100;
        float dist = 400;   // 相机和地球的距离
        Quaternion earthRotation;
        Vector2 targetOffCenter = Vector2.zero;
        Vector2 offCenter = Vector2.zero;
        public float rotateSensitivityFactor = 5.0f;
        #endregion

        #region viveController Parameters
        private Vector3 _lastPointerPos = new Vector3(-1000f, -1000f, -1000f);
        //public bool isPressed = false;
        #endregion

        #region pin&billboard
        public GameObject pinPrefab;
        private FlagBillboardController _flagControllerScript = null;
        private GameObject _pin = null;
        private float _lastPutPin = 0;
        #endregion

        private void Start()
        {
            Init();
            if (GetComponent<VRTK_SimplePointer>() == null)
            {
                Debug.LogError("VRTK_ControllerPointerEvents_ListenerExample is required to be attached to a Controller that has the VRTK_SimplePointer script attached to it");
                return;
            }
            
            //Setup controller event listeners
            GetComponent<VRTK_SimplePointer>().DestinationMarkerEnter += new DestinationMarkerEventHandler(DoPointerIn);
            GetComponent<VRTK_SimplePointer>().DestinationMarkerExit += new DestinationMarkerEventHandler(DoPointerOut);
            GetComponent<VRTK_SimplePointer>().DestinationMarkerSet += new DestinationMarkerEventHandler(DoPointerDestinationSet);

            //test();
        }

        private void DebugLogger(uint index, string action, Transform target, float distance, Vector3 tipPosition)
        {
            string targetName = (target ? target.name : "<NO VALID TARGET>");
            Debug.Log("Controller on index '" + index + "' is " + action + " at a distance of " + distance + " on object named " + targetName + " - the pointer tip position is/was: " + tipPosition);
        }

        #region pointer events handler
        private void DoPointerIn(object sender, DestinationMarkerEventArgs e) // pressed
        {
            //DebugLogger(e.controllerIndex, "POINTER IN", e.target, e.distance, e.destinationPosition);
            
            if(e.target.name == "Earth")
            {
                if(this._lastPointerPos.x > -900)
                {
                    Vector2 axis = GetPointerAxis(this._lastPointerPos, e.destinationPosition);  //delta
                    // 进行旋转；
                    RotateEarth(axis);
                }
               
                _lastPointerPos = e.destinationPosition; // 上一帧射线射到的位置

                // 如果grip按下，放置大头针
                if(GetComponent<VREarth_ControllerEventListener>().gripPressed)
                {
                    // 控制大头针放置的频率 （2s间隔时间）
                    if(Time.time > (_lastPutPin+2))
                        PutPin(e.destinationPosition);
                }


            }

            if(e.target.tag == "Flag") //如果射线射到Flag,则激活该Flag
            {
                _flagControllerScript = e.target.GetComponent<FlagBillboardController>();
                _flagControllerScript.ActiveFlag();

                // 如果grip按下，则显示详情UI
                if(GetComponent<VREarth_ControllerEventListener>().gripPressed)
                {
                    _flagControllerScript.ChooseFlag();
                }
            }
            else  // 射线射到的物体不是Flag
            {
                if(_flagControllerScript != null)
                {
                    _flagControllerScript.InActiveFlag();
                    _flagControllerScript = null;
                }
            }
        }

        private void DoPointerOut(object sender, DestinationMarkerEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "POINTER OUT", e.target, e.distance, e.destinationPosition);

            if(e.target.tag == "Flag")
            {
                e.target.GetComponent<FlagBillboardController>().InActiveFlag();
                // ??
                _flagControllerScript = null;
            }

            this._lastPointerPos = new Vector3(-1000, -1000, -1000);

        }

        private void DoPointerDestinationSet(object sender, DestinationMarkerEventArgs e)  // release
        {
            //DebugLogger(e.controllerIndex, "POINTER DESTINATION", e.target, e.distance, e.destinationPosition);
            this._lastPointerPos = new Vector3(-1000, -1000, -1000);
            // ??? 好像不起作用
            if (e.target.tag == "Flag")
            {
                e.target.GetComponent<FlagBillboardController>().InActiveFlag();
                // ??
                _flagControllerScript = null;
            }
            Debug.Log("VREarth_Pointer Listener: PointerDestinationSet");
        }
        #endregion

        /// <summary>
        /// 计算得到旋转参数，用Pointer的偏移量计算Axis
        /// </summary>
        /// <param name="lastPos"></param>
        /// <param name="curPos"></param>
        /// <returns></returns>
        private Vector2 GetPointerAxis(Vector3 lastPos, Vector3 curPos)
        {


            /*    y ^
                    |
                    |
          x <-------o 

             */
            Vector3 offsetVector = curPos - lastPos;

            // 把offset投影到xy平面
            offsetVector.z = 0;
            // 归一
            offsetVector.x /= EARTH_RADIUS;
            offsetVector.y /= EARTH_RADIUS;

            return new Vector2(offsetVector.x, offsetVector.y);
        }

        private void Init()
        {
            earthRotation = earth.rotation;  // 初始时旋转状态
            _lastPutPin = Time.time;
        }

        private void RotateEarth(Vector2 axis)
        {
            if(axis.x !=0 || axis.y !=0)
            {
                //Debug.Log("RotateEarth axis: " + axis.x + " " + axis.y);

                float temp = ((dist - EARTH_RADIUS) / (EARTH_RADIUS)) * rotateSensitivityFactor;
                float rotateSensitivity = Mathf.Min(20f, temp)*1.2f;
                //Debug.Log("calculate sensitivity: " + temp);
                Matrix4x4 rot = new Matrix4x4();
                rot.SetTRS(new Vector3(0, 0, 0), earthRotation, new Vector3(1, 1, 1));
                rot = rot.inverse;
                earthRotation *= (Quaternion.AngleAxis(rotateSensitivity * axis.x, rot*Vector3.up) *
                    Quaternion.AngleAxis(rotateSensitivity * axis.y, rot*Vector3.left));  //绕世界坐标的up, left旋转
                earth.rotation = earthRotation;
            }
        }

        public void ReleasePointer()  // 射线消失。在ControllerEventListener中调用
        {
            this._lastPointerPos = new Vector3(-1000, -1000, -1000);
            if(_flagControllerScript != null)
            {
                _flagControllerScript = null;
            }
            Debug.Log("VREarth_Pointer Listener: ReleasePointer");
        }


        // 把大头针放在地球上
        public void PutPin(Vector3 position)
        {
            if (_pin)
                Destroy(_pin);  // 只能放置一个大头针

            Quaternion rotation = Quaternion.identity;
            _pin = Instantiate(pinPrefab, position, rotation, earth) as GameObject;

            if(cameraRig.position.z < 400)
            {
                _pin.transform.localScale = Vector3.one * 0.04f;
            }
            else
            {
                _pin.transform.localScale = Vector3.one * cameraRig.position.z / 10000;
            }
            //_pin.transform.localScale = Vector3.one * 0.08f;
            _pin.transform.up = _pin.transform.position;

            _lastPutPin = Time.time;

            Vector2 LatLon = UnityCoorToLatLon(position);
            padScreen.GetComponent<PadScreenController>().DisplayPin("(" + LatLon.x + ", " + LatLon.y + ")");
            // 设置bridge 经纬度
            Bridge.latitude = LatLon.x;
            Bridge.longitude = LatLon.y;

            Debug.Log("放置大头针- position:" + position + "latlon:" + LatLon);
        }


        /// <summary>
        /// unity坐标转成经纬度
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector2 UnityCoorToLatLon(Vector3 position)
        {
            Vector2 LatLon;
            float sinLat = position.y / EARTH_RADIUS;
            LatLon.x = Mathf.Asin(sinLat);  // 弧度

            if(sinLat == 1 || sinLat == -1)  // ???sinLat是浮点数
            {
                // 点在极点
                LatLon.x = 180 * LatLon.x / Mathf.PI;
                LatLon.y = 0;
                return LatLon;
            }

            float r2 = Mathf.Cos(LatLon.x) * EARTH_RADIUS;
            Debug.Log("r2 " + r2);
            float cosLon = position.z / r2;
            LatLon.y = Mathf.Acos(cosLon);  // 返回0~pi

            if (position.x > 0)
                LatLon.y *= -1;


            LatLon.x = 180 * LatLon.x / Mathf.PI;
            LatLon.y = 180 * LatLon.y / Mathf.PI;

            return LatLon;
        }

        void test()
        {
            Vector3 t = new Vector3(-15.0f, 50.0f, 85.3f);
            Vector2 r = UnityCoorToLatLon(t);
            Debug.Log("test " + r);
        }
            
    }
}
