using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Floor : MonoBehaviour {

	[SerializeField, Tooltip("床の横の大きさ")]
	float mWidth = 1.0f;

	[SerializeField, Tooltip("床の縦の大きさ")]
	float mHeight = 1.0f;

	[SerializeField, Tooltip("床のプレハブ"), DisallowSceneObject]
	GameObject mFloorPrefab;
	
	[SerializeField]
	GameObject mCollider;

	[SerializeField]
	GameObject mModel;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


#if UNITY_EDITOR

	[ContextMenu("Resize")]
	public void Resize()
	{
		if (this == null) return;

		var lPrefabType = PrefabUtility.GetPrefabType(gameObject);
		switch (lPrefabType)
		{
			case PrefabType.Prefab:
			case PrefabType.ModelPrefab:
				return;
			default:
				break;
		}

		DestroyModel();
		InstatiateModel();
		ResizeCollider();
	}


	private void OnValidate()
	{
		EditorApplication.delayCall += () => Resize();
	}

	void DestroyModel()
	{
		for (int i = mModel.transform.childCount - 1; i >= 0; i--) {
			Undo.DestroyObjectImmediate(mModel.transform.GetChild(i).gameObject);
		}
	}

	void InstatiateModel() {
		
		for (int i = 0; i < (int)mHeight; i++) {
			for (int j = 0; j < (int)mWidth; j++) {
				GameObject t = Instantiate(mFloorPrefab, mModel.transform);
				GameObject a = PrefabUtility.ConnectGameObjectToPrefab(t, mFloorPrefab);
				a.transform.localPosition = GetModelPosition(j, i);
				Undo.RegisterCreatedObjectUndo(a, "Create Object");
			}
		}
	}

	static Vector3 GetModelPosition(int aX, int aY)
	{
		return new Vector3(0.5f + aX, 0.5f + aY, 0.0f);
	}

	void ResizeCollider()
	{
		mCollider.transform.localScale = new Vector3(mWidth, mHeight, mCollider.transform.localScale.z);
		mCollider.transform.localPosition = new Vector3(mWidth / 2, mHeight / 2, 0.0f);
	}
#endif
}
