namespace VRTK.Examples
{
    using UnityEngine;
    using System.Collections.Generic;

    public class VR_SandboxRctrPointerListener : MonoBehaviour
    {
        int status = 0;  // 初始为0, 放置起点1，放置终点退出NaviMode
        public GameObject startPrefab;
        public GameObject endPrefab;
        public GameObject readyToPlace = null;

        private Transform _startTransform;  //引用transfrom，用于路径请求
        private Transform _endTransform;

        public GameObject DirectionHelperObj;
        public GameObject customRoot;

        private void Start()
        {
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

        private void DoPointerIn(object sender, DestinationMarkerEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "POINTER IN", e.target, e.distance, e.destinationPosition);
            if(TrafficTargetIsValid(e.target)) // 在合法区域内
            {
                if(status == 0) // 放置起点
                {
                    if (readyToPlace == null)
                    {
                        Vector3 placePosition = new Vector3(e.destinationPosition.x, 0.315f, e.destinationPosition.z);
                        readyToPlace = Instantiate(startPrefab, placePosition, Quaternion.Euler(90, 0, 0)) as GameObject;
                        
                        // 放在customRoot下
                        readyToPlace.transform.parent = customRoot.transform;
                        _startTransform = readyToPlace.transform;
                    }
                    else
                    {
                        readyToPlace.transform.position = new Vector3(e.destinationPosition.x, 0.315f, e.destinationPosition.z);
                    }
                        

                    if(this.GetComponent<VRTK.Examples.VR_SandboxRctrListener>().touchPadPressDown)
                    {
                        //status++;
                        status = -1;
                        // 1s 过后设置status = 1;
                        Invoke("DelayStatusTo1", 1.0f);
                        readyToPlace = null;
                        Debug.Log("确认放置1");
                        // 更改Tooltips
                        //VRTK.VRTK_ControllerTooltips RctrTooltipsController = this.transform.Find("ControllerTooltips").GetComponent<VRTK_ControllerTooltips>();
                        //RctrTooltipsController.touchpadText = "Place Target";
                        
                        this.transform.Find("ControllerTooltips").Find("TouchpadTooltip").GetComponent<VRTK_ObjectTooltip>().displayText = "Place Target";

                    }
                }
                if(status == 1)
                {
                    if (readyToPlace == null)
                    {
                        Vector3 placePosition = new Vector3(e.destinationPosition.x, 0.315f, e.destinationPosition.z);
                        readyToPlace = Instantiate(endPrefab, placePosition, Quaternion.Euler(90, 0, 0)) as GameObject;
                        // 放在customRoot下
                        readyToPlace.transform.parent = customRoot.transform;
                        _endTransform = readyToPlace.transform;
                    }
                    else
                    {
                        readyToPlace.transform.position = new Vector3(e.destinationPosition.x, 0.315f, e.destinationPosition.z);
                    }
                        

                    if(this.GetComponent<VRTK.Examples.VR_SandboxRctrListener>().touchPadPressDown)
                    {
                        //退出导航模式
                        status=0;
                        readyToPlace = null;

                        // TODO显示 路径
                        // 请求:
                        QueryDirection(_startTransform, _endTransform);

                        Debug.Log("确认放置2");
                        AfterPlaceEndPoint();
                    }
                }
            }
        }

        private void DoPointerOut(object sender, DestinationMarkerEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "POINTER OUT", e.target, e.distance, e.destinationPosition);
        }

        private void DoPointerDestinationSet(object sender, DestinationMarkerEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "POINTER DESTINATION", e.target, e.distance, e.destinationPosition);
        }

        void AfterPlaceEndPoint()
        {
            this.GetComponent<VRTK.Examples.VR_SandboxRctrListener>().LeaveNaviMode();
            // 隐藏 tool tip
            this.transform.Find("ControllerTooltips").gameObject.SetActive(false);
        }

        // 判断是否是可放置路标的合法点
        bool TrafficTargetIsValid(Transform pointerTarget)
        {
            return pointerTarget.name == "NaviPlane";
        }

        void DelayStatusTo1()
        {
            status = 1;
        }

        void QueryDirection(Transform start, Transform end)
        {
            List<Transform> temp = new List<Transform>();
            temp.Add(start);
            temp.Add(end);

            DirectionHelperObj.GetComponent<DirectionsHelper>().Waypoints = temp;

            DirectionHelperObj.GetComponent<DirectionsHelper>().Query();  // 请求的路径放在customRoot下
            Debug.Log("controller请求路径");
        }
    }
}