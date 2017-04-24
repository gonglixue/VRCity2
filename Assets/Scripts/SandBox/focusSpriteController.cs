using UnityEngine;
using System.Collections;

public class focusSpriteController : MonoBehaviour {
    public Transform screenCenter;
	// Use this for initialization
	void Start () {
        Vector3 temp = this.transform.position - screenCenter.transform.position;
        this.transform.forward = new Vector3(temp.x, this.transform.forward.y, temp.z);
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.localEulerAngles = this.transform.localEulerAngles + new Vector3(0, 0, 50*Time.deltaTime);
        
	}
}
