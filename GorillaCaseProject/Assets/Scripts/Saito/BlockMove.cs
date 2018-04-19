using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BlockSpeed))]
public class BlockMove : MonoBehaviour {

	[SerializeField]
	BlockSpeed.CBlockWeight mBlockWeight;

	[SerializeField]
	BlockSpeed.CEnviroment mEnviroment;

	BlockSpeed mBlockSpeed;

	List<Rigidbody> mAllBody;	//自分のリジッドボディ

	WaterStop mWaterStop;	//水で止まるようにするスクリプト

	// Use this for initialization
	void Start () {

		//コンポーネントの取得
		//
		mBlockSpeed = GetComponent<BlockSpeed>();

		mAllBody = new List<Rigidbody>();
		mAllBody.Add(GetComponent<Rigidbody>());
		foreach(var t in GetComponentsInChildren<Rigidbody>()) {
			mAllBody.Add(t);
		}

		mWaterStop = GetComponentInChildren<WaterStop>();
	}
	
	// Update is called once per frame
	void Update () {

		if(mWaterStop.IsInWater() == true) {
			mEnviroment = BlockSpeed.CEnviroment.cWater;
		}
		else {
			mEnviroment = BlockSpeed.CEnviroment.cAir;
		}

		mWaterStop.DisableCollision();
		if (mEnviroment == BlockSpeed.CEnviroment.cAir) {
			if(mBlockWeight == BlockSpeed.CBlockWeight.cLight) {
				mWaterStop.EnableCollision();
			}
		}

		//ブロックの移動
		Move();

		//デバッグ表示
		DrawDebug(mBlockWeight);
	}


	//ブロックに加速度を適用
	void Move() {
		
		//軽さに応じた加速度を取得
		Vector3 lAccel = mBlockSpeed.GetAccel(mBlockWeight, mEnviroment);

		//横方向に移動しない
		foreach(var lBody in mAllBody) {
			lBody.velocity = new Vector3(0.0f, lBody.velocity.y, lBody.velocity.z);
		}


		//リジッドボディに加速度を適用する
		foreach (var lBody in mAllBody) {
			lBody.AddForce(lAccel, ForceMode.Acceleration);
		}
	}
	

	//デバッグ表示
	void DrawDebug(BlockSpeed.CBlockWeight aWeight) {

		GameObject lDebugText = transform.Find("Debug/WeightText").gameObject;
		lDebugText.GetComponent<TextMesh>().text = BlockSpeed.GetWeight(aWeight).ToString() + "/2";
		
		lDebugText.transform.rotation = Quaternion.identity;
	}
}
