using UnityEngine;
using System.Collections;

public class configTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            Test();

        this.transform.localEulerAngles = this.transform.localEulerAngles + new Vector3(0, 0, 1);
    }

    void Test()
    {
        GoTweenConfig rotConfig = new GoTweenConfig()
            .localEulerAngles(Vector3.up * 10, true)  // 旋转了10
            .setEaseType(GoEaseType.Linear);

        Go.to(this.transform, 0.8f, rotConfig);
    }
}
