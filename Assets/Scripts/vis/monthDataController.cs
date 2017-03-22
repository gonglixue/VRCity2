using UnityEngine;
using System.Collections;

public class monthDataController : MonoBehaviour {
    const float maxHeight = 7;
    public int[] monthData;

    public GameObject linePrefab;
    public Transform screen;

    public Transform[] monthTransformList;
	// Use this for initialization
	void Start () {
        DropDownScreen();
        //Invoke("JoinLine", 2.5f);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void DropDownScreen()
    {
        var config = new GoTweenConfig()
            .scale(new Vector3(screen.localScale.x, 4.0f, screen.localScale.z))
            .setEaseType(GoEaseType.CubicInOut);
        // 设置屏幕下降的回调
        config.onComplete(delegate (AbstractGoTween obj)
        {
            BeginSetValue();
        });
        var tween = Go.to(screen, 1.5f, config);

    }

    void BeginSetValue()
    {
        Debug.Log("begin set value");
        // TODO
        // 完成屏幕下降后，设置month dot位置

        // find max
        int max = monthData[0];
        for (int j = 1; j < monthData.Length; j++)
        {
            if (monthData[j] > max)
                max = monthData[j];
        }

        // 设置高度
        int i = 0;

        foreach (Transform child in monthTransformList)
        {
            if (i > 11)
                break;

            //Debug.Log(child.name);
            child.GetComponent<monthDotInfo>().setValue(monthData[i], max);
            i++;
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
