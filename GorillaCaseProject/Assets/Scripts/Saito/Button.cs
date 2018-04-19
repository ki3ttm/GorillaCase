using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

	// Use this for initialization
	void Start () {

		//押されている割合の初期化
		mPushRate = 0.0f;
		mIsPush = false;


		//開始位置と終了位置の取得
		mLedgeMoveStart = transform.Find("Ledge").localPosition;
		mLedgeMoveEnd = transform.Find("LedgeEnd").localPosition;


		//ボックスキャスト用の箱の取得
		mWeightCheckCollider = transform.Find("Ledge/WeightCheck").GetComponent<BoxCollider>();
	}


	
	// Update is called once per frame
	void Update () {

		//押されている割合の更新
		UpdatePushRate();

		//Ledgeの移動
		MoveLedge();
	}

	private void FixedUpdate() {

		//押されていない状態にしておく
		mIsPush = false;

		//箱にかかっている重さの合計が、起動に必要な重さ以上なら、押される
		int lTotalWeight = GetTotalWeight();
		if (lTotalWeight >= mOnWeight) {
			mIsPush = true;
		}

		//デバッグ情報の表示
		DrawDebug(lTotalWeight);
	}


	//現在ボタンにかかっている重さの合計を求める
	int GetTotalWeight() {

		//判定用のコライダーと当たっているコライダーを取得
		Collider[] lHitColliders = Physics.OverlapBox(mWeightCheckCollider.transform.position, mWeightCheckCollider.transform.localScale / 2.0f, mWeightCheckCollider.transform.rotation);

		int lTotalWeight = 0;

		//全てのコライダーの重さを求める
		foreach (var lCollider in lHitColliders) {
			if (lCollider == mWeightCheckCollider) continue;    //自分自身なら処理しない

			GameObject lGameObject = lCollider.transform.gameObject;

			//<TODO>
			//if (lGameObject.GetComponent<WeightComponent> == null) continue;    //重さを持たないオブジェクトなら処理しない

			//重さを足す
			//<TODO>
			//lTotalWeight += lGameObject.GetComponent<WeightComponent>.GetTotalWeight();
			lTotalWeight += 1;
		}

		return lTotalWeight;
	}


	//押されている割合の更新
	void UpdatePushRate() {

		//押されているなら
		if(mIsPush == true) {
			mPushRate += Time.deltaTime * (1.0f / mPushTakeTime);
		}
		//押されていないなら
		else {
			mPushRate -= Time.deltaTime * (1.0f / mReleaseTakeTime);
		}

		mPushRate = Mathf.Clamp01(mPushRate);	//mPushRateは0.0f~1.0f
	}


	//Ledgeを移動させる
	void MoveLedge() {
		//今は単なる線形補間
		transform.Find("Ledge").localPosition = Vector3.Lerp(mLedgeMoveStart, mLedgeMoveEnd, mPushRate);
	}


	//デバッグ表示
	void DrawDebug(int aTotalWeight) {

		GameObject lDebugText = transform.Find("Debug/TotalWeightText").gameObject;
		lDebugText.GetComponent<TextMesh>().text = aTotalWeight.ToString() + "/" + mOnWeight.ToString();

		if (IsButtonOn()) {
			lDebugText.GetComponent<TextMesh>().color = Color.blue;
		}
		else {
			lDebugText.GetComponent<TextMesh>().color = Color.white;
		}
		lDebugText.transform.rotation = Quaternion.identity;
	}


	//ボタンがONかどうか
	public bool IsButtonOn() {
		if (mPushRate >= 1.0f) return true;	//押されている割合が1.0以上なら、ON
		return false;
	}



	[SerializeField]
	float mPushTakeTime;    //押されるのにかかる時間

	[SerializeField]
	float mReleaseTakeTime;    //離されるのにかかる時間

	[SerializeField]
	int mOnWeight;    //押されるのに必要な重さ

	[SerializeField]
	bool mIsReverse;    //逆向きのスイッチかどうか


	float mPushRate;    //現在押されている割合
	bool mIsPush;    //このフレームで押されているか

	Vector3 mLedgeMoveStart;	//でっぱりの最初の位置
	Vector3 mLedgeMoveEnd;  //でっぱりが完全に押されたときの位置

	BoxCollider mWeightCheckCollider;	//押されているオブジェクトを見つけるときに使うコライダー
}
