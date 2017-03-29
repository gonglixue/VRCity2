using UnityEngine;
using System.Collections;

public class FlagBillboardController : MonoBehaviour {
    private bool isPointed = false;

    public Transform eyeCamera;
    public Transform padScreen;

    private string _city;
    private string _location;
    private string _country;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
            // TODO 高亮

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
            // TODO 取消高亮
            // TODO 取消连出的billboard
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
