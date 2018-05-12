using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saito_Range2Attribute_Test : MonoBehaviour {

	[SerializeField, Saito_Range2(0, 10)]
	int hp;

	[SerializeField, Saito_Range2(0, 10)]
	string str;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
