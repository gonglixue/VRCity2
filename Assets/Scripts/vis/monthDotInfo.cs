using UnityEngine;
using System.Collections;

public class monthDotInfo : MonoBehaviour {

    public int value;
    public int month;  // 1-12

    static float maxHeight = 7.0f;

    public GameObject verticalBar;
    public GameObject line;

    void Awake()
    {
        createVerticalBar();
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setValue(int v, int max)
    {
        value = v;
        float newPosY = v*1.0f / max * maxHeight;

        // TODO
        // ease transformation
        this.transform.position = new Vector3(transform.position.x, newPosY, transform.position.z);
    }

    void clickThisMonth()
    {
        // 点击查看本月详细信息

    }

    void createVerticalBar()
    {
        float vBary = 4.0f;
        Vector3 vBarPos = new Vector3(this.transform.position.x, vBary, this.transform.position.z);
        GameObject vBar = Instantiate(verticalBar) as GameObject;
        
        vBar.transform.position = vBarPos;
        vBar.transform.SetParent(this.transform.parent);



    }
}
