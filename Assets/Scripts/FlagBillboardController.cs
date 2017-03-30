using UnityEngine;
using System.Collections;

public class FlagBillboardController : MonoBehaviour {
    private bool isPointed = false;

    public Transform eyeCamera;
    public Transform padScreen;
    public Transform cameraRig;
    public float scaleFactor = (0.028f - 0.02f) / (800 - 300);

    #region flag billboard info
    [SerializeField]
    private string _city;
    [SerializeField]
    private string _location;
    [SerializeField]
    private string _country;
    #endregion

    // Use this for initialization
    void Start () {
        eyeCamera = GameObject.Find("Camera (eye)").transform;
        padScreen = GameObject.Find("PadScreen").transform;
        cameraRig = GameObject.Find("[CameraRig]").transform;
	}
	
	// Update is called once per frame
	void Update () {
	    // Flag的大小随cameraRig距地球的距离而调整
        if(this.transform.localScale.x > 0.01)
        {
            float factor = scaleFactor * (cameraRig.position.z - 300) + 0.01f;
            this.transform.localScale = Vector3.one * factor;
        }
	}

    /// <summary>
    /// pointer指向该Flag
    /// </summary>
    public void ActiveFlag()
    {
        if(!isPointed)
        {
            this.transform.localScale *= 1.2f;
            isPointed = true;
            Debug.Log("pointer hover flag ");
            // 粒子系统 高亮
            this.transform.GetChild(0).gameObject.SetActive(true);

            // TODO 连出一个Billboard
            // ...
        }

    }

    /// <summary>
    /// pointer离开该Flag
    /// </summary>
    public void InActiveFlag()
    {
        if(isPointed)
        {
            this.transform.localScale /= 1.2f;
            isPointed = false;
            Debug.Log("pointer leave flag ");
            // 取消高亮
            this.transform.GetChild(0).gameObject.SetActive(false);
            // TODO 取消连出的billboard
            // ...
        }

    }

    /// <summary>
    /// pointer选择该Flag，显示详情UI元素
    /// </summary>
    public void ChooseFlag()
    {
        // TODO 显示菜单 菜单放在另一只手？
        // 在此场景中，左controller为一个小屏幕padScreen
        padScreen.GetComponent<PadScreenController>().DisplayInfo(_location, _city, _country);
        Debug.Log("press grip choose flag");
    }

    /// <summary>
    /// 初始化该点信息
    /// </summary>
    /// <param name="location"></param>
    /// <param name="city"></param>
    /// <param name="country"></param>
    public void InitInfo(string location, string city, string country )
    {
        _location = location;
        _city = city;
        _country = country;
    }

    
}
