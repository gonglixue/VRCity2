using UnityEngine;
using System.Collections;

public class TransformFindTest : MonoBehaviour {

    public Transform zero;
    public Transform one;
    public GameObject prefabZero;
    public GameObject prefabOne;

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

        TranformTest();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void TranformTest()
    {
        GameObject zeroChild = Instantiate(prefabZero) as GameObject;
        GameObject oneChild = Instantiate(prefabOne) as GameObject;

        zeroChild.transform.position = new Vector3(0, 0, 0);
        zeroChild.transform.parent = zero;

        oneChild.transform.position = new Vector3(0, 0, 0);
        oneChild.transform.parent = one;
    }
}
