using UnityEngine;
using System.Collections;

public class frameTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.transform.Find("BlueFrameGroup").gameObject.SetActive(true);
        Debug.Log("set active");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
