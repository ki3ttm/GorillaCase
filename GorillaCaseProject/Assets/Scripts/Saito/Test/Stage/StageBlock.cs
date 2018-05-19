using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBlock : MonoBehaviour {

	[SerializeField]
	GameObject mModel;

	[SerializeField]
	GameObject mCollider;

	[SerializeField]
	GameObject mModelUpPrefab;

	[SerializeField]
	GameObject mModelDownPrefab;

	[SerializeField]
	GameObject mModelLeftPrefab;

	[SerializeField]
	GameObject mModelRightPrefab;

	[SerializeField]
	GameObject mModelUpLeftPrefab;

	[SerializeField]
	GameObject mModelUpRightPrefab;

	[SerializeField]
	GameObject mModelDownLeftPrefab;

	[SerializeField]
	GameObject mModelDownRightPrefab;

	[SerializeField]
	GameObject mModelNonePrefab;

	[SerializeField]
	bool mIsUp;

	[SerializeField]
	bool mIsDown;

	[SerializeField]
	bool mIsLeft;

	[SerializeField]
	bool mIsRight;
	

	[SerializeField, Tooltip("幅")]
	int mWidth;

	[SerializeField, Tooltip("高さ")]
	int mHeight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


#if UNITY_EDITOR

	//床のサイズ変更
	void ResizeFloor()
	{

		//現在のモデルの削除
		for (int i = mModel.transform.childCount - 1; i >= 0; i--) {
			DestroyGameObject(mModel.transform.GetChild(i).gameObject);
		}

		//モデルの配置
		for(int i = 0; i < mHeight; i++) {

			for (int j = 0; j < mWidth; j++) {

				bool lUp = false;
				bool lDown = false;
				bool lLeft = false;
				bool lRight = false; ;
				if (mIsUp && i == mHeight - 1) {
					lUp = true;
				}
				if (mIsDown && i == 0) {
					lDown = true;
				}
				if (mIsLeft && j == 0) {
					lLeft = true;
				}
				if (mIsRight && j == mWidth - 1) {
					lRight = true;
				}

				GameObject lPrefab = null;
				if(lUp && lLeft) {
					lPrefab = mModelUpLeftPrefab;
				}
				else if (lUp && lRight) {
					lPrefab = mModelUpRightPrefab;
				}
				else if (lDown && lLeft) {
					lPrefab = mModelDownLeftPrefab;
				}
				else if (lDown && lRight) {
					lPrefab = mModelDownRightPrefab;
				}
				else if (lUp) {
					lPrefab = mModelUpPrefab;
				}
				else if (lDown) {
					lPrefab = mModelDownPrefab;
				}
				else if (lLeft) {
					lPrefab = mModelLeftPrefab;
				}
				else if (lRight) {
					lPrefab = mModelRightPrefab;
				}
				else {
					lPrefab = mModelNonePrefab;
				}

				GameObject lGameObject = InstantiatePrefab(lPrefab, mModel);
				lGameObject.transform.localPosition = new Vector3(j, i, 0.0f);
			}
		}

		//コライダーの大きさ変更
		mCollider.transform.localScale = new Vector3(mWidth, mHeight, 1.0f);
	}
	
	[ContextMenu("Resize")]
	void Resize()
	{
		if (this == null) return;
		if (IsPrefab(gameObject)) return;
		if (UnityEditor.EditorApplication.isPlaying) return;
		ResizeFloor();

	}

	private void OnValidate()
	{
		//UnityEditor.EditorApplication.delayCall += Resize;
	}

	public static bool IsPrefab(Object aObject)
	{

		var prefabType = UnityEditor.PrefabUtility.GetPrefabType(aObject);
		switch (prefabType)
		{
			case UnityEditor.PrefabType.Prefab:
			case UnityEditor.PrefabType.ModelPrefab:
				return true;
			default:
				return false;
		}
	}

	public static GameObject InstantiatePrefab(GameObject aPrefab, GameObject aParent)
	{
		GameObject go = GameObject.Instantiate(aPrefab, aParent.transform);
		GameObject g = UnityEditor.PrefabUtility.ConnectGameObjectToPrefab(go, aPrefab);
		UnityEditor.Undo.RegisterCreatedObjectUndo(g, "InstantiatePrefab");
		return g;
	}
	public static void DestroyGameObject(GameObject aGameObject)
	{
		UnityEditor.Undo.DestroyObjectImmediate(aGameObject);
	}
	
#endif
}
