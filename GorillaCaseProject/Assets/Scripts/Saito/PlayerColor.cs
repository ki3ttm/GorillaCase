using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour {

	//色を変更する必要があるモデルの中で、一番親に当たるゲームオブジェクト
	[SerializeField]
	GameObject mModelObject;


	//重さに応じたマテリアル
	[SerializeField]
	Material[] mMaterial;


	//このマテリアルがセットされている部分は、変更される
	[SerializeField]
	Material mChangeTargetMaterial;

	//変更する必要があるマテリアルのリスト
	class MaterialInRenderer {
		public Renderer mRenderer;	//そのマテリアルを持っているレンダラー
		public int mIndex;	//レンダラーの中のマテリアル配列の中のインデックス
	}
	List<MaterialInRenderer> mChangeMaterials = new List<MaterialInRenderer>();


	//重さを管理しているコンポーネントへの参照
	WeightManager mWeightManager;

	WeightManager.Weight mBeforeWeight; //前のフレームの重さ


	// Use this for initialization
	void Start()
	{
		//変更する必要のあるマテリアルの取得
		//　子オブジェクトの全てのレンダラーを取得して調べる

		var lRenderers = mModelObject.GetComponentsInChildren<Renderer>();

		//レンダラー
		foreach(var tRenderer in lRenderers) {

			//マテリアル
			for(int i = 0; i < tRenderer.materials.Length; i++) {

				if (tRenderer.materials[i].name == mChangeTargetMaterial.name + " (Instance)") {
					var t = new MaterialInRenderer();
					t.mRenderer = tRenderer;
					t.mIndex = i;
					mChangeMaterials.Add(t);
				}

			}
		}

		//コンポーネントの取得
		mWeightManager = GetComponent<WeightManager>();


		//重さに応じて色を変更
		ChangeColor(mWeightManager.SeemWeightLv);
	}

	// Update is called once per frame
	void Update()
	{

		//前のフレームから重さが変わっていたら
		if (mBeforeWeight != mWeightManager.SeemWeightLv)
		{
			//重さに応じて色を変更
			ChangeColor(mWeightManager.SeemWeightLv);
		}
	}


	//重さに応じて色を変更
	void ChangeColor(WeightManager.Weight aWeight) {

		//変更された後のマテリアル
		int lMaterialIndex = GetMaterialIndex(aWeight);

		//マテリアルを変更
		foreach(var t in mChangeMaterials) {
			var tMats = t.mRenderer.materials;
			tMats[t.mIndex] = mMaterial[lMaterialIndex];
			t.mRenderer.materials = tMats;
		}
		
		mBeforeWeight = aWeight;
	}


	//重さに応じた、マテリアルのインデックスを返す
	static int GetMaterialIndex(WeightManager.Weight aWeight)
	{
		switch (aWeight) {
			case WeightManager.Weight.flying:
				return 0;
			case WeightManager.Weight.light:
				return 1;
			case WeightManager.Weight.heavy:
				return 2;
		}
		Debug.LogError("重さが異常です");
		return 0;
	}
}
