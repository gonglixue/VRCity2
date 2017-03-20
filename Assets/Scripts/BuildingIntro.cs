using UnityEngine;
using System.Collections;

public class BuildingIntro : MonoBehaviour {
    public string buildingName;
    public double latitude;
    public double longitude;
    public double altitude;

    // 和这幢建筑相关联的tip
    private GameObject thisTip;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setBuildingInfo(string _bname, double _latitude, double _longitude, double _altitude)
    {
        buildingName = _bname;
        latitude = _latitude;
        longitude = _longitude;
        altitude = _altitude;
    }

    void displayBillBoard()
    {
        Camera mainCamera = Camera.main;
        Object tipPrefab = Resources.Load("tip", typeof(GameObject));
        Vector3 tipPos = new Vector3(this.transform.position.x, this.transform.position.y + 10, this.transform.position.z);
        GameObject tip = Instantiate(tipPrefab) as GameObject;

        tip.transform.up = mainCamera.transform.position - tip.transform.position;
        tip.GetComponent<tipController>().SetText(buildingName);
        thisTip = tip;
    }

    void OnMouseDown()
    {
        Debug.Log("click " + buildingName);
        displayBillBoard();
    }
}
