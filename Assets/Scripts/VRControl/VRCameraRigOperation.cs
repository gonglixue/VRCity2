using UnityEngine;
using System.Collections;

public class VRCameraRigOperation : MonoBehaviour {

    #region movement control     
    private bool activeMovement = false;
    private Vector3 movement = new Vector3(0, 0, 0);
    public float speed = 20.0f;
    public GameObject eyeCamera;
    #endregion
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(activeMovement)
        {
            Debug.Log("move");
            transform.position += movement;
        }
	}

    public void move(Vector3 m)
    {
        movement = m;
    }

    public void activeMove(Vector3 m)
    {
        m = m * speed;
        m = Vector3.ClampMagnitude(m, speed);
        m *= Time.deltaTime;
        movement = eyeCamera.transform.TransformDirection(m);

        activeMovement = true;
    }

    public void inactiveMove()
    {
        activeMovement = false;
    }
}
