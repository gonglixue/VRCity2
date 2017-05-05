using UnityEngine;
using System.Collections;

public class monthDataController : MonoBehaviour {
    public int[] monthData;
    //public ArrayList monthData = new ArrayList();
    public GameObject linePrefab;
    public Transform screen;
    public Transform lineGroup;
    public GameObject classDataGroup;

    public Transform[] monthTransformList;
    //public ArrayList monthTransformList = new ArrayList();

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
    static int AlignFinishNum = 0;  // 所有点排列完再连线
    #endregion

    public GameObject ScreenUICanvas;

    // Use this for initialization
    void Start () {
        //DropDownScreen();
        AlignStep = Mathf.Abs(AlignDegree) * 2 / (monthData.Length - 1);
        monthTransformList = new Transform[monthData.Length];       
    }
	
	// Update is called once per frame
	void Update () {
        Test();
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
            {
                BeginSetValue_Align();
                _firstDrop = false;
            }                
            else
                BeginSetValue();
            _isFullDrop = true;

            // 显示chartScreen UI
            ScreenUICanvas.GetComponent<Canvas>().enabled = true;
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
        NetworkService myNetwork = new NetworkService();

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
            monthDotEntity.GetComponent<monthDotInfo>().month = j + 1;  // 设置月份
            // TODO 设置每个散点的柱状图数据
            // TODO 网络请求： 请求月份天气 ?month=j&year=2016
            string args = "?month=" + j + "&year=2016";
            StartCoroutine(myNetwork.RequestClassData(args, monthDotEntity, RequestClassDataCallback));

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

            GameObject line = Instantiate(linePrefab, centerPos, Quaternion.identity, lineGroup) as GameObject;
            //GameObject line = Instantiate(linePrefab) as GameObject;
            //line.transform.rotation = Quaternion.identity;
            //line.transform.position = centerPos;
            line.transform.forward = child.position - lastChild.position;
            line.transform.localScale = new Vector3(line.transform.localScale.x, line.transform.localScale.y, distance);
            //line.transform.parent = lineGroup;
        }
    }

    public void ReAlignDots()
    {
        DestroyOldLine();

        int[] oldMonthData = monthData;
        monthData = new int[oldMonthData.Length + 12];

        float max=oldMonthData[0], min=oldMonthData[0];

        for(int i=0; i<monthData.Length; i++)
        {
            if(i < oldMonthData.Length)
                monthData[i] = oldMonthData[i];
            else  // temp: 复制原来的数据
            {
                monthData[i] = oldMonthData[i % oldMonthData.Length];
                if (monthData[i] > max)
                    max = monthData[i];
                if (monthData[i] < min)
                    min = monthData[i];
            }
        }

        AlignStep = Mathf.Abs(AlignDegree) * 2 / (monthData.Length - 1);
        Transform[] oldTransformList = monthTransformList;

        monthTransformList = new Transform[oldTransformList.Length + 12];
        for(int i=0; i<monthTransformList.Length; i++)
        {
            if(i < oldTransformList.Length)
            {
                float currentDegree = AlignDegree + AlignStep * i;
                float posX, posZ, posY;
                posX = AlignCenter.x + Mathf.Sin(currentDegree / 180 * Mathf.PI) * AlignRadius;
                posZ = AlignCenter.z + Mathf.Cos(currentDegree / 180 * Mathf.PI) * AlignRadius;
                posY = oldTransformList[i].position.y;

                GoTweenConfig config = new GoTweenConfig()
                    .position(new Vector3(posX, posY, posZ))
                    .setEaseType(GoEaseType.Linear);
                config.onComplete(delegate (AbstractGoTween obj)
                {
                    AlignFinishNum++;
                    if(AlignFinishNum == monthTransformList.Length)
                    {
                        JoinLine();
                    }
                });

                Go.to(oldTransformList[i], 0.8f, config);

                //移动原有的柱子
                oldTransformList[i].GetComponent<monthDotInfo>().moveVerticalBar(new Vector3(posX, 3.8f, posZ));

                monthTransformList[i] = oldTransformList[i];
            }
            else
            {
                // 新dot
                GameObject monthDotEntity = Instantiate(monthDotEntityPrefab) as GameObject;
                monthDotEntity.name = "m_Align" + i;
                float currentDegree = AlignDegree + AlignStep * i;
                float posX, posZ;
                posX = AlignCenter.x + Mathf.Sin(currentDegree / 180 * Mathf.PI) * AlignRadius;
                posZ = AlignCenter.z + Mathf.Cos(currentDegree / 180 * Mathf.PI) * AlignRadius;
                monthDotEntity.transform.position = new Vector3(posX, 0, posZ);
                monthDotEntity.transform.parent = this.transform;

                monthDotEntity.GetComponent<monthDotInfo>().setValue(monthData[i], max, min);

                monthTransformList[i] = monthDotEntity.transform;
                AlignFinishNum++;
            }
        }
    }

    public void DecreaseDots()
    {
        DestroyOldLine();
        int[] oldMonthData = monthData;
        monthData = new int[oldMonthData.Length - 12];

        float max =oldMonthData[0], min = oldMonthData[0];
        for(int i=0; i<monthData.Length; i++)
        {
            monthData[i] = oldMonthData[i];
            if (monthData[i] > max)
                max = oldMonthData[i];
            if (monthData[i] < min)
                min = oldMonthData[i];
        }

        AlignStep = Mathf.Abs(AlignDegree) * 2 / (monthData.Length - 1);
        AlignFinishNum = 0;

        Transform[] oldTransformList = monthTransformList;
        monthTransformList = new Transform[oldTransformList.Length - 12];
        //for(int i=0; i<oldTransformList.Length; i++)
        for(int i=oldTransformList.Length-1; i>=0; i--)  //先Destroy新增的点，再做异步位移
        {
            if(i<monthTransformList.Length)
            {
                monthTransformList[i] = oldTransformList[i];
                // TODO 移动
                float currentDegree = AlignDegree + AlignStep * i;
                float posX, posZ, posY;
                posX = AlignCenter.x + Mathf.Sin(currentDegree / 180 * Mathf.PI) * AlignRadius;
                posZ = AlignCenter.z + Mathf.Cos(currentDegree / 180 * Mathf.PI) * AlignRadius;
                posY = oldTransformList[i].position.y;

                GoTweenConfig config = new GoTweenConfig()
                    .position(new Vector3(posX, posY, posZ))
                    .setEaseType(GoEaseType.Linear);
                config.onComplete(delegate (AbstractGoTween obj)
                {
                    AlignFinishNum++;
                    if(AlignFinishNum == monthTransformList.Length)
                    {
                        JoinLine();
                    }
                });

                Go.to(monthTransformList[i], 0.8f, config);
                monthTransformList[i].GetComponent<monthDotInfo>().moveVerticalBar(new Vector3(posX, 3.8f, posZ));
            }
            else
            {
                // TODO Destroy
                oldTransformList[i].GetComponent<monthDotInfo>().DestroyThisDot();
            }
        }
    }

    void DestroyOldLine()
    {
        
        foreach(Transform lineChild in lineGroup)
        {
            //Debug.Log("destroy old line");
            Destroy(lineChild.gameObject);
        }
    }

    void RequestClassDataCallback(GameObject monthDotEntity, float[] data_in)
    {
        monthDotEntity.GetComponent<monthDotInfo>().SetClassData(data_in);
        Debug.Log("request class data callback: " + data_in[0] + "," + data_in[1] + "," + data_in[2] + "," + data_in[3]);
    }

    void Test()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            ReAlignDots();
        }
    }
}
