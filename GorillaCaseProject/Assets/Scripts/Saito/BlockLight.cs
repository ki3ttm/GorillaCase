using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLight : MonoBehaviour {

	//色を変更するモデル
	[SerializeField]
	GameObject mLightObject;


	//色を変更するマテリアル
	public Material mat;
	public Color emission;


	//重さを管理しているコンポーネントへの参照
	WeightManager mWeightManager;

	WeightManager.Weight mBeforeWeight;	//前のフレームの重さ


	// Use this for initialization
	void Start () {

		//マテリアルの取得
		var lRenderer = mLightObject.GetComponent<Renderer>();

		mat = lRenderer.materials[1];

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
		emission = GetColor(aWeight);

		//光らせる
		mat.SetColor("_EmissionColor",emission);

		mBeforeWeight = aWeight;

		//mMaterialAndEmission[0].mat.EnableKeyword("_EMISSION");
		//こういうのもあるらしいが、exeをビルドして実行するとエラーが出ることもあるらしいからやめておく。
		//スクリプトだけでEmissionを有効にしても、UnityがEmissionを有効にしたシェーダが要らないと判断するらしい。
	}


	//光るマテリアルを返す
	static Color GetColor(WeightManager.Weight aWeight) {
		switch (aWeight) {
			case WeightManager.Weight.flying:
				return Color.yellow;
			case WeightManager.Weight.light:
				return Color.green;
			case WeightManager.Weight.heavy:
				return Color.red;
		}
		return Color.black;
	}
}
