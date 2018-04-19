using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMousePointer : MonoBehaviour {
	[SerializeField]
	Vector3 mousePos = Vector3.zero;
	[SerializeField]
	Transform targetPoint = null;

	// Use this for initialization
	//	void Start () {}

	// Update is called once per frame
	void Update() {
		func();
	}

	void func() {
		//		targetPoint.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0.0f, 0.0f, -Camera.main.transform.position.z));	
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//		Debug.DrawRay(ray.origin, ray.direction);
		//		RaycastHit hitInfo;
		//		Physics.Raycast(ray, out hitInfo);
		//		targetPoint.position = hitInfo.point;
		Plane plane = new Plane(new Vector3(0.0f, 0.0f, -1.0f), 0.0f);
		float enter = 0.0f;
		if (plane.Raycast(ray, out enter)) {
			targetPoint.position = ray.GetPoint(enter);
		}
	}
}
