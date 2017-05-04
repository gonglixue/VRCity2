using UnityEngine;
using System.Collections;

public class baseDataInfo : MonoBehaviour {
    public Vector3 localPosition = new Vector3(0, 0, 0);
    public Vector3 localScale = new Vector3(0.7f, 10, 0.7f);

    static float maxHeight = 20f;

    private float _value;
    public GameObject cylinderPrefab;
    private GameObject _cylinderRefer = null;

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
        //Vector3 cylinderLocalPos = this.transform.parent.localToWorldMatrix.MultiplyPoint(this.transform.position);
        Vector3 cylinderLocalPos = this.transform.localToWorldMatrix.MultiplyPoint(localPosition);
        if(!_cylinderRefer)
        {
            _cylinderRefer = Instantiate(cylinderPrefab, cylinderLocalPos, Quaternion.identity, this.transform) as GameObject;
        }
        
        float scaleY = maxHeight / this.transform.parent.GetComponent<classGroupController>().maxBaseData * data;
        _cylinderRefer.transform.localScale = new Vector3(localScale.x, 0, localScale.z);

        GoTweenConfig config = new GoTweenConfig()
            .scale(new Vector3(localScale.x, scaleY, localScale.z))
            .setEaseType(GoEaseType.CubicInOut);

        Go.to(_cylinderRefer.transform, 1.0f, config);
    }

    public void HideCylinder()
    {
        GoTweenConfig config = new GoTweenConfig()
            .scale(new Vector3(localScale.x, 0, localScale.z))
            .setEaseType(GoEaseType.CubicInOut);
        Go.to(_cylinderRefer.transform, 1.0f, config);
    }
}
