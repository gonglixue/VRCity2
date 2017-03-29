﻿namespace VRTK.Examples
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

        #region pin
        public GameObject pinPrefab;

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
                    Vector2 axis = GetPointerAxis(this._lastPointerPos, e.destinationPosition);
                    // 进行旋转；
                    RotateEarth(axis);
                }
               
                _lastPointerPos = e.destinationPosition; // 上一帧射线射到的位置

                // 如果grip按下，放置大头针
                if(GetComponent<VREarth_ControllerEventListener>().gripPressed)
                {
                    PutPin(e.destinationPosition);
                }
            }

            if(e.target.tag == "Flag") //如果射线射到Flag,则激活该Flag
            {
                e.target.GetComponent<FlagBillboardController>().ActiveFlag();

                // 如果grip按下，则显示详情UI
                if(GetComponent<VREarth_ControllerEventListener>().gripPressed)
                {
                    e.target.GetComponent<FlagBillboardController>().ChooseFlag();
                }
            }
        }

        private void DoPointerOut(object sender, DestinationMarkerEventArgs e)
        {
            DebugLogger(e.controllerIndex, "POINTER OUT", e.target, e.distance, e.destinationPosition);
            // ???
            if(e.target.tag == "Flag")
            {
                e.target.GetComponent<FlagBillboardController>().InActiveFlag();
            }
        }

        private void DoPointerDestinationSet(object sender, DestinationMarkerEventArgs e)  // release
        {
            DebugLogger(e.controllerIndex, "POINTER DESTINATION", e.target, e.distance, e.destinationPosition);
            this._lastPointerPos = new Vector3(-1000, -1000, -1000);
            // ???
            if(e.target.tag == "Flag")
            {
                e.target.GetComponent<FlagBillboardController>().InActiveFlag();
            }
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
        }

        private void RotateEarth(Vector2 axis)
        {
            if(axis.x !=0 || axis.y !=0)
            {
                //Debug.Log("rotate earth: " + axis.x + " " + axis.y);

                float temp = ((dist - EARTH_RADIUS) / (EARTH_RADIUS)) * rotateSensitivityFactor;
                float rotateSensitivity = Mathf.Min(20f, temp);
                Debug.Log("calculate sensitivity: " + temp);
                earthRotation *= (Quaternion.AngleAxis(rotateSensitivity * axis.x, Vector3.up) *
                    Quaternion.AngleAxis(rotateSensitivity * axis.y, Vector3.left));  //绕世界坐标的up, left旋转
                earth.rotation = earthRotation;
            }
        }

        public void ReleasePointer()
        {
            this._lastPointerPos = new Vector3(-1000, -1000, -1000);
        }


        // 把大头针放在地球上
        public void PutPin(Vector3 position)
        {
            Quaternion rotation = Quaternion.identity;
            GameObject pin = Instantiate(pinPrefab, position, rotation, earth) as GameObject;
            pin.transform.localScale = Vector3.one * 0.1f;
            pin.transform.up = pin.transform.position;

            // TODO 放置后获得放置点的经纬度
            // ...
            // TODO 跳转
        }

    }
}
