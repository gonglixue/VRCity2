using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Earth_UI_Interactions : MonoBehaviour {

    public GameObject HintText;
    public GameObject StartBtnText;
    public GameObject ControllerLeft;
    public GameObject UILayer;
    public GameObject EyeTip;

    private int status = 0;

    [SerializeField]
    private string newText = "Congratulations! You have learned how to use pointer!\nNow start your exploring";
    [SerializeField]
    private string newBtnText = "Start";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ClickHaveTry()
    {
        if(status == 0)
        {
            Debug.Log("Have a try");

            HintText.GetComponent<Earth_Text_Change>().EaseHideText(newText);
            StartBtnText.GetComponent<Earth_Text_Change>().ChangeBtnText(newBtnText);

            status++;
        }
        else if(status == 1)
        {
            ControllerLeft.GetComponent<VRTK.Examples.VREarth_ControllerEventListener>().enabled = true;
            // TODO 隐藏UILayer
            GoTweenConfig UI_hideConf = new GoTweenConfig()
                .scale(new Vector3(UILayer.transform.localScale.x, 0, UILayer.transform.localScale.z))
                .setEaseType(GoEaseType.CubicInOut);
            UI_hideConf.onComplete(delegate (AbstractGoTween obj)
            {
                // TODO 显示放置pin的提示
                // ShowEyeTip();
            });

            Go.to(UILayer.transform, 0.8f, UI_hideConf);

            
            status++;
        }
        
        
    }

    public void ShowEyeTip()
    {
        Debug.Log("show eye tip");
        GoTweenConfig EyeTip_ShowConf = new GoTweenConfig()
            .scale(new Vector3(EyeTip.transform.localScale.x, 1, EyeTip.transform.localScale.z))
            .setEaseType(GoEaseType.CubicInOut);

        EyeTip_ShowConf.onComplete(delegate (AbstractGoTween obj)
        {
            //
        });

        Go.to(EyeTip.transform, 0.8f, EyeTip_ShowConf);
    }

}
