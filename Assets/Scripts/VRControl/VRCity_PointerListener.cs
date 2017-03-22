namespace VRTK.Examples
{
    using UnityEngine;

    public class VRCity_PointerListener : MonoBehaviour
    {
        private Transform currentPointerObj = null;   // 当前pointer指向的建筑
        

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
            
            if(!currentPointerObj)
            {
                if (e.target.GetComponent<BuildingIntro>())   // pointer指向了一个建筑
                {
                    Debug.Log("pointer in " + e.target);
                    BuildingIntro building = e.target.GetComponent<BuildingIntro>();
                    currentPointerObj = e.target;
                    building.displayBillBoard();
                }
            }
        }

        private void DoPointerOut(object sender, DestinationMarkerEventArgs e)
        {
            DebugLogger(e.controllerIndex, "POINTER OUT", e.target, e.distance, e.destinationPosition);

        }

        private void DoPointerDestinationSet(object sender, DestinationMarkerEventArgs e)  // release
        {
            DebugLogger(e.controllerIndex, "POINTER DESTINATION", e.target, e.distance, e.destinationPosition);

            if(currentPointerObj)
            {
                BuildingIntro building = currentPointerObj.GetComponent<BuildingIntro>();
                building.destroyTip();   // 清除biilboard
            }

            currentPointerObj = null;
            
        }
    }
}