using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saito_ExtendTest_Getter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Saito_ExtendTest_A a = GetComponent<Saito_ExtendTest_A>();
		Debug.Log(a);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
