using UnityEngine;
using System.Collections;

public class monthDataController : MonoBehaviour {
    public int[] monthData;

    public GameObject linePrefab;
    public Transform screen;
    public GameObject classDataGroup;

    public Transform[] monthTransformList;

    private bool _isFullDrop = false;
    private bool _firstDrop = true;

    #region 点排列
    [SerializeField]
    private Vector3 AlignCenter;
    [SerializeField]
    private float AlignRadius;
    
    private float AlignDegree = -30f;
    private float AlignStep;
    [SerializeField]
    private GameObject monthDotEntityPrefab;
    #endregion

    // Use this for initialization
    void Start () {
        //DropDownScreen();
        AlignStep = Mathf.Abs(AlignDegree) * 2 / (monthData.Length - 1);
        monthTransformList = new Transform[12];       
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void DropDownScreen()
    {
        var config = new GoTweenConfig()
            .scale(new Vector3(screen.localScale.x, 4.0f, screen.localScale.z))
            .setEaseType(GoEaseType.CubicInOut);
        // 设置屏幕下降的回调
        config.onComplete(delegate (AbstractGoTween obj)
        {
            if (_firstDrop)
                BeginSetValue_Align();
            else
                BeginSetValue();
            _isFullDrop = true;
        });
        var tween = Go.to(screen, 1.5f, config);
    }

    public void HideScreen()
    {
        if(_isFullDrop)
        {
            GoTweenConfig config = new GoTweenConfig()
            .scale(new Vector3(screen.localScale.x, 0.0f, screen.localScale.z))
            .setEaseType(GoEaseType.CubicInOut);
            config.onComplete(delegate (AbstractGoTween obj)
            {
                //this.gameObject.SetActive(false);
                this.transform.parent.gameObject.SetActive(false);
                // 隐藏柱状图
                classDataGroup.GetComponent<classGroupController>().HideCylinder();
            });
            Go.to(screen, 1.0f, config);

            _isFullDrop = false;
        }
        
    }

    void BeginSetValue()
    {
        Debug.Log("begin set value");
        // 完成屏幕下降后，设置month dot位置

        // find max
        int max = monthData[0];
        int min = monthData[0];
        for (int j = 1; j < monthData.Length; j++)
        {
            if (monthData[j] > max)
                max = monthData[j];
            if (monthData[j] < min)
                min = monthData[j];
        }

        // 设置散点的高度
        int i = 0;

        foreach (Transform child in monthTransformList)
        {
            if (i > 11)
                break;

            //Debug.Log(child.name);
            child.GetComponent<monthDotInfo>().setValue(monthData[i], max, min);
            i++;
        }
    }

    void BeginSetValue_Align()
    {
        Debug.Log("begin set align value");

        // find max & min
        int max = monthData[0];
        int min = monthData[1];
        for(int j=1;j<monthData.Length;j++)
        {
            if (monthData[j] > max)
                max = monthData[j];
            if (monthData[j] < min)
                min = monthData[j];
        }

        // 为"monthDot"创建子物体，表示散点
        for(int j = 0; j < monthData.Length; j++)
        {
            GameObject monthDotEntity = Instantiate(monthDotEntityPrefab) as GameObject;
            monthDotEntity.name = "m_Align" + j;
            float currentDegree = AlignDegree + AlignStep * j;
            float posX, posZ;
            posX = AlignCenter.x + Mathf.Sin(currentDegree / 180 * Mathf.PI) * AlignRadius;
            posZ = AlignCenter.z + Mathf.Cos(currentDegree / 180 * Mathf.PI) * AlignRadius;
            monthDotEntity.transform.position = new Vector3(posX, 0, posZ);
            monthDotEntity.transform.parent = this.transform;

            monthDotEntity.GetComponent<monthDotInfo>().setValue(monthData[j], max, min);

            monthTransformList[j] = monthDotEntity.transform;
        }
    }

    public void JoinLine()
    {
        
        Debug.Log("join line");

        for(int i=1; i<monthTransformList.Length; i++)
        {
            Transform lastChild = monthTransformList[i - 1];
            Transform child = monthTransformList[i];

            float distance = Vector3.Distance(lastChild.position, child.position);
            Vector3 centerPos = (lastChild.position + child.position) / 2.0f;

            GameObject line = Instantiate(linePrefab, centerPos, Quaternion.identity, this.transform) as GameObject;
            line.transform.forward = child.position - lastChild.position;
            line.transform.localScale = new Vector3(line.transform.localScale.x, line.transform.localScale.y, distance);
        }
    }
}
