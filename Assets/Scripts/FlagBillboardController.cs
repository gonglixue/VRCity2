using UnityEngine;
using System.Collections;

public class FlagBillboardController : MonoBehaviour {
    private bool isPointed = false;
    public Texture2D flagTexture;
    public Texture2D flagActiveTexture;  // 被选中后的纹理

    public Transform eyeCamera;

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
        this.transform.localScale *= 1.2f;
        this.GetComponent<MeshRenderer>().material.mainTexture = flagActiveTexture;
    }

    /// <summary>
    /// pointer离开该Flag
    /// </summary>
    public void InActiveFlag()
    {
        this.transform.localScale /= 1.2f;
        this.GetComponent<MeshRenderer>().material.mainTexture = flagTexture;
    }

    void BillboardFace()
    {
        this.transform.up = this.transform.position;
        this.transform.forward = eyeCamera.position - this.transform.position;
    }

    /// <summary>
    /// pointer选择该Flag，显示详情UI元素
    /// </summary>
    public void ChooseFlag()
    {

    }

    
}
