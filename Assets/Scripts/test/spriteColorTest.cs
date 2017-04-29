using UnityEngine;
using System.Collections;

public class spriteColorTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKey(KeyCode.C))
        {
            Colortransform();
        }
	}

    void Colortransform()
    {
        Debug.Log("change color");
        GoTweenConfig testConf = new GoTweenConfig()
            .colorProp("color", new Color(0, 0, 0))
            .setEaseType(GoEaseType.Linear);

        Go.to(this.GetComponent<SpriteRenderer>(), 2.0f, testConf);

    }
}
