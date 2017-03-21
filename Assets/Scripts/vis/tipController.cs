﻿using UnityEngine;
using System.Collections;

public class tipController : MonoBehaviour {

    private Material m;
    public float speed = 8.0f;
    private float threshold = 0.0f;
    public GameObject eye;

    void Awake()
    {
        eye = GameObject.Find("Camera");
    }
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

        // look at camera
        Vector3 targetUp = eye.transform.position - this.transform.position;
        Quaternion newQ = Quaternion.LookRotation(Vector3.up, targetUp);
        this.transform.localRotation = newQ;

        
	}

    public void SetText(string content)
    {
        Transform textChild = this.transform.GetChild(0);
        textChild.GetComponent<TextMesh>().text = content;
    }
}
