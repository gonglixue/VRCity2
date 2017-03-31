using UnityEngine;
using System.Collections;

public class PinController : MonoBehaviour {
    public Transform cameraRig;

	// Use this for initialization
	void Start () {
        cameraRig = GameObject.Find("[CameraRig]").transform;
	}
	
	// Update is called once per frame
	void Update () {
        // transform 0.08 - 0.04 根据距离更新pin的大小
        // 800 -400
        if(cameraRig.transform.position.z >= 400)
        {
            this.transform.localScale = Vector3.one * cameraRig.position.z / 1000;
        }
	}
}
