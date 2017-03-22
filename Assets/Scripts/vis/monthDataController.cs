using UnityEngine;
using System.Collections;

public class monthDataController : MonoBehaviour {
    const float maxHeight = 7;
    public int[] monthData;

    public GameObject linePrefab;
	// Use this for initialization
	void Start () {
        
        // find max
        int max = monthData[0];
        for(int j = 1; j < monthData.Length; j++)
        {
            if (monthData[j] > max)
                max = monthData[j];
        }

        // 设置高度
        int i = 0;
        
        foreach (Transform child in this.transform)
        {
            if (i > 11)
                break;

            Debug.Log(child.name);
            child.GetComponent<monthDotInfo>().setValue(monthData[i], max);
            

            // TODO 
            // 等到所有原点移动到正确位置(setvalue完成后）再连线
            if(i > 0)
            {
                Transform lastChild = this.transform.GetChild(i - 1);
                float distance = Vector3.Distance(lastChild.position, child.position);
                Vector3 centerPos = (lastChild.position + child.position) / 2.0f;
                GameObject line = Instantiate(linePrefab, centerPos, Quaternion.identity, this.transform) as GameObject;
                line.transform.forward = child.position - lastChild.position;
                line.transform.localScale = new Vector3(line.transform.localScale.x, line.transform.localScale.y, distance);
            }

            i++;
        }

        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
