using UnityEngine;
using System.Collections;

public class Vis_UI_Interactions : MonoBehaviour {
    #region dropdown
    public GameObject MonthDataCtr;
    #endregion
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DropDown(int value)
    {
        Debug.Log("screen ui: dropdown " + value);
        if(value == 1)
        {
            MonthDataCtr.GetComponent<monthDataController>().ReAlignDots();
        }
    }
}
