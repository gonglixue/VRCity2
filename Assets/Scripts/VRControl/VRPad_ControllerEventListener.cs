﻿namespace VRTK.Examples
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class VRPad_ControllerEventListener : MonoBehaviour
    {
        public Transform padScreen;
        private Transform _padScreenButton;
        private GameObject _PressEnterTip;
        private bool _buttonState = false;  // 原始位置

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

            _padScreenButton = padScreen.Find("PadButton"); // 找到button
            _PressEnterTip = _padScreenButton.Find("PadToolTip").gameObject;
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
        }

        private void DoGripReleased(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "GRIP", "released", e);
        }

        private void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
        {
            DebugLogger(e.controllerIndex, "TOUCHPAD", "pressed down", e);
            
            pressButton();
        }

        private void DoTouchpadReleased(object sender, ControllerInteractionEventArgs e)
        {
            DebugLogger(e.controllerIndex, "TOUCHPAD", "released", e);

            releaseButton();
        }

        private void DoTouchpadTouchStart(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "TOUCHPAD", "touched", e);
            touchButton();
        }

        private void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
        {
            //DebugLogger(e.controllerIndex, "TOUCHPAD", "untouched", e);
            untouchButton();
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

        void pressButton()
        {
            if(!_buttonState)
            {
                // 变换，颜色变化
                _padScreenButton.transform.Rotate(10, 0, 0, Space.Self);
                _padScreenButton.GetComponent<MeshRenderer>().material.color = new Color(92 / 255f, 225 / 255f, 231 / 255f, 1.0f); // 92 225 231

                _buttonState = true;
            }
        }

        void releaseButton()
        {
            if(_buttonState)
            {
                // 复原变换，颜色复原
                _padScreenButton.transform.Rotate(-10, 0, 0, Space.Self);
                _padScreenButton.GetComponent<MeshRenderer>().material.color = new Color(149 / 255f, 149 / 255f, 149 / 255f, 1.0f);
                _buttonState = false;

                Debug.Log("按钮release，跳转");
                SceneManager.LoadScene("MeshGenerationOrigin");
            }
        }

        void touchButton()
        {
            // touch
            _padScreenButton.GetComponent<MeshRenderer>().material.color = new Color(92 / 255f, 1.0f, 1.0f, 1.0f);
            // TODO
            // 显示press to enter提示;
            _PressEnterTip.SetActive(true);
        }

        void untouchButton()
        {
            // 颜色复原
            _padScreenButton.GetComponent<MeshRenderer>().material.color = new Color(149 / 255f, 149 / 255f, 149 / 255f, 1.0f);
            // 隐藏press to enter提示；
            _PressEnterTip.SetActive(false);
        }
    }
}