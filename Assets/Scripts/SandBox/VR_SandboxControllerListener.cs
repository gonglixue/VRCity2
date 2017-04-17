namespace VRTK.Examples
{
    using UnityEngine;

    public class VR_SandboxControllerListener : MonoBehaviour
    {
        [SerializeField]
        private float _startTouchAngle;
        public GameObject worldRoot = null;
        public GameObject customWorldRoot;
        public bool grip = false;

        private void Start()
        {
            if (GetComponent<VRTK_ControllerEvents>() == null)
            {
                Debug.LogError("VRTK_ControllerEvents_ListenerExample is required to be attached to a Controller that has the VRTK_ControllerEvents script attached to it");
                return;
            }

            //Setup controller event listeners
            GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
            GetComponent<VRTK_ControllerEvents>().TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);

            GetComponent<VRTK_ControllerEvents>().TriggerTouchStart += new ControllerInteractionEventHandler(DoTriggerTouchStart);
            GetComponent<VRTK_ControllerEvents>().TriggerTouchEnd += new ControllerInteractionEventHandler(DoTriggerTouchEnd);

            GetComponent<VRTK_ControllerEvents>().TriggerHairlineStart += new ControllerInteractionEventHandler(DoTriggerHairlineStart);
            GetComponent<VRTK_ControllerEvents>().TriggerHairlineEnd += new ControllerInteractionEventHandler(DoTriggerHairlineEnd);

            GetComponent<VRTK_ControllerEvents>().TriggerClicked += new ControllerInteractionEventHandler(DoTriggerClicked);
            GetComponent<VRTK_ControllerEvents>().TriggerUnclicked += new ControllerInteractionEventHandler(DoTriggerUnclicked);

            GetComponent<VRTK_ControllerEvents>().TriggerAxisChanged += new ControllerInteractionEventHandler(DoTriggerAxisChanged);

            GetComponent<VRTK_ControllerEvents>().ApplicationMenuPressed += new ControllerInteractionEventHandler(DoApplicationMenuPressed);
            GetComponent<VRTK_ControllerEvents>().ApplicationMenuReleased += new ControllerInteractionEventHandler(DoApplicationMenuReleased);

            GetComponent<VRTK_ControllerEvents>().GripPressed += new ControllerInteractionEventHandler(DoGripPressed);
            GetComponent<VRTK_ControllerEvents>().GripReleased += new ControllerInteractionEventHandler(DoGripReleased);

            GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed);
            GetComponent<VRTK_ControllerEvents>().TouchpadReleased += new ControllerInteractionEventHandler(DoTouchpadReleased);

            GetComponent<VRTK_ControllerEvents>().TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouchStart);
            GetComponent<VRTK_ControllerEvents>().TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);

            GetComponent<VRTK_ControllerEvents>().TouchpadAxisChanged += new ControllerInteractionEventHandler(DoTouchpadAxisChanged);

            GetComponent<VRTK_ControllerEvents>().ControllerEnabled += new ControllerInteractionEventHandler(DoControllerEnabled);
            GetComponent<VRTK_ControllerEvents>().ControllerDisabled += new ControllerInteractionEventHandler(DoControllerDisabled);
        }

        private void DebugLogger(uint index, string button, string action, ControllerInteractionEventArgs e)
        {
            Debug.Log("Controller on index '" + index + "' " + button + " has been " + action
                    + " with a pressure of " + e.buttonPressure + " / trackpad axis at: " + e.touchpadAxis + " (" + e.touchpadAngle + " degrees)");
        }

        private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "TRIGGER", "pressed", e);
        }

        private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "TRIGGER", "released", e);
        }

        private void DoTriggerTouchStart(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "TRIGGER", "touched", e);
        }

        private void DoTriggerTouchEnd(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "TRIGGER", "untouched", e);
        }

        private void DoTriggerHairlineStart(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "TRIGGER", "hairline start", e);
        }

        private void DoTriggerHairlineEnd(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "TRIGGER", "hairline end", e);
        }

        private void DoTriggerClicked(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "TRIGGER", "clicked", e);
        }

        private void DoTriggerUnclicked(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "TRIGGER", "unclicked", e);
        }

        private void DoTriggerAxisChanged(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "TRIGGER", "axis changed", e);
        }

        private void DoApplicationMenuPressed(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "APPLICATION MENU", "pressed down", e);
        }

        private void DoApplicationMenuReleased(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "APPLICATION MENU", "released", e);
        }

        private void DoGripPressed(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "GRIP", "pressed down", e);
            grip = true;
        }

        private void DoGripReleased(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "GRIP", "released", e);
            grip = false;
        }

        private void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "TOUCHPAD", "pressed down", e);
        }

        private void DoTouchpadReleased(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "TOUCHPAD", "released", e);
        }

        private void DoTouchpadTouchStart(object sender, ControllerInteractionEventArgs e)  // 开始触摸touchpad
        {
            //DebugLogger(e.controllerIndex, "TOUCHPAD", "touched", e);
            if(worldRoot == null)
            {
                worldRoot = GameObject.Find("worldRoot");
            }
            _startTouchAngle = e.touchpadAngle;
        }

        private void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "TOUCHPAD", "untouched", e);
        }

        private void DoTouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "TOUCHPAD", "axis changed", e);
            //TODO 控制沙盘旋转
            float rotateAngle;
            if(_startTouchAngle > 0.1f)
            {
                rotateAngle = e.touchpadAngle - _startTouchAngle;
                worldRoot.transform.Rotate(new Vector3(0, rotateAngle, 0), Space.Self); // 旋转了rotateAngle
                customWorldRoot.transform.Rotate(new Vector3(0, rotateAngle, 0), Space.Self);

                /*
                GoTweenConfig rotConfig = new GoTweenConfig()
                    .localEulerAngles(worldRoot.transform.localEulerAngles + Vector3.up * rotateAngle,true)
                    .setEaseType(GoEaseType.Linear);
                Go.to(worldRoot.transform, 0.3f, rotConfig);
                Go.to(customWorldRoot.transform, 0.3f, rotConfig);
                */
            }
          
        }

        private void DoControllerEnabled(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "CONTROLLER STATE", "ENABLED", e);
        }

        private void DoControllerDisabled(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "CONTROLLER STATE", "DISABLED", e);
        }
    }
}