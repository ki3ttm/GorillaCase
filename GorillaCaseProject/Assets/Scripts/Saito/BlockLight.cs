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

	[SerializeField]
	Color mFlyingColor;

	[SerializeField]
	float mFlyingColorPower = 1.0f;

	[SerializeField]
	Color mLightColor;

	[SerializeField]
	float mLightColorPower = 1.0f;

	[SerializeField]
	Color mHeavyColor;

	[SerializeField]
	float mHeavyColorPower = 1.0f;

	//色を変更するマテリアル
	[SerializeField]
	Material mLightMaterial;


	//重さを管理しているコンポーネントへの参照
	WeightManager mWeightManager;

	WeightManager.Weight mBeforeWeight;	//前のフレームの重さ


	// Use this for initialization
	void Start () {

		//コンポーネントの取得
		mWeightManager = GetComponent<WeightManager>();

		//重さに応じて光る場所を変更
		ChangeLight(mWeightManager.SeemWeightLv);

	}
	
	// Update is called once per frame
	void Update () {
		
		//前のフレームから重さが変わっていたら
		if (mBeforeWeight != mWeightManager.SeemWeightLv) {
			//重さに応じて光る場所を変更
			ChangeLight(mWeightManager.SeemWeightLv);
		}
	}


	//重さに応じて光る場所を変更
	void ChangeLight(WeightManager.Weight aWeight) {
		
		//光らせる
		if (mIsBlock) {
			Utility.ChangeMaterialColor(mLightObject, mLightMaterial, "_EmissionColor", GetColor(aWeight));
		}
		
		ShowEffect(aWeight);

		mBeforeWeight = aWeight;
	}


	//光る色を返す
	Color GetColor(WeightManager.Weight aWeight) {
		switch (aWeight) {
			case WeightManager.Weight.flying:
				return mFlyingColor * mFlyingColorPower;
			case WeightManager.Weight.light:
				return mLightColor * mLightColorPower;
			case WeightManager.Weight.heavy:
				return mHeavyColor * mHeavyColorPower;
		}
		return Color.black;
	}

	void ShowEffect(WeightManager.Weight aWeight)
	{
		if(aWeight == WeightManager.Weight.flying) {
			Play(mHoverModel);
			Stop(mLightModel);
			Stop(mHeavyModel);
		}

		if (aWeight == WeightManager.Weight.light)
		{
			Play(mLightModel);
			Stop(mHoverModel);
			Stop(mHeavyModel);
		}

		if (aWeight == WeightManager.Weight.heavy)
		{
			Play(mHeavyModel);
			Stop(mLightModel);
			Stop(mHoverModel);
		}
	}

	void Play(GameObject aWeightModel)
	{
		foreach (var p in aWeightModel.GetComponentsInChildren<ParticleSystem>()) {
			p.Play();
		}
		foreach (var p in aWeightModel.GetComponentsInChildren<MeshRenderer>()) {
			p.enabled = true;
		}
	}
	void Stop(GameObject aWeightModel)
	{
		foreach (var p in aWeightModel.GetComponentsInChildren<ParticleSystem>()) {
			p.Stop();
		}
		foreach (var p in aWeightModel.GetComponentsInChildren<MeshRenderer>()) {
			p.enabled = false;
		}
	}
}
