using UnityEngine;
using System.Collections;

public class baseDataInfo : MonoBehaviour {
    public Vector3 localPosition = new Vector3(0, 0, 0);
    public Vector3 localScale = new Vector3(0.7f, 10, 0.7f);

    static float maxHeight = 25f;

    private float _value;
    public GameObject cylinderPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void InitData(float data)
    {
        _value = data;
        CreateCylinder(data);
    }

    void CreateCylinder(float data)
    {
        Vector3 cylinderLocalPos = this.transform.parent.localToWorldMatrix.MultiplyPoint(this.transform.position);
        GameObject cylinder = Instantiate(cylinderPrefab, cylinderLocalPos, Quaternion.identity, this.transform) as GameObject;
        float scaleY = maxHeight / this.transform.parent.GetComponent<classGroupController>().maxBaseData * data;
        cylinder.transform.localScale = new Vector3(localScale.x, scaleY, localScale.z);
    }
}
