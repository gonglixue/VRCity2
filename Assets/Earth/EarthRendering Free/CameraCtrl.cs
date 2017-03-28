using UnityEngine;
using System.Collections;

public class CameraCtrl : MonoBehaviour
{
	public Transform light = null;
	public MeshRenderer earthRenderer = null;
	public MeshRenderer atmosphereRenderer = null;

	float MIN_DIST = 200;
	float MAX_DIST = 5000;

	float dist = 400;
	Quaternion cameraRotation;
	Vector2 targetOffCenter = Vector2.zero;
	Vector2 offCenter = Vector2.zero;

	// Use this for initialization
	void Start()
	{
		cameraRotation = Quaternion.LookRotation(-transform.position.normalized, Vector3.up);  // Vector3.up = (0,1,0)
	}

	// Update is called once per frame
	void Update()
	{
		float wheelDelta = Input.GetAxis("Mouse ScrollWheel");
		if (wheelDelta > 0)
		{
			dist *= 0.87f;
            Debug.Log("wheelDelta > 0");
		}
		else if (wheelDelta < 0)
		{
			dist *= 1.15f;
            Debug.Log("wheelDelta < 0");
		}
		if (dist < MIN_DIST)
		{
			dist = MIN_DIST;
		}
		else if (dist > MAX_DIST)
		{
			dist = MAX_DIST;
		}
		float xMove = Input.GetAxis("Mouse X");  //delta
		float yMove = Input.GetAxis("Mouse Y");

		float targetRadius = 100;

        // 旋转地球
		if (Input.GetButton("Fire1"))
		{
			if (xMove != 0 || yMove != 0)
			{
				float rotateSensitivity = Mathf.Min(2f, (float)((dist - targetRadius) / targetRadius) * 1.2f);
				cameraRotation *= (Quaternion.AngleAxis(rotateSensitivity * xMove, Vector3.up) *
									Quaternion.AngleAxis(rotateSensitivity * yMove, Vector3.left));
                // Quaternion.AngleAxis：绕(世界坐标)轴旋转角度
                // 四元数的乘法顺序？
			}
		}
        // 旋转光照
		else if (Input.GetButton("Fire2"))
		{
			Quaternion lightRotation = light.rotation;
			lightRotation *= Quaternion.AngleAxis(xMove * 2, Vector3.up);
			light.rotation = lightRotation;
		}
		else if (Input.GetButton("Fire3"))
		{
			const float MOUSE_TRANSLATE_SENSITIVITY = 10;
			targetOffCenter.x -= xMove * MOUSE_TRANSLATE_SENSITIVITY;
			targetOffCenter.y -= yMove * MOUSE_TRANSLATE_SENSITIVITY;

			float translateMultiply = 0.5625f * Screen.width / Screen.height * Mathf.Tan(GetComponent<Camera>().fieldOfView / 2) * dist / Screen.height / 2.5f;
			offCenter.x = targetOffCenter.x * translateMultiply;
			offCenter.y = targetOffCenter.y * translateMultiply;
		}

		transform.rotation = cameraRotation;

		transform.position = cameraRotation * (-Vector3.forward * dist);  //从local转到global
        // 这段是干嘛的？
		//transform.position += new Vector3(transform.right.x * offCenter.x + transform.up.x * offCenter.y,
		//									transform.right.y * offCenter.x + transform.up.y * offCenter.y,
		//									transform.right.z * offCenter.x + transform.up.z * offCenter.y);

		Vector3 lightDir = Quaternion.Inverse(light.rotation) * Vector3.forward;
		earthRenderer.material.SetVector("_LightDir", lightDir);
		atmosphereRenderer.material.SetVector("_LightDir", lightDir);
	}
}
