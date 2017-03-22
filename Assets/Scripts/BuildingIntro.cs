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

    public void displayBillBoard()
    {
        if (!thisTip)
        {
            Camera mainCamera = Camera.main;
            Object tipPrefab = Resources.Load("pivotTip", typeof(GameObject));
            Vector3 highestPoint = findMaxHeight();
            //Vector3 tipPos = new Vector3(this.transform.position.x, highestPoint.y+10, this.transform.position.z);
            //Vector3 tipPos = transform.localToWorldMatrix * highestPoint;
            //Vector3 tipPos = highestPoint;
            Vector3 tipPos = this.transform.localToWorldMatrix.MultiplyPoint(highestPoint);
            Quaternion tipQ = Quaternion.Euler(-90, 0, 0);          
            GameObject tip = Instantiate(tipPrefab, tipPos, tipQ) as GameObject;   // use pivot axis to locate the position of billboard

            tip.GetComponent<tipController>().SetText(buildingName);
            thisTip = tip;
        }
    }

    //void OnMouseDown()
    //{
    //    Debug.Log("click " + buildingName);
    //    displayBillBoard();
    //}

    public void destroyTip()
    {
        if(thisTip)
        {
            // Destroy(thisTip);
            thisTip.GetComponent<tipController>().hideBillboard();
            thisTip = null;
            Debug.Log("distroy tip");
        }
    }

    Vector3 findMaxHeight()
    {
        float z;
        Vector3 highestPoint;
        Mesh colliderMesh = GetComponent<MeshCollider>().sharedMesh;
        Vector3[] vertices = colliderMesh.vertices;

        z = vertices[0].z;
        highestPoint = vertices[0];

        for(int i=1; i<vertices.Length;i++)
        {
            if (vertices[i].z >z)
            {
                z = vertices[i].z;
                highestPoint = vertices[i];
            }
        }
        //Debug.Log("highest point is: " + highestPoint);
        return highestPoint;
    }
}
