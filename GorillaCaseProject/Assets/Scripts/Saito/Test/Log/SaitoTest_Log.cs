using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaitoTest_Log : MonoBehaviour {

	// Use this for initialization
	private void Start() {
		Debug.Log("Test", FindObjectOfType<Player>());
	}
}
