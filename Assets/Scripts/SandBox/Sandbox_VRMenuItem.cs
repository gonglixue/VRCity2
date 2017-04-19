using UnityEngine;
using System.Collections;

public class Sandbox_VRMenuItem : MonoBehaviour {

    public enum VRMenuItemName
    {
        Earth = 0,
        Sandbox = 1,
        CitySample = 2,
        About = 3
    }

    public VRMenuItemName menuItemName;

    #region pending parameters
    private GoTween _pendingTween = null;
    private GoTweenConfig _pendingTweenConfig;
    private Vector3 _originPendingScale;
    public Transform pendingTransform;
    public GameObject pendingBarBack;
    #endregion

    public GameObject cameraRig;

    // Use this for initialization
    void Start () {
        InitPendingParams();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnHover()
    {
        if(_pendingTween == null)
        {
            pendingBarBack.SetActive(true);
            _pendingTween = Go.to(pendingTransform, 1.8f, _pendingTweenConfig);
        }
        
    }

    public void OnLeave()
    {
        if(_pendingTween != null)
        {
            _pendingTween.destroy();
            pendingTransform.localScale = _originPendingScale;
            _pendingTween = null;
        }

        pendingBarBack.SetActive(false);
    }

    void InitPendingParams()
    {
        _originPendingScale = pendingTransform.localScale;

        _pendingTweenConfig = new GoTweenConfig()
            .scale(new Vector3(1f, 1f, 1f))
            .setEaseType(GoEaseType.CubicInOut);

        _pendingTweenConfig.onComplete(delegate (AbstractGoTween obj)
        {
            ConfirmChoose();
        });
    }

    void ConfirmChoose()
    {
        // pending状态复原
        pendingTransform.localScale = _originPendingScale;
        pendingBarBack.SetActive(false);

        switch(this.menuItemName)
        {
            case VRMenuItemName.Earth:
                break;
            case VRMenuItemName.Sandbox:
                cameraRig.transform.position = new Vector3(0, 0, -2.7f);
                break;
            case VRMenuItemName.CitySample:
                break;
            case VRMenuItemName.About:
                break;

        }
    }
}
