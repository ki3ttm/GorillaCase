using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	[SerializeField] float walkSpd = 0.2f;			// 地上移動速度
	[SerializeField] float turnSpd = 2.0f;			// 向き変更速度
	[SerializeField] float jumpFirstSpd = 500.0f;	// ジャンプ初速

	[SerializeField] bool moveFlg = true;	// 移動可能フラグ
	public bool MoveFlg { get { return moveFlg; } private set { moveFlg = value; } }

	[SerializeField] bool jumpFlg = true;	// ジャンプ可能フラグ
	public bool JumpFlg { get { return jumpFlg; } private set { jumpFlg = value; } }

	[SerializeField] bool shotFlg = true;	// ショット可能フラグ
	public bool ShotFlg { get { return shotFlg; } private set { shotFlg = value; } }

	[SerializeField] Transform plEye = null;			// プレイヤーの目
	[SerializeField] bool isFreeFall = false;           // 落下・浮遊による操作不可フラグ

	WeightManager weightMng = null;     // 重さ管理コンポーネント
	ForceManager forceMng = null;       // 物理挙動コンポーネント
	ShooterManager shooterMng = null;   // ショット管理コンポーネント

	// Use this for initialization
	void Start () {
		// WeightManagerを取得
		if (weightMng == null) {
			weightMng = GetComponent<WeightManager>();
			if (weightMng == null) {
				Debug.LogError("WeightManagerが見つかりませんでした。\n" + MessageLog.GetNameAndPos(gameObject));
			}
		}

		// ForceManagerを取得
		if (forceMng == null) {
			forceMng = GetComponent<ForceManager>();
			if (forceMng == null) {
				Debug.LogError("ForceManagerが見つかりませんでした。\n" + MessageLog.GetNameAndPos(gameObject));
			}
		}

		// ShooterManagerを取得
		if (shooterMng == null) {
			shooterMng = GetComponent<ShooterManager>();
			if (shooterMng == null) {
				Debug.LogError("ShooterManagerが見つかりませんでした。\n" + MessageLog.GetNameAndPos(gameObject));
			}
		}
	}

	// Update is called once per frame
	void Update () {
		//test
		if (Input.GetKeyDown("1")) GetComponent<WeightManager>().WeightLv = WeightManager.Weight.flying;
		if (Input.GetKeyDown("2")) GetComponent<WeightManager>().WeightLv = WeightManager.Weight.light;
		if (Input.GetKeyDown("3")) GetComponent<WeightManager>().WeightLv = WeightManager.Weight.heavy;
		if (Input.GetKeyDown("4")) GetComponent<ForceManager>().AddForce(new Vector3(1000, 1000, 0));

		// 移動
		Move();

		// ジャンプ
		Jump();

		// ショット
		Shot();
	}

	void Move() {
		// 移動不可時は処理しない
		if (!MoveFlg) return;

		// 入力に応じて移動
		transform.position += (transform.forward * Input.GetAxis("Horizontal") * walkSpd);
		plEye.transform.Rotate(-Input.GetAxis("SubVertical") * turnSpd, 0.0f, 0.0f);
	}

	void Jump() {
		// 入力時以外は処理しない
		if (!Input.GetKeyDown(KeyCode.Space)) return;

		// 特定の重さなら処理しない
		if (weightMng.WeightLv == WeightManager.Weight.heavy) return;

		// ジャンプ不可時は処理しない
		if (!JumpFlg) return;

		// ジャンプ初速を設定
		forceMng.AddForce(new Vector3(0.0f, jumpFirstSpd, 0.0f));
	}

	void Shot() {
		// ショット不可時は処理しない
		if (!ShotFlg) return;

		// ショット処理
		if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Z)) {
			// pullショット	
			shooterMng.Shot(Bullet.Type.pull);
		}
		if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.X)) {
			// pushショット
			shooterMng.Shot(Bullet.Type.push);
		}
		if (Input.GetKeyDown(KeyCode.Mouse2) || Input.GetKeyDown(KeyCode.C)) {
			// holdショット
			shooterMng.Shot(Bullet.Type.hold);
		}
	}
}
