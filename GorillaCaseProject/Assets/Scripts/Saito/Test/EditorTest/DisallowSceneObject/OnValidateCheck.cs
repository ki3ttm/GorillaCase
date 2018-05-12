using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnValidateCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[SerializeField]
	GameObject aPrefab;

	private void OnValidate()
	{
		Instantiate(aPrefab);
	}
}
