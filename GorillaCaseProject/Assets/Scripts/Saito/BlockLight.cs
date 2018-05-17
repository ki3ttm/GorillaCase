using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLight : MonoBehaviour {

	//色を変更するモデル
	[SerializeField]
	GameObject mHoverModel;

	[SerializeField]
	GameObject mLightModel;

	[SerializeField]
	GameObject mHeavyModel;

	[SerializeField]
	GameObject mLightObject;

	[SerializeField]
	bool mIsBlock = true;


	//色を変更するマテリアル
	Material mat;
	Color emission;


	//重さを管理しているコンポーネントへの参照
	WeightManager mWeightManager;

	WeightManager.Weight mBeforeWeight;	//前のフレームの重さ


	// Use this for initialization
	void Start () {

		//マテリアルの取得
		var lRenderer = mLightObject.GetComponent<Renderer>();

		mat = null;
		if(mIsBlock) {
			mat = lRenderer.materials[1];
		}
		

		//コンポーネントの取得
		mWeightManager = GetComponent<WeightManager>();

		//重さに応じて光る場所を変更
		ChangeLight(mWeightManager.SeemWeightLv);
	}
	
	// Update is called once per frame
	void Update () {

		//前のフレームから重さが変わっていたら
		if(mBeforeWeight != mWeightManager.SeemWeightLv) {
			//重さに応じて光る場所を変更
			ChangeLight(mWeightManager.SeemWeightLv);
		}

	}


	//重さに応じて光る場所を変更
	void ChangeLight(WeightManager.Weight aWeight) {

		//何番目のマテリアルまで光るか
		emission = GetColor(aWeight);

		//光らせる
		if(mat != null) {
			mat.SetColor("_EmissionColor", emission);
		}
		

		ShowEffect(aWeight);

		mBeforeWeight = aWeight;
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

	void ShowEffect(WeightManager.Weight aWeight)
	{
		if(aWeight == WeightManager.Weight.flying) {
			mHoverModel.SetActive(true);
			mLightModel.SetActive(false);
			mHeavyModel.SetActive(false);
		}

		if (aWeight == WeightManager.Weight.light)
		{
			mHoverModel.SetActive(false);
			mLightModel.SetActive(true);
			mHeavyModel.SetActive(false);
		}

		if (aWeight == WeightManager.Weight.heavy)
		{
			mHoverModel.SetActive(false);
			mLightModel.SetActive(false);
			mHeavyModel.SetActive(true);
		}
	}
}
