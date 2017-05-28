using UnityEngine;
using System.Collections;

public class EaseSpriteFrame : MonoBehaviour {
    [SerializeField]
    private float _alpha;
    private bool _active = false;
    public float easeFactor = 10f;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (_active)
        {
            if (_alpha < 1.0f)
            {
                _alpha += Time.deltaTime * easeFactor;
                this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, _alpha);
                //Debug.Log("chang alpha:" + _alpha);
            }
        }
	    
	}

    public void ActiveFrame()
    {
        _alpha = this.GetComponent<SpriteRenderer>().color.a;
        _active = true;
    }

    public void InactiveFrame()
    {
        _alpha = 0f;
        _active = false;
        this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
}
