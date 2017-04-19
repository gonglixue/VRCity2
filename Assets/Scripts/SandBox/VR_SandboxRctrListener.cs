namespace VRTK.Examples
{
    using UnityEngine;

    public class VR_SandboxRctrListener : MonoBehaviour
    {
        public GameObject cubeMenu;
        public float cubeMenuPosFactor = 0.4f;

        public bool allowCubeMenu = true;
        public bool touchPadPressDown = false; // 用于放置Navi标记

        private Vector2 _touchPadAxis;
        private float _touchStartX;

        public GameObject ControllerTooltipsObj;

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
            if(allowCubeMenu)
                ShowCubeMenu();
        }

        private void DoTriggerUnclicked(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "TRIGGER", "unclicked", e);
            HideCubeMenu();
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
        }

        private void DoGripReleased(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "GRIP", "released", e);
        }

        private void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "TOUCHPAD", "pressed down", e);
            touchPadPressDown = true; 

            if(allowCubeMenu)
            {
                if (e.touchpadAxis.x < -0.3f)
                {
                    cubeMenu.GetComponent<Sandbox_CubeMenuController>().SwipeLeft();
                }
                else if (e.touchpadAxis.x > 0.3f)
                {
                    cubeMenu.GetComponent<Sandbox_CubeMenuController>().SwipeRight();
                }
            }
            
        }

        private void DoTouchpadReleased(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "TOUCHPAD", "released", e);
            touchPadPressDown = false;
        }

        private void DoTouchpadTouchStart(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "TOUCHPAD", "touched", e);
            _touchStartX = e.touchpadAxis.x;
        }

        private void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "TOUCHPAD", "untouched", e);
            float touchOffset = e.touchpadAxis.x - _touchStartX;
            if(touchOffset < -1.0f)
            {
                Debug.Log("controller swipe left");
                // swipe left
                cubeMenu.GetComponent<Sandbox_CubeMenuController>().SwipeLeft();  // 滑动cube菜单
            }
            else if(touchOffset > 1.0f)
            {
                Debug.Log("controller swipe right");
                // swipe right
                cubeMenu.GetComponent<Sandbox_CubeMenuController>().SwipeRight(); // 滑动cube菜单
            }
        }

        private void DoTouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "TOUCHPAD", "axis changed", e);
        }

        private void DoControllerEnabled(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "CONTROLLER STATE", "ENABLED", e);
        }

        private void DoControllerDisabled(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "CONTROLLER STATE", "DISABLED", e);
        }

        void ShowCubeMenu()
        {
            cubeMenu.SetActive(true);
            cubeMenu.transform.position = this.transform.position + this.transform.forward * cubeMenuPosFactor;
        }

        void HideCubeMenu()
        {
            cubeMenu.SetActive(false);
        }

        public void EnterNaviMode()
        {
            allowCubeMenu = false;
            this.GetComponent<VRTK.VRTK_SimplePointer>().enabled = true;
            this.GetComponent<VRTK.Examples.VR_SandboxRctrPointerListener>().enabled = true;
            HideCubeMenu();  // 进入路径选择模式，隐藏菜单

            // 显示right Controller Tool Tip
            // trigger 激发射线
            // touchpad 放置
            ControllerTooltipsObj.SetActive(true);
            VRTK.VRTK_ControllerTooltips RctrTooltipsController = ControllerTooltipsObj.GetComponent<VRTK.VRTK_ControllerTooltips>();
            RctrTooltipsController.triggerText = "Active Pointer";
            RctrTooltipsController.touchpadText = "Place Start Point";

            this.GetComponent<BoxCollider>().enabled = false;

        }

        public void LeaveNaviMode()
        {
            allowCubeMenu = true;
            this.GetComponent<VRTK.VRTK_SimplePointer>().enabled = false;
            this.GetComponent<VRTK.Examples.VR_SandboxRctrPointerListener>().enabled = false;
            this.GetComponent<BoxCollider>().enabled = true;
        }
    }
}