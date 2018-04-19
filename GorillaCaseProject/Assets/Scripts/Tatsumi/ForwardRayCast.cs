using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardRayCast : MonoBehaviour {
	[SerializeField] GameObject hitObj = null;  // 照準が当たっているオブジェクト
	public GameObject HitObj { get { return hitObj; } private set { hitObj = value; } }
	[SerializeField] float maxDis = 100.0f;		// 最大有効距離
	private float MaxDis { get { return maxDis; } }
	[SerializeField] Transform endPoint = null;	// 照準オブジェクト
	public Transform EndPoint { get { return endPoint; } private set { endPoint = value; } }
	[SerializeField] GameObject validEnableObj = null;
	[SerializeField] GameObject invalidEnableObj = null;

	// Use this for initialization
	//	void Start () {}

	// Update is called once per frame
	void Update () {
		RaycastHit hitInfo;
		Ray ray = new Ray(transform.position, transform.forward);
		// hitがあった場合
		bool isHit = Physics.Raycast(ray.origin, ray.direction, out hitInfo, MaxDis);
		if (isHit) {
			// 対象オブジェクトを保持
			HitObj = hitInfo.collider.gameObject;

			// hit位置に照準オブジェクトを移動
			EndPoint.position = hitInfo.point;

			// Rayの命中位置までを赤色、それ以降を黄色でデバッグ表示
			Debug.DrawRay(ray.origin, (ray.direction * hitInfo.distance), Color.red);
			Debug.DrawRay(hitInfo.point, (ray.direction * (MaxDis - hitInfo.distance)), Color.yellow, 0.0f, false);
		}
		// hitがなかった場合
		else {
			// 対象オブジェクトをnullに
			HitObj = null;

			// 照準の先に照準オブジェクトを移動
			EndPoint.position = (ray.origin + ray.direction * MaxDis);

			// Rayを青色でデバッグ表示
			Debug.DrawRay(ray.origin, (ray.direction * MaxDis), Color.blue);
		}

		// 照準時オブジェクトと非照準時オブジェクトの有効化/無効化
		validEnableObj.SetActive(isHit);	// 照準時に有効化
		invalidEnableObj.SetActive(!isHit);	// 非照準時に有効化
	}
}
