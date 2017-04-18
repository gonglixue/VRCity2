namespace VRTK.Examples
{
    using UnityEngine;

    public class VR_SandboxRctrPointerListener : MonoBehaviour
    {
        int status = 0;  // 初始为0, 放置起点1，放置终点退出NaviMode
        public GameObject startPrefab;
        public GameObject endPrefab;
        public GameObject readyToPlace = null;

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
                        readyToPlace = Instantiate(startPrefab, e.destinationPosition, Quaternion.Euler(90,0,0)) as GameObject;
                    else
                        readyToPlace.transform.position = e.destinationPosition;

                    if(this.GetComponent<VRTK.Examples.VR_SandboxRctrListener>().touchPadPressDown)
                    {
                        status++;
                        readyToPlace = null;
                        Debug.Log("确认放置");
                    }
                }
                if(status == 1)
                {
                    if (readyToPlace == null)
                        readyToPlace = Instantiate(endPrefab, e.destinationPosition, Quaternion.Euler(90, 0, 0)) as GameObject;
                    else
                        readyToPlace.transform.position = e.destinationPosition;

                    if(this.GetComponent<VRTK.Examples.VR_SandboxRctrListener>().touchPadPressDown)
                    {
                        //退出导航模式
                        status=0;
                        readyToPlace = null;

                        // TODO显示 路径

                        Debug.Log("确认放置");
                        AfterPlaceEndPoint();
                    }
                }
            }
        }

        private void DoPointerOut(object sender, DestinationMarkerEventArgs e)
        {
            DebugLogger(e.controllerIndex, "POINTER OUT", e.target, e.distance, e.destinationPosition);
        }

        private void DoPointerDestinationSet(object sender, DestinationMarkerEventArgs e)
        {
            DebugLogger(e.controllerIndex, "POINTER DESTINATION", e.target, e.distance, e.destinationPosition);
        }

        void AfterPlaceEndPoint()
        {
            this.GetComponent<VRTK.Examples.VR_SandboxRctrListener>().LeaveNaviMode();
        }

        // 判断是否是可放置路标的合法点
        bool TrafficTargetIsValid(Transform pointerTarget)
        {
            return pointerTarget.name == "NaviPlane";
        }
    }
}