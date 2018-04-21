using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLine : MonoBehaviour {
	[SerializeField] Transform fromPoint = null;
	[SerializeField] Transform toPoint = null;
	[SerializeField] GameObject markPrefab = null;
	[SerializeField] float markInterval = 1.0f;
	[SerializeField] float maxLen = 100.0f;
	[SerializeField] Player player = null;
	List<GameObject> markList = new List<GameObject>();

	[SerializeField] bool isTargetOver = true;

	// Use this for initialization
	//	void Start () {}
		
	// Update is called once per frame
	void Update() {
		if (fromPoint == null) {
			Debug.LogError("fromPointが設定されていません。\n" + MessageLog.GetNameAndPos(gameObject));
			return;
		}
		if (toPoint == null) {
			Debug.LogError("toPointが設定されていません。\n" + MessageLog.GetNameAndPos(gameObject));
			return;
		}
		if (markPrefab == null) {
			Debug.LogError("markPrefabが設定されていません。\n" + MessageLog.GetNameAndPos(gameObject));
			return;
		}
		if (player == null) {
			Debug.LogError("playerが設定されていません。\n" + MessageLog.GetNameAndPos(gameObject));
			return;
		}

		// ショット不可時なら
		if(!player.ShotFlg) {
			// 点を全削除
			while(markList.Count > 0) {
				Destroy(markList[markList.Count - 1]);
				markList.RemoveAt(markList.Count - 1);
			}
			return;
		}

		// 方向を取得
		Vector3 vec = (toPoint.position - fromPoint.position).normalized;

		// 壁までの距離を取得
		float dis = 0.0f;

		// 照準までの距離
		if (!isTargetOver) {
			dis = Vector3.Distance(fromPoint.position, toPoint.position);
		}
		else {
			// 照準方向の壁までの距離
			RaycastHit hitInfo;
			Physics.Raycast(fromPoint.position, vec, out hitInfo, maxLen, LayerMask.GetMask(new string[] { "Stage" }));
			dis = hitInfo.distance;
		}
		// 点の数を求める
		int markNum = (int)(dis / markInterval) + 1;

		// リストのサイズを変更
		while (markList.Count < markNum) {
			// 追加
			markList.Add(Instantiate(markPrefab, transform));
			markList[markList.Count - 1].transform.rotation = Quaternion.identity;
		}
		 while (markList.Count > markNum) {
			// 削除
			Destroy(markList[markList.Count - 1]);
			markList.RemoveAt(markList.Count - 1);
		}

		// 各点の位置を設定
		for(int idx = 0; idx < markList.Count; idx++) {
			markList[idx].transform.position = (fromPoint.position + (vec * markInterval * idx));
		}
	}
}
