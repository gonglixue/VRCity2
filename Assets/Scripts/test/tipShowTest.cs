using UnityEngine;
using System.Collections;

public class tipShowTest : MonoBehaviour {
    private Material m;
    public float speed = 0.1f;
    private float threshold = 0.0f;
	// Use this for initialization
	void Start () {
        m = this.GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        
	    if(threshold < 10)
        {
            threshold += speed * Time.deltaTime;
            m.SetFloat("_Factor", threshold);
        }
	}
}
