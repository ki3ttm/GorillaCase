using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// targetにz軸正方向を向ける
public class LookTarget : MonoBehaviour {
	[SerializeField] Transform target = null;	// 注視対象
	public Transform Target { get { return target; } set { target = value; } }

	[SerializeField] Quaternion defRot = Quaternion.identity;   // 注視前の向き
	[SerializeField] bool defFlg = false;						// 注視前の向きを保持しているフラグ

	// Use this for initialization
	//	void Start () {}

	// Update is called once per frame
	void Update() {
		// 注視対象が設定されていれば
		if (target != null) {
			// 注視前の向きが保持されていなければ
			if (!defFlg) {
				// 注視前の向きを保持
				defFlg = true;
				defRot = transform.rotation;
			}

			// 注視対象にz方向を向ける
			transform.rotation = Quaternion.LookRotation(target.position - transform.position);
		}
		// 注視対象が設定されていなければ
		else {
			// 向きが保持されていれば
			if (defFlg) {
				// 保持していた注視前の向きに戻す
				defFlg = false;
				transform.rotation = defRot;
			}
		}
	}
}
