using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBackGround : MonoBehaviour {

	[SerializeField]
	GameObject mModelPrefab;

	[SerializeField, Tooltip("横方向の敷き詰め数")]
	int mWidth;

	[SerializeField, Tooltip("縦方向の敷き詰め数")]
	int mHeight;

	[SerializeField, Tooltip("横方向の敷き詰める間隔")]
	float mXInterval;

	[SerializeField, Tooltip("縦方向の敷き詰める間隔")]
	float mYInterval;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


#if UNITY_EDITOR

	//背景のサイズ変更
	void ResizeBackGround() {

		//現在のモデルの削除
		for (int i = transform.childCount - 1; i >= 0; i--) {
			DestroyGameObject(transform.GetChild(i).gameObject);
		}

		//モデルの配置
		for (int i = 0; i < mHeight; i++) {
			for (int j = 0; j < mWidth; j++) {
				float lXIndex = j - (mWidth - 1) / 2.0f;
				float lYIndex = i - (mHeight - 1) / 2.0f;
				GameObject lGameObject = InstantiatePrefab(mModelPrefab, gameObject);
				lGameObject.transform.localPosition = new Vector3(lXIndex * mXInterval, lYIndex * mYInterval, 0.0f);
			}
		}

	}

	[ContextMenu("Resize")]
	void Resize()
	{
		if (this == null) return;
		if (IsPrefab(gameObject)) return;
		if (UnityEditor.EditorApplication.isPlaying) return;
		ResizeBackGround();

	}

	private void OnValidate() {
		//UnityEditor.EditorApplication.delayCall += Resize;
	}

	public static bool IsPrefab(Object aObject) {

		var prefabType = UnityEditor.PrefabUtility.GetPrefabType(aObject);
		switch (prefabType) {
			case UnityEditor.PrefabType.Prefab:
			case UnityEditor.PrefabType.ModelPrefab:
				return true;
			default:
				return false;
		}
	}

	public static GameObject InstantiatePrefab(GameObject aPrefab, GameObject aParent) {
		GameObject go = GameObject.Instantiate(aPrefab, aParent.transform);
		GameObject g = UnityEditor.PrefabUtility.ConnectGameObjectToPrefab(go, aPrefab);
		UnityEditor.Undo.RegisterCreatedObjectUndo(g, "InstantiatePrefab");
		return g;
	}
	public static void DestroyGameObject(GameObject aGameObject) {
		UnityEditor.Undo.DestroyObjectImmediate(aGameObject);
	}

#endif
}
