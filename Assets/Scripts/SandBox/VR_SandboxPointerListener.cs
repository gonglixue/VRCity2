namespace VRTK.Examples
{
    using UnityEngine;

    public class VR_SandboxPointerListener : MonoBehaviour
    {
        private VR_SandboxControllerListener _thisControllerListener;
        public Transform lastFocusBillboard;
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

            _thisControllerListener = this.GetComponent<VR_SandboxControllerListener>();
        }

        private void DebugLogger(uint index, string action, Transform target, float distance, Vector3 tipPosition)
        {
            string targetName = (target ? target.name : "<NO VALID TARGET>");
            Debug.Log("Controller on index '" + index + "' is " + action + " at a distance of " + distance + " on object named " + targetName + " - the pointer tip position is/was: " + tipPosition);
        }

        private void DoPointerIn(object sender, DestinationMarkerEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "POINTER IN", e.target, e.distance, e.destinationPosition);
            SandboxBillboardController billboardCtr = e.target.GetComponent<SandboxBillboardController>();
            if(billboardCtr)
            {
                billboardCtr.Hover();
                if (_thisControllerListener.grip)
                {
                    billboardCtr.FocusThis();
                }

                lastFocusBillboard = e.target;
            }

            if(e.target.tag == "Menu")
            {
                e.target.GetComponent<Sandbox_VRMenuItem>().OnHover();

            }
        }

        private void DoPointerOut(object sender, DestinationMarkerEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "POINTER OUT", e.target, e.distance, e.destinationPosition);
            if (e.target.GetComponent<SandboxBillboardController>())
            {
                e.target.GetComponent<SandboxBillboardController>().Leave();
            }
            if(e.target.tag == "Menu")
            {
                e.target.GetComponent<Sandbox_VRMenuItem>().OnLeave();
            }
        }

        private void DoPointerDestinationSet(object sender, DestinationMarkerEventArgs e)  // release
        {
            DebugLogger(e.controllerIndex, "POINTER DESTINATION", e.target, e.distance, e.destinationPosition);
            if(e.target.GetComponent<SandboxBillboardController>())
            {
                e.target.GetComponent<SandboxBillboardController>().FocusThis();
            }
        }
    }
}
