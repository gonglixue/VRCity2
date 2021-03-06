﻿using UnityEngine;
using System.Collections;

public class Sandbox_CubeMenuItemController : MonoBehaviour {
    public enum ItemName
    {
        Home = 0,
        NaviMode = 1,
        DataVis = 2,
        Bulb = 3
    }

    public ItemName menuItemName;
    public Sprite subSprite;
    private Sprite _originSprite;

    private GoTween _pendingTween = null;
    private GoTweenConfig _pendingTweenConfig;
    private Vector3 _originPendingScale ;
    public Transform pendingTransform;
    public GameObject pendingBarBack;

    private bool bHover = false;

    public GameObject RightController;
    public GameObject CameraRig;
    public GameObject ChartScrenn;

	// Use this for initialization
	void Start () {
        _originSprite = this.GetComponent<SpriteRenderer>().sprite;
        _originPendingScale = pendingTransform.localScale;

        _pendingTweenConfig = new GoTweenConfig()
            .scale(new Vector3(1f, 1f, 1f))
            .setEaseType(GoEaseType.CubicInOut);
        _pendingTweenConfig.onComplete(delegate (AbstractGoTween obj)
        {
            ConfirmChoose();
        });


	}
	
	// Update is called once per frame
	void Update () {
        Test();
	}

    void OnTriggerEnter()
    {
        Debug.Log("menu item on trigger enter;");
        OnHover();
    }

    void OnTriggerExit()
    {
        Debug.Log("menu item on trigger exit");
        OnLeave();
    }

    void OnTriggerStay()
    {

    }



    void OnHover()
    {
        pendingBarBack.SetActive(true);
        this.GetComponent<SpriteRenderer>().sprite = subSprite;
        _pendingTween = Go.to(pendingTransform, 1.8f, _pendingTweenConfig);
        bHover = true;
    }

    void OnLeave()
    {
        this.GetComponent<SpriteRenderer>().sprite = _originSprite;
        if(_pendingTween != null)
        {
            Debug.Log("stop pending tween");
            _pendingTween.destroy();
            pendingTransform.localScale = _originPendingScale;
            _pendingTween = null;
        }
        bHover = false;

        pendingBarBack.SetActive(false);
    }

    void ConfirmChoose()
    {
        Debug.Log("choose: " + menuItemName);
        //pendingTransform.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 0, 0);

        // pending状态复原
        pendingTransform.localScale = _originPendingScale;
        pendingBarBack.SetActive(false);


        // TODO 选择菜单项后, 执行相应的处理程序
        switch(this.menuItemName)
        {
            case ItemName.Home: // 主菜单
                HandleChooseHome();
                break;
            case ItemName.NaviMode:
                HandleChooseReturn();
                break;
            case ItemName.DataVis:
                HandleChooseRefresh();
                break;
            case ItemName.Bulb:
                // TODO 
                // 显示VR帮助菜单
                HandleChooseBulb();
                break;
        }

        // sprite复原
        this.transform.GetComponent<SpriteRenderer>().sprite = _originSprite;
    }


    void HandleChooseHome()
    {
        // TODO: 控制VRMenu的渐变
        // ...
        CameraRig.transform.position = new Vector3(36.53f, 0, -0.15f);

    }

    void HandleChooseRefresh()
    {
        // 交互式图表
        if (!ChartScrenn.activeSelf)
        {
            ChartScrenn.SetActive(true);
            ChartScrenn.transform.Find("monthDot").GetComponent<monthDataController>().DropDownScreen();
        }
        else
        {
            ChartScrenn.transform.Find("monthDot").GetComponent<monthDataController>().HideScreen();
        }
    }

    void HandleChooseReturn()
    {
        // 放置导航标志模式
        RightController.GetComponent<VRTK.Examples.VR_SandboxRctrListener>().EnterNaviMode();
        
    }

    void HandleChooseBulb()
    {

    }

    void Test()
    {
        if(this.menuItemName == 0)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                Debug.Log("menu item on trigger enter");
                OnHover();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Debug.Log("menu item on trigger leave");
                OnLeave();
            }
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("choose menu item refresh");
                HandleChooseRefresh();
            }
        }
        
        
    }




}
