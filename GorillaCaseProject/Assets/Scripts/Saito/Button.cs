using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Button : MonoBehaviour {

	// Use this for initialization
	void Start () {

		//押されている割合の初期化
		mPushRate = 0.0f;
		mIsPush = false;


		//開始位置と終了位置の取得
		mLedgeMoveStart = transform.Find("Ledge").localPosition;
		mLedgeMoveDelta = transform.Find("LedgeEnd").localPosition - transform.Find("LedgeStart").localPosition;

		//ボックスキャスト用の箱の取得
		mWeightCheckCollider = transform.Find("Ledge/WeightCheck").GetComponent<BoxCollider>();
		mLayerMask = LayerMask.GetMask( new string[] { "Player", "Box" });

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
		Collider[] lHitColliders = Physics.OverlapBox(mWeightCheckCollider.transform.position, mWeightCheckCollider.transform.localScale / 2.0f, mWeightCheckCollider.transform.rotation, mLayerMask);

		List<GameObject> lPushBlockList = new List<GameObject>();	//ボタンに接しているブロックのリスト

		//ボタンに乗っている全てのブロックを求める
		foreach (var lCollider in lHitColliders) {

			GameObject lGameObject = lCollider.transform.gameObject;

			WeightBox tWeightBox = lGameObject.GetComponent<Redirect>().GetRedirect().GetComponent<WeightBox>();

			if (tWeightBox == null) continue;    //重さを持たないオブジェクトなら処理しない

			List<GameObject> tBlockList;
			//逆転しているなら、下に接しているブロックを取得
			if(mDirection == CDirection.cDown) {
				tBlockList = tWeightBox.GetPileBoxList(Vector3.down);
			}
			else {
				tBlockList = tWeightBox.GetPileBoxList(Vector3.up);
			}

			foreach(var tBlock in tBlockList) {
				lPushBlockList.Add(tBlock);	
			}
			
			//そのブロック自身も追加
			lPushBlockList.Add(tWeightBox.gameObject);
		}

		//ブロックの重複をなくす
		var lPushBlockDistinctList = lPushBlockList.Distinct();


		//<TODO>
		int lTotalWeight = 0;

		foreach(var tBlock in lPushBlockDistinctList) {
			
			//逆転しているなら、上向きの力を取得する
			if(mDirection == CDirection.cDown) {
				lTotalWeight += BlockSpeed.GetUpForce(tBlock.GetComponent<WeightManager>().WeightLv, tBlock.GetComponent<BlockMove>().mEnviroment);
			}
			else {
				lTotalWeight += BlockSpeed.GetDownForce(tBlock.GetComponent<WeightManager>().WeightLv, tBlock.GetComponent<BlockMove>().mEnviroment);
			}
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

		mPushRate = Mathf.Clamp01(mPushRate);   //mPushRateは0.0f~1.0f


		
		bool lNowButtonOn;
		//押されている割合が1.0以上なら
		if (mPushRate >= 1.0f) {
			lNowButtonOn = true;	//現在押されている
		}
		else {
			lNowButtonOn = false;
		}

		//前は押されていなくて、現在押されているなら
		if (!mIsButtonOn) {
			if(lNowButtonOn) {
				//ボタンが押された瞬間の処理
				Debug.Log("ButtonOn", this);
			}
		}

		//前は押されていて、現在押されていないなら
		if (mIsButtonOn) {
			if (!lNowButtonOn) {
				//ボタンが離された瞬間の処理
				Debug.Log("ButtonOff", this);
			}
		}

		mIsButtonOn = lNowButtonOn;	//現在の状態を上書き
	}


	//Ledgeを移動させる
	void MoveLedge() {
		//今は単なる線形補間
		transform.Find("Ledge").localPosition = Vector3.Lerp(mLedgeMoveStart, mLedgeMoveStart + mLedgeMoveDelta, mPushRate);
	}


	//デバッグ表示
	void DrawDebug(int aTotalWeight) {

		if(mDebugObject) {
			GameObject lDebugText = mDebugObject.transform.Find("TotalWeightText").gameObject;
			lDebugText.GetComponent<TextMesh>().text = aTotalWeight.ToString() + "/" + mOnWeight.ToString();

			if (IsButtonOn())
			{
				lDebugText.GetComponent<TextMesh>().color = Color.blue;
			}
			else
			{
				lDebugText.GetComponent<TextMesh>().color = Color.white;
			}
			lDebugText.transform.rotation = Quaternion.identity;
		}
	}


	//ボタンがONかどうか
	public bool IsButtonOn() {
		return mIsButtonOn;
	}

	private void OnValidate()
	{
		switch(mDirection) {
			case CDirection.cUp:
				transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
				break;
			case CDirection.cDown:
				transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
				break;
			case CDirection.cRight:
				transform.rotation = Quaternion.Euler(0.0f, 0.0f, 270.0f);
				break;
			case CDirection.cLeft:
				transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
				break;
		}
	}


	[SerializeField, Tooltip("押されるのにかかる時間")]
	float mPushTakeTime;

	[SerializeField, Tooltip("離されるのにかかる時間")]
	float mReleaseTakeTime;

	[SerializeField, Tooltip("押されるのに必要な重さ")]
	int mOnWeight;

	enum CDirection {
		cUp,
		cDown,
		cRight,
		cLeft
	}
	[SerializeField, Tooltip("スイッチの方向"), InstanceOverride]
	CDirection mDirection;


	float mPushRate;    //現在押されている割合
	bool mIsPush;    //このフレームで押されているか

	bool mIsButtonOn = false;	//ボタンが完全に押されているか

	Vector3 mLedgeMoveStart;	//でっぱりの最初の位置
	Vector3 mLedgeMoveDelta;  //でっぱりが完全に押されたときの移動量

	BoxCollider mWeightCheckCollider;   //押されているオブジェクトを見つけるときに使うコライダー
	int mLayerMask; //押されているオブジェクトを見つけるときのレイヤーマスク

	[SerializeField]
	GameObject mDebugObject;
}
