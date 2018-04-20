using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	[SerializeField] float walkSpd = 0.2f;				// 地上移動速度
	[SerializeField] float turnSpd = 2.0f;				// 向き変更速度
	[SerializeField] float jumpFirstSpd = 400.0f;		// ジャンプ初速
	[SerializeField] float revJumpFirstSpd = -200.0f;	// 反転時ジャンプ初速
	[SerializeField] float turnStartSpd = 0.03f;		// 振り向きに必要な速度
	Vector3 prevPos = Vector3.zero;						// 前回位置

	[SerializeField] bool moveFlg = true;	// 移動可能フラグ
	public bool MoveFlg { get { return moveFlg; } private set { moveFlg = value; } }

	[SerializeField] bool jumpFlg = true;	// ジャンプ可能フラグ
	public bool JumpFlg { get { return jumpFlg; } private set { jumpFlg = value; } }

	[SerializeField] bool shotFlg = true;	// ショット可能フラグ
	public bool ShotFlg { get { return shotFlg; } private set { shotFlg = value; } }

	[SerializeField] bool revFlg = false;   // 天井行動フラグ
	public bool RevFlg { get { return revFlg; } private set { revFlg = value; } }

	[SerializeField] bool isFreeFall = false;           // 落下・浮遊による操作不可フラグ

	WeightManager weightMng = null;     // 重さ管理コンポーネント
	ForceManager forceMng = null;       // 物理挙動コンポーネント
	ShooterManager shooterMng = null;   // ショット管理コンポーネント

	Vector3 rotVec = new Vector3(1.0f, 1.0f, 0.0f);	// 向き

	// Use this for initialization
	void Start () {
		// WeightManagerを取得
		if (weightMng == null) {
			weightMng = GetComponent<WeightManager>();
			if (weightMng == null) {
				Debug.LogError("WeightManagerが見つかりませんでした。\n" + MessageLog.GetNameAndPos(gameObject));
			}
		}

//		// ForceManagerを取得
//		if (forceMng == null) {
//			forceMng = GetComponent<ForceManager>();
//			if (forceMng == null) {
//				Debug.LogError("ForceManagerが見つかりませんでした。\n" + MessageLog.GetNameAndPos(gameObject));
//			}
//		}

		// ShooterManagerを取得
		if (shooterMng == null) {
			shooterMng = GetComponent<ShooterManager>();
			if (shooterMng == null) {
				Debug.LogError("ShooterManagerが見つかりませんでした。\n" + MessageLog.GetNameAndPos(gameObject));
			}
		}

		// 開始時の位置を前回位置を保持
		prevPos = transform.position;

		// 開始時に左向きならプレイヤーとしての向きを左に設定
		if (Vector3.Dot(transform.forward, Vector3.left) > 0.0f) {
			rotVec.x = -1.0f;
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

		// 回転
		Rotate();
	}

	void Move() {
		// 移動不可時は処理しない
		if (!MoveFlg) return;

		// 入力に応じて移動
		GetComponent<Rigidbody>().MovePosition(transform.position + (Vector3.right * Input.GetAxis("Horizontal") * walkSpd));
		//transform.position += (Vector3.right * Input.GetAxis("Horizontal") * walkSpd);
	}

	void Jump() {
		// 入力時以外は処理しない
		if (!Input.GetKeyDown(KeyCode.Space)) return;

		// ジャンプ不可時は処理しない
		if (!JumpFlg) return;

		// 重さによって挙動が変化
		switch (weightMng.WeightLv) {
		case WeightManager.Weight.flying:
			// 接地方向と逆方向にジャンプ
			GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, revJumpFirstSpd, 0.0f));
			break;
		case WeightManager.Weight.light:
			// 接地方向と逆方向にジャンプ
			GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, jumpFirstSpd, 0.0f));
			break;
		case WeightManager.Weight.heavy:
			break;
		default:
			break;
		}
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

	void Rotate() {
		Rigidbody rb = GetComponent<Rigidbody>();

		if (rb == null) {
			Debug.LogError("Rigidbodyが見つかりませんでした。\n" + MessageLog.GetNameAndPos(gameObject));
			return;
		}

		// 移動方向によって向きを設定
		Vector3 subPos = (prevPos - transform.position);
		if (subPos.x < -turnStartSpd) {
			rotVec.x = 1.0f;
		} else if (subPos.x > turnStartSpd) {
			rotVec.x = -1.0f;
		}

		// 前回位置を更新
		prevPos = transform.position;

		// 接地方向によって向きを設定
		if (weightMng.WeightLv == WeightManager.Weight.flying) {
			rotVec.y = 1.0f;
		} else {
			rotVec.y = 0.0f;
		}

		Debug.Log(rotVec);

		// 設定された向きにスラープ補間
		Quaternion qt = Quaternion.Euler(0.0f, 90.0f * rotVec.x, rotVec.y * 180.0f);
		transform.rotation = Quaternion.Slerp(transform.rotation, qt, 0.2f);
	}

}
