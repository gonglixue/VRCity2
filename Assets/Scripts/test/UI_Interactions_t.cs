using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Interactions_t : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ClickExampleBtn()
    {
        Debug.Log("click example btn");
    }

    public void PointerEnterBtn()
    {
        Debug.Log("pointer enter btn");
    }

    public void PointerExitBtn()
    {
        Debug.Log("pointer exit btn");
    }
}
