using UnityEngine;
using System.Collections;

public class AllBillboardManager : MonoBehaviour {
    public GameObject[] allBillboards;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Choose(string name)
    {
        foreach(GameObject item in allBillboards)
        {
            if(item.name != name)
            {
                item.SetActive(false);
            }
        }
    }

    public void ActiveAll()
    {
        foreach(GameObject item in allBillboards)
        {
            item.SetActive(true);
        }
    }
}
