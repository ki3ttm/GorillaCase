using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderInspector : MonoBehaviour {

	[SerializeField]
	float sumTime;

	[SerializeField, Range(0.0f, 10.0f)]
	float time1;

	[SerializeField, Range(0.0f, 10.0f)]
	float time2;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
