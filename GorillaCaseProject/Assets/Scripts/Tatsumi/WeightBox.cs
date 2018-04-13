using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightBox : MonoBehaviour {
/*	// 四辺に存在する当たり判定
	[SerializeField] BoxCollider[] fourSideCol = new BoxCollider[4];

	// Use this for initialization
	//	void Start () {}

	// Update is called once per frame
	//	void Update () {}

	public List<GameObject> GetTopBoxList() {
		// 上方向に積まれているボックスのリストを返す
		List<GameObject> ret = new List<GameObject>();
		AddChainBoxList(ret, new Vector3(0.0f, 1.0f, 0.0f));
		return ret;
	}
	public List<GameObject> GetBottomBoxList() {
		// 下方向に積まれているボックスのリストを返す
		List<GameObject> ret = new List<GameObject>();
		AddChainBoxList(ret, new Vector3(0.0f, -1.0f, 0.0f));
		return ret;
	}
	public List<GameObject> GetLeftBoxList() {
		// 左方向に積まれているボックスのリストを返す
		List<GameObject> ret = new List<GameObject>();
		AddChainBoxList(ret, new Vector3(-1.0f, 0.0f, 0.0f));
		return ret;
	}
	public List<GameObject> GetRightBoxList() {
		// 右方向に積まれているボックスのリストを返す
		List<GameObject> ret = new List<GameObject>();
		AddChainBoxList(ret, new Vector3(1.0f, 0.0f, 0.0f));
		return ret;
	}

	void AddChainBoxList(List<GameObject> _boxList, Vector3 _vec) {
		// 四辺コライダーを特定方向に近い順に並び替え
		SortFourSideCollider(_vec);
	
		// 指定方向から一番目と二番目に近い四辺コライダーに接触している対象オブジェクトのコライダーをリスト化	
		List<Collider> hitColList = new List<Collider>();
		hitColList.AddRange(Physics.OverlapBox(fourSideCol[0].center, fourSideCol[0].size));
		hitColList.AddRange(Physics.OverlapBox(fourSideCol[1].center, fourSideCol[1].size));

		// 重複を排除
		RemoveDuplicate(_boxList);

		// 指定方向から遠い二つのコライダーの片方にでも接触している対象オブジェクトをリストから排除
		for (int targetIdx = 0; targetIdx < targetList.Count; targetIdx++) {
			//if( || ){
			//
			//}
		}

		// リスト内の対象オブジェクトを既存リストと重複無しに統合
		for (int targetIdx = 0; targetIdx < targetList.Count; targetIdx++) {
			for (int idx = 0; idx < _list.Count; idx++) {
				// 重複していれば追加しない
				if (_list[idx] == targetList[targetIdx]) {
					// 対象オブジェクトリストループのインデックスを進める
					break;
				}
				// 重複が無ければ既存リストに追加
				else if (idx == _list.Count - 1) {
					_list.AddRange(targetList.ConvertAll<GameObject>());
				}
			}
		}

		// リスト内の対象オブジェクトそれぞれで再帰呼び出し
	}

	// 四辺のコライダーを指定の方向に近い順に並び替え
	void SortFourSideCollider(Vector3 _vec) {
		_vec = _vec.normalized;
		// 指定方向に最も近い四辺コライダーを
		for (int fourColIdx = 0; fourColIdx < 4; fourColIdx++) {
			
		}

	}


	List<BoxCollider> GetSortColliderList() {
		// 指定方向に近い順番で四辺のコライダーのリストを作成
		List<BoxCollider> sortColList = new List<BoxCollider>();
		for (int colIdx = 0; colIdx < colList.Count; colIdx++) {
			// ソート用の挿入位置を求める
			int sortIdx = 0;
			for (; sortIdx < sortColList.Count; sortIdx++) {
				// 既存の要素よりも指定方向から遠くなった位置を挿入位置とする
				bool breakFlg = false;
				switch (_vec) {
				case ChainVec.top:
					breakFlg = (colList[colIdx].transform.position.y <= sortColList[sortIdx].transform.position.y);
					break;
				case ChainVec.bottom:
					breakFlg = (colList[colIdx].transform.position.y >= sortColList[sortIdx].transform.position.y);
					break;
				case ChainVec.right:
					breakFlg = (colList[colIdx].transform.position.x <= sortColList[sortIdx].transform.position.x);
					break;
				case ChainVec.left:
					breakFlg = (colList[colIdx].transform.position.x >= sortColList[sortIdx].transform.position.x);
					break;
				default:
					break;
				}
				if (breakFlg) {
					break;
				}
			}
			// 求めた位置に挿入
			sortColList.Insert(sortIdx, colList[colIdx]);
		}
	}

	// GameObjectのリストから重複している要素を排除し、排除した数を返す
	int RemoveDuplicate(List<BoxCollider> _list) {
		int cnt = 0;
		// 対象リストから重複を排除
		for (int targetIdx = 0; targetIdx < _list.Count; targetIdx++) {
			// 以降に同様の要素が存在すれば
			while (_list.LastIndexOf(_list[targetIdx]) > targetIdx) {
				// 重複している要素を排除
				targetList.RemoveAt(targetList.LastIndexOf(targetList[targetIdx]);
				cnt++;
			}
		}
		return cnt;
	}
*/
}
