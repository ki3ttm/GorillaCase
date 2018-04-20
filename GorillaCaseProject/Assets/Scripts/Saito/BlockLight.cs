using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLight : MonoBehaviour {

	//色を変更するモデル
	[SerializeField]
	GameObject mLightObject;


	//色を変更するマテリアル
	class MaterialAndEmission {
		public Material mat;
		public Color emission;
	}
	MaterialAndEmission[] mMaterialAndEmission = new MaterialAndEmission[3];


	//重さを管理しているコンポーネントへの参照
	WeightManager mWeightManager;

	WeightManager.Weight mBeforeWeight;	//前のフレームの重さ


	// Use this for initialization
	void Start () {

		//マテリアルの取得
		var lRenderer = mLightObject.GetComponent<Renderer>();

		//Rendererで、マテリアルが並んでいる順番（Rendererの2番目のマテリアルが、一番軽い時に光る）
		int[] lMatIndex = new int[3] { 2, 1, 0 };

		for(int i = 0; i < 3; i++) {
			int tMatIdx = lMatIndex[i];
			mMaterialAndEmission[i] = new MaterialAndEmission();
			mMaterialAndEmission[i].mat = lRenderer.materials[tMatIdx];
			mMaterialAndEmission[i].emission = mMaterialAndEmission[i].mat.GetColor("_EmissionColor");
		}

		//コンポーネントの取得
		mWeightManager = GetComponent<WeightManager>();


		//重さに応じて光る場所を変更
		ChangeLight(mWeightManager.WeightLv);
	}
	
	// Update is called once per frame
	void Update () {

		//前のフレームから重さが変わっていたら
		if(mBeforeWeight != mWeightManager.WeightLv) {
			//重さに応じて光る場所を変更
			ChangeLight(mWeightManager.WeightLv);
		}

	}


	//重さに応じて光る場所を変更
	void ChangeLight(WeightManager.Weight aWeight) {

		//何番目のマテリアルまで光るか
		int lLightTopIndex = GetLightTopIndex(aWeight);

		//光らせる
		for (int i = 0; i <= lLightTopIndex; i++) {
			mMaterialAndEmission[i].mat.SetColor("_EmissionColor", mMaterialAndEmission[i].emission);
		}

		//光らせない
		for (int i = lLightTopIndex + 1; i < 3; i++) {
			mMaterialAndEmission[i].mat.SetColor("_EmissionColor", Color.black);
		}

		mBeforeWeight = aWeight;

		//mMaterialAndEmission[0].mat.EnableKeyword("_EMISSION");
		//こういうのもあるらしいが、exeをビルドして実行するとエラーが出ることもあるらしいからやめておく。
		//スクリプトだけでEmissionを有効にしても、UnityがEmissionを有効にしたシェーダが要らないと判断するらしい。
	}


	//その重さで光るマテリアルの、一番大きいインデックス番号を返す
	static int GetLightTopIndex(WeightManager.Weight aWeight) {
		switch (aWeight) {
			case WeightManager.Weight.flying:
				return 0;
			case WeightManager.Weight.light:
				return 1;
			case WeightManager.Weight.heavy:
				return 2;
		}
		return -1;
	}
}
