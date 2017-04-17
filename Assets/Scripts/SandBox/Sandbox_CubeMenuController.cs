using UnityEngine;
using System.Collections;

public class Sandbox_CubeMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void SwipeLeft()
    {
        Vector3 currentEulerAngle = this.transform.localEulerAngles;
        GoTweenConfig config = new GoTweenConfig()
            .localEulerAngles(currentEulerAngle + new Vector3(0, 90, 0))
            .setEaseType(GoEaseType.CubicInOut);

        Go.to(this.transform, 0.6f, config);
            
    }

    public void SwipeRight()
    {
        Vector3 currentEulerAngle = this.transform.localEulerAngles;
        GoTweenConfig config = new GoTweenConfig()
            .localEulerAngles(currentEulerAngle + new Vector3(0, -90, 0))
            .setEaseType(GoEaseType.CubicInOut);
        Go.to(this.transform, 0.6f, config);
    }

    void Test()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SwipeLeft();
        }
    }
}
