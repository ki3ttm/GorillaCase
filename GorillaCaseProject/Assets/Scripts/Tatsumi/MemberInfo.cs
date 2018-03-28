using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberInfo : MonoBehaviour {
	enum Member {
		NoSelect,
		Public,
		Tatsumi,
		Tamura,
		Saito,
		Murata,
	}

	[SerializeField] Member member;	// 担当者
	[SerializeField] string cmnt;   // 備考

	// Use this for initialization
	void Start() {
		if (member == Member.NoSelect) {
			Debug.LogWarning("担当者が設定されていません。\n" + name);
		}
	}

	// Update is called once per frame
//	void Update() {}
}
