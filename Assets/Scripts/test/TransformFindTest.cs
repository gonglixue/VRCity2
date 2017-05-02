using UnityEngine;
using System.Collections;

public class TransformFindTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    foreach(Transform child in this.transform)
        {
            Debug.Log(child.name);
        }

        Transform findTest = this.transform.Find("child2");
        if(findTest)
        {
            Debug.Log("find" + findTest.name);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
