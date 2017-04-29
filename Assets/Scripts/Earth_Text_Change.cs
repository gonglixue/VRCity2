using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Earth_Text_Change : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void EaseHideText(string newText)
    {
        GoTweenConfig hideConf = new GoTweenConfig()
            .colorProp("color", new Color(1, 1, 1, 0))
            .setEaseType(GoEaseType.Linear);

        hideConf.onComplete(delegate (AbstractGoTween obj)
        {
            this.GetComponent<Text>().text = newText;
            EaseShowText();
        });

        Go.to(this.GetComponent<Text>(), 0.8f, hideConf);
    }

    void EaseShowText()
    {
        GoTweenConfig showConf = new GoTweenConfig()
            .colorProp("color", new Color(1, 1, 1, 1))
            .setEaseType(GoEaseType.Linear);

        Go.to(this.GetComponent<Text>(), 0.8f, showConf);
    }

    public void ChangeBtnText(string newText)
    {
        this.GetComponent<Text>().text = newText;
    }

}
