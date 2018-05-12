using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RailFloor : MonoBehaviour {

	[DisallowSceneObject, SerializeField]
	GameObject mLeftModel;

	[DisallowSceneObject, SerializeField]
	GameObject mRightModel;

	[DisallowSceneObject, SerializeField]
	GameObject mMidModel;

	[SerializeField]
	int mWidth;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


#if UNITY_EDITOR
	[ContextMenu("Resize")]
	void Resize() {

		if (this == null) return;

		if (IsPrefab(gameObject)) return;

		if (!(mWidth >= 2))
		{
			Debug.LogError("Width is small", this);
			return;
		}
		
		var c = transform.Find("Collider");
		c.localScale = new Vector3(mWidth, 1.0f, 1.0f);
		c.localPosition = new Vector3(mWidth / 2.0f, 0.5f, 0.0f);

		var m = transform.Find("Model");

		List<GameObject> lDeleteList = new List<GameObject>();

		foreach(Transform t in m.transform) {
			lDeleteList.Add(t.gameObject);
		}
		foreach (var g in lDeleteList)
		{
			DestroyImmediate(g, true);
		}

		if (mLeftModel)
		{
			var l = Instantiate(mLeftModel, m);
			l.transform.localPosition = GetModelPosition(0);
		}

		if (mMidModel)
		{
			for (int i = 1; i < mWidth - 1; i++)
			{
				var mi = Instantiate(mMidModel, m);
				mi.transform.localPosition = GetModelPosition(i);
			}
		}

		if (mRightModel)
		{
			var r = Instantiate(mRightModel, m);
			r.transform.localPosition = GetModelPosition(mWidth - 1);
		}
	}

	private void OnValidate()
	{
		EditorApplication.delayCall += () => Resize();
	}

	static Vector3 GetModelPosition(int aIndex)
	{
		return new Vector3(aIndex + 0.5f, 0.5f, 0.0f);
	}

	public static bool IsPrefab(GameObject aGameObject) {

		var lPrefabType = PrefabUtility.GetPrefabType(aGameObject);
		switch (lPrefabType)
		{
			case PrefabType.Prefab:
			case PrefabType.ModelPrefab:
				return true;
			default:
				// Prefab以外ならtrue
				return false;
		}
	}
#endif
}
