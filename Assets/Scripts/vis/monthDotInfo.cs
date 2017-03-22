using UnityEngine;
using System.Collections;

public class monthDotInfo : MonoBehaviour {

    public int value;
    public int month;  // 1-12

    static float maxHeight = 7.0f;

    public GameObject verticalBar;
    public GameObject line;

    #region 交互响应参数
    public float scaleFactor = 1.2f;   // 点击时变大倍数
    public enum ScaleState
    {
        Normal = 0,
        Bigger = 1
    }
    private ScaleState scaleState = ScaleState.Normal;
    private float originalScaleSize;
    private GoTween _scaleInTween;  //放大操作
    private GoTween _scaleOutTween;  //缩小操作
    #endregion


    void Awake()
    {
        originalScaleSize = this.transform.localScale.x;
        createVerticalBar();
    }
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetMouseButtonDown(0) && month == 1)
        {
            Debug.Log("mouse down:" + month);
            PointerInMonth();
        }
        if(Input.GetMouseButtonUp(0) && month == 1)
        {
            Debug.Log("mouse up:" + month);
            PointerOutMonth();
        }
    }

    public void setValue(int v, int max)
    {
        value = v;
        float newPosY = v*1.0f / max * maxHeight;


        // ease transformation
        Vector3 newPos = new Vector3(transform.position.x, newPosY, transform.position.z);
        Go.to(this.transform, 1.0f,
            new GoTweenConfig()
            .position(newPos)
            .setEaseType(GoEaseType.CubicInOut));
    }

    /// <summary>
    /// 射线进入本月的圆点
    /// </summary>
    void PointerInMonth()
    {
        // 点击查看本月详细信息
        UpdateClassData();

        if(scaleState == ScaleState.Normal)
        {
            // 变大
            Go.to(this.transform, 0.8f,
                new GoTweenConfig()
                .scale(originalScaleSize * scaleFactor)
                .setEaseType(GoEaseType.CubicInOut));
            scaleState = ScaleState.Bigger;
        }
        

    }

    /// <summary>
    /// 射线离开本月的圆点
    /// </summary>
    void PointerOutMonth()
    {
        if(scaleState == ScaleState.Bigger)
        {
            StopBiggerTween();
            // 变小
            Go.to(this.transform, 0.8f,
                new GoTweenConfig()
                .scale(originalScaleSize)
                .setEaseType(GoEaseType.CubicInOut));
            scaleState = ScaleState.Normal;
        }
        
    }

    void StopBiggerTween()
    {
        if(_scaleOutTween != null)
        {
            _scaleOutTween.complete();
            _scaleOutTween.destroy();
            _scaleOutTween = null;
        }
    }

    public void createVerticalBar()  
    {
        float vBary = 4.0f;
        Vector3 vBarPos = new Vector3(this.transform.position.x, vBary, this.transform.position.z);
        GameObject vBar = Instantiate(verticalBar) as GameObject;
        
        vBar.transform.position = vBarPos;
        vBar.transform.SetParent(this.transform.parent);
    }

    void UpdateClassData()
    {

    }

}
