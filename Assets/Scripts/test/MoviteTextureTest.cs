using UnityEngine;
using System.Collections;

public class MoviteTextureTest : MonoBehaviour {
    public MovieTexture movie;
	// Use this for initialization
	void Start () {
        this.GetComponent<MeshRenderer>().material.mainTexture = movie;
        movie.Play();
        movie.loop = true;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
