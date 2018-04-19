using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageLog : MonoBehaviour {
	// Use this for initialization
//	void Start () {}
	
	// Update is called once per frame
//	void Update () {}

	static public string GetNameAndPos(GameObject _obj) {
		// メッセージを構築して返す
		return ("name:" + _obj.name + " position:" + _obj.transform.position);
	}
}
