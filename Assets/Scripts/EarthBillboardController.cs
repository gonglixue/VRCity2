using UnityEngine;
using System.Collections;

public class EarthBillboardController : MonoBehaviour {

    //显示billboard的经纬度点
    public string[] LatLonList;
    public string[] City;
    public string[] Country;

    public float radius = 100;
    public GameObject billboardPrefab;
    public GameObject cameraRig;

    

	// Use this for initialization
	void Start () {
	    for(int i=0;i<LatLonList.Length;i++)
        {
            CreateBillboard(LatLonList[i]);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// 把经纬度字符串转成unity世界坐标
    /// </summary>
    /// <param name="LatLon">经纬度字符串</param>
    /// <returns>unity世界坐标</returns>
    Vector3 LatLonToUnityCoord(string LatLon)
    {
        // 纬度,经度
        Vector2 LatLonVec = new Vector2(float.Parse(LatLon.Split(',')[0]), float.Parse(LatLon.Split(',')[1]));
        Vector3 worldPos;
        worldPos.y = radius * Mathf.Sin(Mathf.PI*LatLonVec.x/180);
        worldPos.z = radius * Mathf.Cos(Mathf.PI*LatLonVec.x/180) * Mathf.Cos(Mathf.PI*LatLonVec.y/180);
        worldPos.x = radius * Mathf.Cos(Mathf.PI*LatLonVec.x/180) * -Mathf.Sin(Mathf.PI*LatLonVec.y/180);

        return worldPos;
    }

    void CreateBillboard(string LatLon)
    {
        Vector3 position = LatLonToUnityCoord(LatLon);
        // rotation
        Quaternion rotation = Quaternion.identity;

        GameObject billboard = Instantiate(billboardPrefab, position, rotation, this.transform) as GameObject;
        billboard.transform.localScale = Vector3.one * 0.1f;  // TODO： scale
        billboard.transform.up = billboard.transform.position;
    }
}
