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

	Rigidbody mOwnBody;	//自分のリジッドボディ
	Rigidbody[] mOtherBodys;	//周りの四つのリジッドボディ

	// Use this for initialization
	void Start () {

		//コンポーネントの取得
		//
		mBlockSpeed = GetComponent<BlockSpeed>();

		mOwnBody = GetComponent<Rigidbody>();

		mOtherBodys = GetComponentsInChildren<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

		if(transform.position.y <= 0.0f) {
			mEnviroment = BlockSpeed.CEnviroment.cWater;
		}
		else {
			mEnviroment = BlockSpeed.CEnviroment.cAir;
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

		//真ん中のリジッドボディには、そのままの加速度を適応
		mOwnBody.AddForce(lAccel, ForceMode.Acceleration);

		//浮いている状態なら、横方向に移動しない
		if (lAccel.y > 0.0f)
		{
			//mOwnBody.velocity = new Vector3(0.0f, mOwnBody.velocity.y, mOwnBody.velocity.z);
		}

		//ブロックの角の４つのリジッドボディに加速度を適用する
		foreach (var lOtherBody in mOtherBodys) {

			//ブロックが浮いているなら
			//if (mBlockWeight == BlockSpeed.CBlockWeight.cHover) {
			if (lAccel.y > 0.0f) {

				//ブロックが水平になるように、上のほうの角２つには上向きの加速度、下のほうの角２つには
				//下向きの加速度を与えてやる

				if (lOtherBody.position.y >= transform.position.y) {
					lOtherBody.AddForce(lAccel * 6, ForceMode.Acceleration);
				}
				else {
					lOtherBody.AddForce(lAccel * -4, ForceMode.Acceleration);
				}
			}
			//浮いていないなら、そのままの加速度を適用
			else {
				lOtherBody.AddForce(lAccel, ForceMode.Acceleration);
			}

		}
	}



	//デバッグ表示
	void DrawDebug(BlockSpeed.CBlockWeight aWeight) {

		GameObject lDebugText = transform.Find("Debug/WeightText").gameObject;
		lDebugText.GetComponent<TextMesh>().text = mBlockSpeed.GetWeight(aWeight).ToString() + "/2" ;
		
		lDebugText.transform.rotation = Quaternion.identity;
	}
}
