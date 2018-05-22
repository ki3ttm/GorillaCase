using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	[SerializeField]
	List<GameObject> mPrefabList;

	private void Awake() {

		foreach(var p in mPrefabList) {
			Instantiate(p, transform);
		}
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
