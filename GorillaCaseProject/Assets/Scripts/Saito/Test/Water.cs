﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[SerializeField, Tooltip("横の大きさ")]
	float mWidth;

	[SerializeField, Tooltip("縦の大きさ")]
	float mHeight;

	[SerializeField]
	GameObject mModel;

	[SerializeField]
	GameObject mCollider;

	private void OnValidate() {
		mModel.transform.localScale = new Vector3(mWidth, mHeight, 1.0f);
		mModel.transform.localPosition = new Vector3(mWidth / 2.0f, -mHeight / 2.0f, 0.0f);

		mCollider.transform.localScale = new Vector3(mWidth, mHeight, 1.0f);
		mCollider.transform.localPosition = new Vector3(mWidth / 2.0f, -mHeight / 2.0f, 0.0f);
	}
}
