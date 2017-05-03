using UnityEngine;
using System.Collections;

public class SpriteTriggerReceiver : MonoBehaviour {
    public Sprite subSprite;
    public GameObject monthDataCtr;
    private Sprite _originSprite;

    private GoTween _BiggerTween = null;  // 放大
    private bool _isHover = false;
    private Vector3 _originSize;
    public float scaleFactor = 1.3f;

	// Use this for initialization
	void Start () {
        _originSprite = this.GetComponent<SpriteRenderer>().sprite;
        _originSize = this.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnHover()
    {
        if(!_isHover)
        {
            _BiggerTween = Go.to(this.transform, 0.8f,
                new GoTweenConfig()
                .scale(_originSize * scaleFactor)
                .setEaseType(GoEaseType.CubicInOut));

            this.GetComponent<SpriteRenderer>().sprite = subSprite;
            _isHover = true;
        }
        
    }

    public void OnLeave()
    {
        if(_isHover)
        {
            if(_BiggerTween != null)
            {
                _BiggerTween.complete();
                _BiggerTween.destroy();
                _BiggerTween = null;
            }

            this.transform.localScale = _originSize;
            this.GetComponent<SpriteRenderer>().sprite = _originSprite;
            _isHover = false;
        }
    }

    public void TriggerUse()
    {
        ShutDownScreen();
    }

    void ShutDownScreen()
    {
        monthDataCtr.GetComponent<monthDataController>().HideScreen();
    }
 
}
