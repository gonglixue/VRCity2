using UnityEngine;
using System.Collections;

public class SanxboxBillboardController : MonoBehaviour {
    public Transform eye;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 forward = eye.position - this.transform.position;
        forward = new Vector3(forward.x, 0, forward.z);
        Quaternion newQ = Quaternion.LookRotation(-forward, Vector3.up);
        this.transform.localRotation = newQ;
	}
}
