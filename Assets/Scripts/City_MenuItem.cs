using UnityEngine;
using System.Collections;

public class City_MenuItem : MonoBehaviour {
    public enum CityMenuItemName
    {
        BackToCity = 0,
        Sandbox = 1,
        BackToHome = 2,
        About = 3

    }

    public CityMenuItemName menuItemName;
    public GameObject cameraRig;
    public GameObject rightController;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ConfirmChoose()
    {
        switch(this.menuItemName)
        {
            case CityMenuItemName.BackToCity:
                rightController.GetComponent<VRTK.Examples.VRCityControllerListener>().BackToCity();
                
                break;
            case CityMenuItemName.Sandbox:
                GotoSandbox();
                break;
            case CityMenuItemName.BackToHome:
                BackToHome();
                break;
            case CityMenuItemName.About:
                break;
        }
    }

    void GotoSandbox()
    {
        // TODO
        // 场景切换
    }
    void BackToHome()
    {
        // TODO
        // 场景切换
    }
}
