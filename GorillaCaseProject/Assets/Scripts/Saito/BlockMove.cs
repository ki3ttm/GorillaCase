using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BlockSpeed))]
public class BlockMove : MonoBehaviour {

	[SerializeField]
	BlockSpeed.CEnviroment mEnviroment;

	BlockSpeed mBlockSpeed;

	List<Rigidbody> mAllBody;	//自分のリジッドボディ

	WaterStop mWaterStop;   //水で止まるようにするスクリプト

	WeightManager mWeightManager;   //重さを管理するスクリプト
	WeightBox mWeightObject;   //重さの対象となるスクリプト

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

		mWeightManager = GetComponent<WeightManager>();
		mWeightObject = GetComponent<WeightBox>();
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
			if(mWeightManager.WeightLv == WeightManager.Weight.light) {
				mWaterStop.EnableCollision();
			}
		}

		//ブロックの移動
		Move();

		//デバッグ表示
		DrawDebug();
	}


	//ブロックに加速度を適用
	void Move() {

		//実質の軽さを取得
		WeightManager.Weight lSubstanceWeight = GetSubstanceWeight();

		//軽さに応じた加速度を取得
		Vector3 lAccel = mBlockSpeed.GetAccel(lSubstanceWeight, mEnviroment);

		//横方向に移動しない
		foreach(var lBody in mAllBody) {
			lBody.velocity = new Vector3(0.0f, lBody.velocity.y, lBody.velocity.z);
		}


		//リジッドボディに加速度を適用する
		foreach (var lBody in mAllBody) {
			lBody.AddForce(lAccel, ForceMode.Acceleration);
		}
	}
	
	//実質の重さを取得
	WeightManager.Weight GetSubstanceWeight() {

		/*
		//２の重さなら、実質の重さも２になる
		if(mWeightManager.WeightLv == WeightManager.Weight.heavy){
			return WeightManager.Weight.heavy;
		}

		List<GameObject> lOnBlockList = GetOnBlockList(); //乗っているブロックのリスト
		List<GameObject> lUnderBlockList = GetUnderBlockList(); //下にあるブロックのリスト

		switch(mWeightManager.WeightLv) {

			case WeightManager.Weight.flying:
				//上に２が乗っていれば２の重さに
				foreach (var t in lOnBlockList) {
					if (t.GetComponent<WeightManager>().WeightLv == WeightManager.Weight.heavy) {
						return WeightManager.Weight.heavy;
					}
				}

				//下に１があって、それが水中なら０の重さに
				foreach (var t in lUnderBlockList) {
					if (t.GetComponent<WeightManager>().WeightLv == WeightManager.Weight.light) {
						if(t.GetComponent<BlockMove>().mEnviroment == BlockSpeed.CEnviroment.cWater) {
							return WeightManager.Weight.light;
						}
					}
				}

				//上に１があれば１の重さに
				foreach (var t in lOnBlockList) {
					if (t.GetComponent<WeightManager>().WeightLv == WeightManager.Weight.light) {
						return WeightManager.Weight.light;
					}
				}

				//それ以外なら０
				return WeightManager.Weight.flying;


			case WeightManager.Weight.heavy: {

					switch (mEnviroment) {
						case BlockSpeed.CEnviroment.cAir:
							//上に２が乗っていれば２の重さに
							foreach (var t in lOnBlockList) {
								if (t.GetComponent<WeightManager>().WeightLv == WeightManager.Weight.heavy) {
									return WeightManager.Weight.heavy;
								}
							}

							//下に１があって、それが水中なら０の重さに
							foreach (var t in lUnderBlockList) {
								if (t.GetComponent<WeightManager>().WeightLv == WeightManager.Weight.light) {
									if (t.GetComponent<BlockMove>().mEnviroment == BlockSpeed.CEnviroment.cWater) {
										return WeightManager.Weight.light;
									}
								}
							}

							//それ以外なら１
							return WeightManager.Weight.light;

						case BlockSpeed.CEnviroment.cWater:
							//上に２が乗っていれば２の重さに
							foreach (var t in lOnBlockList) {
								if (t.GetComponent<WeightManager>().WeightLv == WeightManager.Weight.heavy) {
									return WeightManager.Weight.heavy;
								}
							}

							//それ以外なら１
							return WeightManager.Weight.light;
					}
					break;
				}
		}

		//*/
		return WeightManager.Weight.light;
	}


	//デバッグ表示
	void DrawDebug() {

		GameObject lDebugText = transform.Find("Debug/WeightText").gameObject;
		lDebugText.GetComponent<TextMesh>().text = BlockSpeed.GetWeight(mWeightManager.WeightLv).ToString() + "/2";
		
		lDebugText.transform.rotation = Quaternion.identity;
	}
}
