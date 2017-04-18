namespace VRTK.Examples
{
    using UnityEngine;

    public class VR_SandboxRctrPointerListener : MonoBehaviour
    {
        int status = 0;  // 初始为0, 放置起点1，放置终点退出NaviMode
        public GameObject startPrefab;
        public GameObject endPrefab;

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
            if(TrafficTargetIsValid(e.target)) // 在合法区域内，替换pointer tip
            {

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