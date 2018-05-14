using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HikaruSwitch : MonoBehaviour {

	[SerializeField]
	GameObject mHikaru;

	[SerializeField]
	GameObject mHikaruTuyoi;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.T)) {
			mHikaru.SetActive(false);
			mHikaruTuyoi.SetActive(true);
		}
		if (Input.GetKeyDown(KeyCode.Y)) {
			mHikaru.SetActive(true);
			mHikaruTuyoi.SetActive(false);
		}
		if (Input.GetKeyDown(KeyCode.U)) {
			mHikaru.SetActive(false);
			mHikaruTuyoi.SetActive(false);
		}
	}
}
