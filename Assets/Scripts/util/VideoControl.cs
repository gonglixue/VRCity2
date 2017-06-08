using UnityEngine;
using System.Collections;

public class VideoControl : MonoBehaviour {
    public MovieTexture mp4;
	// Use this for initialization
	void Start () {
        this.GetComponent<MeshRenderer>().material.mainTexture = mp4;
        mp4.loop = true;
        ((MovieTexture)GetComponent<Renderer>().material.mainTexture).Play();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
