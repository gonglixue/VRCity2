using UnityEngine;
using System.Collections;

public class monthDotInfo : MonoBehaviour {

    public int value;
    public int month;  // 1-12


    static float maxHeight = 7.0f;
    

    public GameObject verticalBar;
    public GameObject line;
    public GameObject classGroup;
    private const int BASE_NUM = 8;
    [SerializeField]
    private float[] classData = new float[BASE_NUM];

    #region 交互响应参数
    public float scaleFactor = 1.4f;   // 点击时变大倍数
    public enum ScaleState
    {
        Normal = 0,
        Bigger = 1
    }
    private ScaleState scaleState = ScaleState.Normal;
    private float originalScaleSize;
    private GoTween _scaleInTween;  //放大操作
    private GoTween _scaleOutTween;  //缩小操作
    static int finishMonth = 0;      //有多少个圆点已位移完毕
    #endregion

    #region billboard参数
    private GameObject thisTip;  //和这个圆点关联的billboard
    Object tipPrefab;
    #endregion

    void Awake()
    {
        originalScaleSize = this.transform.localScale.x;
        tipPrefab = Resources.Load("pivotTip", typeof(GameObject));
        thisTip = null;
        
    }
	// Use this for initialization
	void Start () {
        RandomInitClassData();
        classGroup = GameObject.Find("classGroup");
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetMouseButtonDown(0) && month == 5)
        {
            Debug.Log("mouse down:" + month);
            PointerInMonth();
        }
        if(Input.GetMouseButtonUp(0) && month == 5)
        {
            Debug.Log("mouse up:" + month);
            PointerOutMonth();
        }
        Test();
    }

    public void setValue(int v, int max)
    {
        createVerticalBar();

        value = v;
        float newPosY = v*1.0f / max * maxHeight;

        // ease transformation
        Vector3 newPos = new Vector3(transform.position.x, newPosY, transform.position.z);
        GoTweenConfig config = new GoTweenConfig()
            .position(newPos)
            .setEaseType(GoEaseType.CubicInOut);
        config.onComplete(delegate (AbstractGoTween obj)
        {
            finishSetValue();
        });
        Go.to(this.transform, 1.0f, config);
    }

    public void finishSetValue()
    {
        //Debug.Log("finish set value" + month);
        finishMonth++;
        if(finishMonth == 12)  // 完成最后一个点的位移
        {
            Debug.Log("finish all month value setting");
            this.transform.parent.GetComponent<monthDataController>().JoinLine();
        }
    }

    /// <summary>
    /// 射线进入本月的圆点
    /// </summary>
    void PointerInMonth()
    {
        Debug.Log("pointer in month: " + month);
        // TODO
        // display billboard
        displayBillboard();

        if(scaleState == ScaleState.Normal)
        {
            // 变大
            _scaleOutTween = Go.to(this.transform, 0.8f,
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
        Debug.Log("pointer out month: " + month);
        // TODO
        // destroy billboard
        destroyBillboard();

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
            Debug.Log("stop bigger tween");
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

    /// <summary>
    /// trigger use; display class data,显示柱状图
    /// </summary>
    void triggerUseClassData()
    {
        // 对classGroupController进行操作
        classGroup.GetComponent<classGroupController>().SetClassData(this.classData);
        classGroup.GetComponent<classGroupController>().StartElevation();
    }

    /// <summary>
    /// 显示这个圆点关联的billboard
    /// </summary>
    void displayBillboard()
    {
        if(!thisTip)
        {
            GameObject tip = Instantiate(tipPrefab, this.transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
            tip.transform.localScale *= 0.4f;
            string textInfo = getMonthString(month) + ": " + value;
            tip.GetComponent<tipController>().SetText(textInfo);
            thisTip = tip;
        }
    }

    void destroyBillboard()
    {
        if(thisTip)
        {
            thisTip.GetComponent<tipController>().hideBillboard();
            thisTip = null;
        }
    }

    private string getMonthString(int i)
    {
        string month = "unknown";
        switch (i)
        {
            case 1: month = "January"; break;
            case 2: month = "February"; break;
            case 3: month = "March"; break;
            case 4: month = "April"; break;
            case 5: month = "May"; break;
            case 6: month = "June"; break;
            case 7: month = "July"; break;
            case 8: month = "August"; break;
            case 9: month = "September"; break;
            case 10: month = "Octobor"; break;
            case 11: month = "November"; break;
            case 12: month = "December"; break;
             
        }

        return month;
    }

    void RandomInitClassData()
    {
        for(int i = 0; i < BASE_NUM; i++)
        {
            System.Random ran = new System.Random();
            int ranNum = ran.Next(5, 30);
            classData[i] = ranNum;
            Debug.Log("init random:" + classData[i]);
        }
    }

    void Test()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            triggerUseClassData();
        }
    }

}
