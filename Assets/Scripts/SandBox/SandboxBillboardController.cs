using UnityEngine;
using System.Collections;

public class SandboxBillboardController : MonoBehaviour {
    public Transform eye;
    public Transform scriptHolder;
    public Transform border;
    public Transform board;
    private bool _isFocus = false;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 forward = eye.position - this.transform.position;
        forward = new Vector3(forward.x, 0, forward.z);
        Quaternion newQ = Quaternion.LookRotation(-forward, Vector3.up);
        this.transform.localRotation = newQ;
	}

    // 射线选择该billboard，
    public void FocusThis()
    {
        if(!_isFocus)
        {
            _isFocus = true;
            border.gameObject.SetActive(false);
            GoTweenConfig config = new GoTweenConfig().
            scale(board.localScale * 4)
            .setEaseType(GoEaseType.CubicInOut);

            config.onComplete(delegate (AbstractGoTween obj)
            {             
                HideOthers();
            });
            Go.to(board, 0.8f, config);
        }
        else
        {
            // cancel focus
            _isFocus = false;
            GoTweenConfig config = new GoTweenConfig()
                .scale(border.localScale / 4.0f)
                .setEaseType(GoEaseType.CubicInOut);

            config.onComplete(delegate (AbstractGoTween obj)
            {
                // 其他复原
                
            });
            Go.to(board, 0.8f, config);
        }
        
    }

    public void HideOthers()
    {
        scriptHolder.GetComponent<AllBillboardManager>().Choose(this.gameObject.name);
    }
    public void ShowOthers()
    {
        scriptHolder.GetComponent<AllBillboardManager>().ActiveAll();
    }

    public void Hover()
    {
        border.gameObject.SetActive(true);
    }
    public void Leave()
    {
        border.gameObject.SetActive(false);
    }
    
}
