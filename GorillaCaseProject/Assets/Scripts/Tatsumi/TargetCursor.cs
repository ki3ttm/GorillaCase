using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCursor : MonoBehaviour {
	[SerializeField] Vector3 mousePos = Vector3.zero;
	[SerializeField] Transform targetPoint = null;
	[SerializeField] float keyCtrlSensitivityV = 1.0f;
	[SerializeField] float keyCtrlSensitivityH = 1.0f;
	bool mouseCtrlFlg = true;
	Vector3 prevMousePos = Vector3.zero;

	// Use this for initialization
	//	void Start () {}

	// Update is called once per frame
	void Update() {
		// 処理前の位置を保持
		Vector3 prevObjPos = transform.position;

		// マウス以外での移動
		targetPoint.position += new Vector3(Input.GetAxis("SubHorizontal") * keyCtrlSensitivityH, Input.GetAxis("SubVertical") * keyCtrlSensitivityV, 0.0f);

		// 位置が変化している場合
		if (prevObjPos != targetPoint.position) {
			// キーでの操作中
			mouseCtrlFlg = false;
		}

		// キーでの操作中
		if (!mouseCtrlFlg) {
			// マウス位置に変化があれば
			if(prevMousePos != Input.mousePosition) {
				// マウス操作
				mouseCtrlFlg = true;
			}
		}

		// マウスでの移動
		if (mouseCtrlFlg) {
			// マウス位置にオブジェクトを移動
			FollowMousePosition();
		}

		// マウス位置を保持
		prevMousePos = Input.mousePosition;
	}
	void FollowMousePosition() {
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
