using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterManager : MonoBehaviour {
	[SerializeField] Transform targetPoint = null;				// ショットが行われる目的位置
	[SerializeField] List<GameObject> bulPrefabs = null;		// 弾プレハブリスト
	[SerializeField] List<ShooterPoint> shooterPoints = null;	// ショット地点リスト
	[SerializeField] GameObject holdTarget = null;              // 保留中のターゲット
	public GameObject HoldTarget {
		get { return holdTarget; }
		set { holdTarget = value;
			Player pl = GetComponent<Player>();
			if(pl == null) {
				Debug.LogError("Playerコンポーネントが取得できませんでした。\n" +
				"name:" + name + " position:" + transform.position);
				return;
			}
			if (value != null) {
				// 追従するオブジェクトを有効化し、追従を設定
				HoldTargetFollowObj.SetActive(true);
				HoldTargetFollowObj.transform.parent = value.transform;
				HoldTargetFollowObj.transform.localPosition = Vector3.zero;
			}
			else {
				// 追従するオブジェクトを無効化
				HoldTargetFollowObj.SetActive(false);
			}
		}
	}

	[SerializeField] GameObject holdTargetFollowObj = null;    // ホールド中のオブジェクトに追従するオブジェクト
	public GameObject HoldTargetFollowObj { get { return holdTargetFollowObj; } }

	// Use this for initialization
	//	void Start() {}

	// Update is called once per frame
	void Update() {
		// ショット目的位置が設定されていなければ処理しない
		if (targetPoint == null) return;

		// 射線をデバッグ表示
		Debug.DrawLine(transform.position, targetPoint.position);
	}

	public void Shot(Bullet.Type _bulType) {
		Debug.Log("Shot BulType:" + _bulType + "\n" +
			"name:" + name + " position:" + transform.position);

		// 弾のプレハブが正常に設定されていない場合
		if (bulPrefabs == null) {
			Debug.LogError("bulPrefabsがnullです。\n" +
				"name:" + name + " position:" + transform.position);
			return;
		}
		if (bulPrefabs.Count < (int)_bulType) {
			Debug.LogError("bulPrefabsに" + _bulType + "に対応する要素が存在しません。\n" +
				"name:" + name + " position:" + transform.position);
			return;
		}

		ShooterPoint shooterPoint = null;
		for (int idx = 0; idx < shooterPoints.Count; idx++) {
			// ショット可能状態のショット地点が見つかれば
			if (shooterPoints[idx].IsStandby) {
				shooterPoint = shooterPoints[idx];
				break;
			}
		}
		// ショット可能状態のショット地点が見つからなければ処理しない
		if (shooterPoint == null) {
			Debug.Log("ショット可能状態のショット地点が見つかりませんでした。" +
				"name:" + name + " position:" + transform.position);
			return;
		}

		// 弾のプレハブを実体化
		GameObject bul = Instantiate(bulPrefabs[(int)_bulType]);

		// 弾に発生源オブジェクトを設定
		bul.GetComponent<Bullet>().ShooterMng = this;
		
		// 弾の発生位置を設定
		bul.transform.position = shooterPoint.transform.position;

		// 実体化した弾の向きを照準方向に向ける
		bul.transform.rotation = Quaternion.LookRotation(targetPoint.position - bul.transform.position);

		// 次回のショット地点を変更
		shooterPoints.Remove(shooterPoint);
		shooterPoints.Add(shooterPoint);
	}
}
