using UnityEngine;
using System.Collections;

public class classGroupController : MonoBehaviour {
    private const int BASE_NUM = 4;

    public float maxBaseData;
    public float minBaseData;
    private float[] classData = new float[BASE_NUM];
    public Transform[] baseTransform = new Transform[BASE_NUM];

	// Use this for initialization
	void Start () {
	    for(int i = 0; i < BASE_NUM; i++)
        {
            baseTransform[i] = this.transform.GetChild(i);
        }
	}
	
	// Update is called once per frame
	void Update () {
        Test();
        Test2();
	}

    public void SetClassData(float[] data_in)
    {
        float max = 0;
        float min = 0;

        for(int i=0;i<classData.Length;i++)
        {
            classData[i] = data_in[i];
            if (classData[i] > max)
                max = classData[i];
            if (classData[i] < min)
                min = classData[i];
        }
        this.maxBaseData = max;
        this.minBaseData = min;

    }

    public void StartElevation()
    {
        for(int i = 0; i < BASE_NUM; i++)
        {
            // 在每一个base下创建cylinder
            baseTransform[i].GetComponent<baseDataInfo>().InitData(classData[i]);
        }
    }

    public void HideCylinder()
    {
        for(int i = 0; i < BASE_NUM; i++)
        {
            baseTransform[i].GetComponent<baseDataInfo>().HideCylinder();
        }
    }

    void Test()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("press a");
            float[] temp = { 10, 9, 10, 8, 14, 13, 10, 8 };
            //Debug.Log(temp[0]);
            SetClassData(temp);
            StartElevation();
        }
    }
    void Test2()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("press b");
            HideCylinder();
        }
    }
}
