using UnityEngine;
using System.Collections;

public class SandboxBillboardController : MonoBehaviour {
    public Transform eye;
    public Transform scriptHolder;
    public Transform border;
    public Transform board;
    private bool _isFocus = false;
    private Vector3 _originScale;

    private GoTween _biggerTween = null;
    private GoTween _smallerTween = null;

	// Use this for initialization
	void Start () {
        _originScale = board.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 forward = eye.position - this.transform.position;
        forward = new Vector3(forward.x, 0, forward.z);
        Quaternion newQ = Quaternion.LookRotation(-forward, Vector3.up);
        Vector3 parentRotEuler = this.transform.parent.localEulerAngles;
        Quaternion reverseParentRot = Quaternion.AngleAxis(-parentRotEuler.y, Vector3.up);
        this.transform.localRotation = reverseParentRot * newQ;
	}

    // 射线选择该billboard，
    public void FocusThis()
    {
        if(!_isFocus)
        {
            _isFocus = true;
            if (_biggerTween != null)  //已经正在变大
                return;

            border.gameObject.SetActive(false);
            GoTweenConfig config = new GoTweenConfig().
            scale( _originScale * 4)
            .setEaseType(GoEaseType.CubicInOut);

            config.onComplete(delegate (AbstractGoTween obj)
            {             
                HideOthers();
                _biggerTween = null;
            });
            _biggerTween = Go.to(board, 0.8f, config);
        }
        else
        {
            // cancel focus
            _isFocus = false;

            if (_smallerTween != null)
                return;
            GoTweenConfig config = new GoTweenConfig()
                .scale(_originScale)
                .setEaseType(GoEaseType.CubicInOut);

            config.onComplete(delegate (AbstractGoTween obj)
            {
                // 其他复原
                ShowOthers();
                _smallerTween = null;
            });

            _smallerTween = Go.to(board, 0.8f, config);
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
