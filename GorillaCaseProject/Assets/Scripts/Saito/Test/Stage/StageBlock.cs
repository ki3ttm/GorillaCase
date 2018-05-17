using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBlock : MonoBehaviour {

	[SerializeField]
	GameObject mModel;

	[SerializeField]
	GameObject mCollider;

	[SerializeField]
	GameObject mModelPrefab;

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

				GameObject lGameObject = InstantiatePrefab(mModelPrefab, mModel);
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
