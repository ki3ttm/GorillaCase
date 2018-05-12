using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftShotLine : MonoBehaviour {

	[SerializeField]
	GameObject mModelParent;

	[SerializeField, DisallowSceneObject]
	GameObject mModelPrefab;

	[SerializeField, Tooltip("モデルを配置する間隔")]
	float mInterval;

	float mOffset = 0.0f;  //モデルを配置するオフセット

	[SerializeField, Tooltip("点線が動く速度")]
	float mOffsetSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		//オフセットを増やす→点線を移動させる
		mOffset += Time.deltaTime * mOffsetSpeed;
		if(mOffset >= mInterval) {
			mOffset = mOffset % mInterval;
		}

		//ゲームオブジェクトを移動する
		for(int i = mModelParent.transform.childCount - 1; i >= 0; i--) {
			Destroy(mModelParent.transform.GetChild(i).gameObject);
		}

		Vector3 lDir = mTo - mFrom;
		float lNowDist = 0.0f;
		while(true) {
			if (lNowDist + mOffset >= lDir.magnitude) break;
			var g = Instantiate(mModelPrefab, mModelParent.transform);
			g.transform.position = mFrom + lDir.normalized * (lNowDist + mOffset);
			lNowDist += mInterval;
		}

		ChangeColor();
	}

	public void SetColor(Color aColor) {
		mColor = aColor;
	}
	void ChangeColor() {
		for (int i = mModelParent.transform.childCount - 1; i >= 0; i--) {
			mModelParent.transform.GetChild(i).GetComponentInChildren<Renderer>().material.color = mColor;
		}
	}
	public void SetLinePosition(Vector3 aFrom, Vector3 aTo) {
		mFrom = aFrom;
		mTo = aTo;
	}

	Vector3 mFrom;
	Vector3 mTo;
	Color mColor;
}
