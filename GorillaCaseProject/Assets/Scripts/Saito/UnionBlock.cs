using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(WeightManager))]
public class UnionBlock : MonoBehaviour {

	[SerializeField]
	List<UnionBlock> mUnionList;

	public List<UnionBlock> GetUnionList() {
		return mUnionList;
	}

	public bool IsSameUnionGroup(UnionBlock aUnionBlock) {
		return mUnionList.Contains(aUnionBlock);
	}

	public List<UnionBlock> GetUnionAllList() {
		var l = new List<UnionBlock>();
		return GetUnionAllList(l);
	}

	//自分以外の共有ブロックを取得
	public List<UnionBlock> GetUnionAllListExceptOwn() {
		var l = new List<UnionBlock>();
		l = GetUnionAllList(l);
		l.Remove(this);
		return l;
	}

	List<UnionBlock> GetUnionAllList(List<UnionBlock> aList)
	{
		if (aList.Contains(this))
		{
			return aList;   //自身が含まれているなら、すでに通っているので戻る
		}
		else
		{
			aList.Add(this);
			foreach (var u in mUnionList)
			{
				if (u == null) continue;

				foreach (var s in u.GetUnionAllList(aList))
				{
					if (aList.Contains(s) == false)
					{
						aList.Add(s);
					}
				}
			}
			return aList.Distinct().ToList();   //一応重複なしのリストに
		}
	}

	

	// Use this for initialization
	void Start () {
		ChangeBoxColor();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//ボックスの色の変更
	void ChangeBoxColor() {

		foreach (var a in GetComponentsInChildren<Renderer>())
		{
			Material[] mat = a.materials;
			foreach (var m in mat)
			{
				if (m.name == "box tex (Instance)")
				{
					m.color = Color.cyan;
				}
			}
			a.materials = mat;
		}
	}

	[ContextMenu("ConnectUnion")]
	void ConnectUnion() {
		if (IsPrefab(gameObject)) return;

		var lList = GetUnionAllList();

		foreach (var u in lList) {
			u.mUnionList = lList;
		}
	}

	private void OnValidate()
	{
		//ConnectUnion();
	}

	public static bool IsPrefab(GameObject aGameObject)
	{
#if UNITY_EDITOR
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
#endif
		return false;
	}
}
