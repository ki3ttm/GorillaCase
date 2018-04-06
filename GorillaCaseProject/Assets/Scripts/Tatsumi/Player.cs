using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	[SerializeField] float walkSpd = 0.2f;	// 地上移動速度
	[SerializeField] float turnSpd = 2.0f;	// 向き変更速度

	[SerializeField] float jumpVel = 0.0f;			// ジャンプ移動量
	[SerializeField] float jumpFirstSpd = 1.0f;		// ジャンプ初速
	[SerializeField] float jumpResistSpd = 0.1f;	// ジャンプ減衰

	[SerializeField] bool moveFlg = true;	// 移動可能フラグ
	public bool MoveFlg { get { return moveFlg; } private set { moveFlg = value; } }

	[SerializeField] bool jumpFlg = true;	// ジャンプ可能フラグ
	public bool JumpFlg { get { return jumpFlg; } private set { jumpFlg = value; } }

	[SerializeField] bool shotFlg = true;	// ショット可能フラグ
	public bool ShotFlg { get { return shotFlg; } private set { shotFlg = value; } }

	[SerializeField] Transform plEye = null;			// プレイヤーの目
	[SerializeField] ShooterManager shooterMng = null;	// ショット管理コンポーネント
	[SerializeField] bool isFreeFall = false;			// 落下・浮遊による操作不可フラグ

	// Use this for initialization
	//	void Start () {}

	// Update is called once per frame
	void Update () {
		//test
		if (Input.GetKeyDown("1")) GetComponent<WeightManager>().WeightLv = WeightManager.Weight.flying;
		if (Input.GetKeyDown("2")) GetComponent<WeightManager>().WeightLv = WeightManager.Weight.light;
		if (Input.GetKeyDown("3")) GetComponent<WeightManager>().WeightLv = WeightManager.Weight.heavy;

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
		// ジャンプ不可時は処理しない
		if (!JumpFlg) return;

		// 入力時以外は処理しない
		if (!Input.GetKeyDown(KeyCode.Space)) return;

		// ジャンプ移動量にジャンプ初速を設定
		GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, jumpFirstSpd, 0.0f), ForceMode.Acceleration);
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
