using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftShotLine : MonoBehaviour {

	[SerializeField]
	GameObject mModelParent;

	[SerializeField, DisallowSceneObject, EditOnPrefab]
	GameObject mModelPrefab;

	[SerializeField, DisallowSceneObject, EditOnPrefab]
	GameObject mDirectionModelPrefab;

	float mOffset = 0.0f;  //モデルを配置するオフセット

	[SerializeField, Tooltip("モデルを配置する間隔"), Space(4), Header("変更可能")]
	float mInterval;

	[SerializeField, Tooltip("点線が動く速度"), EditOnPrefab]
	float mOffsetSpeed;

	[SerializeField, Tooltip("何個おきに矢印を出すか")]
	int mDirectionInterval = 1;

	[SerializeField, Tooltip("矢印を出し始めたりする位置"), EditOnPrefab]
	float mStartOffset;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		//オフセットを増やす→点線を移動させる
		mOffset += Time.deltaTime * mOffsetSpeed;
		mOffset = mOffset % (mInterval * mDirectionInterval);

		ReplaceModel();

		ChangeColor();
	}

	public void SetColor(Color aColor) {
		mColor = aColor;
	}
	void ChangeColor() {
		for (int i = mModelParent.transform.childCount - 1; i >= 0; i--) {
			foreach (var r in mModelParent.transform.GetChild(i).GetComponentsInChildren<Renderer>()) {
				r.material.SetColor("_EmissionColor", mColor * 2.0f);
			}
		}
	}
	public void SetLinePosition(Vector3 aFrom, Vector3 aTo) {
		mFrom = aFrom;
		mTo = aTo;
		ReplaceModel();
	}
	public void ReplaceModel() {
		//ゲームオブジェクトを移動する
		for (int i = mModelParent.transform.childCount - 1; i >= 0; i--)
		{
			Destroy(mModelParent.transform.GetChild(i).gameObject);
		}

		Vector3 lDir = mTo - mFrom;
		
		float lOffset = mOffset % mInterval;
		for(int i = 0; ; i++) {
			float lNowDist = i * mInterval + lOffset + mStartOffset;
			if (lNowDist >= lDir.magnitude - mStartOffset) break;
			GameObject g;

			int lDirectionIndex = (int)(mOffset / mInterval);
			if (i % mDirectionInterval == lDirectionIndex) {
				g = Instantiate(mDirectionModelPrefab, mModelParent.transform);
			}
			else {
				g = Instantiate(mModelPrefab, mModelParent.transform);
			}
			g.transform.position = mFrom + lDir.normalized * lNowDist;
			g.transform.rotation = Quaternion.FromToRotation(Vector3.right, lDir);
		}
	}

	//lNowDist + lOffset →オブジェクトの現在位置
	//4なら、4


	Vector3 mFrom;
	Vector3 mTo;
	Color mColor;
}
