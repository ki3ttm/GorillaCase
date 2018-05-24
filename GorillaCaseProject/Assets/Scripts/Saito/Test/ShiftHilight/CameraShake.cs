using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	/*
	Vector3 mOriginPosition;

	[SerializeField]
	float mMagnitude = 0.2f;

	float mTargetShakeTime = 0.0f;
	float mShakeTime = 0.0f;

	// Use this for initialization
	void Start () {
		mOriginPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		mShakeTime += Time.deltaTime;
		
		if(IsShake) {
			if(Time.timeScale != 0.0f) {
				transform.position = mOriginPosition + Random.insideUnitSphere * mMagnitude;
			}
		}
		else {
			transform.position = mOriginPosition;
		}
	}

	public bool IsShake {
		get {
			if (mShakeTime <= mTargetShakeTime) {
				return true;
			}
			return false;
		}
	}

	public void ShakeStart(float aTime) {
		mTargetShakeTime = aTime;
		mShakeTime = 0.0f;
	}
	*/


	Vector3 mBeforeMove;

	[SerializeField, Tooltip("振動の大きさ")]
	float mMagnitude = 0.2f;

	//振動し始めてから何秒か
	float mShakeTimeFromStart = 0.0f;

	//振動する秒数
	float mShakeTime = 0.0f;

	// Use this for initialization
	void Start() {
	}

	// Update is called once per frame
	void Update() {

		//ゲームが止まっていたら処理をしない
		if (Time.timeScale == 0.0f) {
			return;
		}

		//揺れた時間を増やす
		mShakeTimeFromStart += Time.deltaTime;

		//元の位置に戻す
		transform.position -= mBeforeMove;

		//もし振動中なら
		if (IsShake) {
			Vector3 lMove = Random.insideUnitSphere * mMagnitude;
			transform.position += lMove;
			mBeforeMove = lMove;
		}
		else {
			mBeforeMove = Vector3.zero;
		}
	}

	public bool IsShake {
		get {
			if (mShakeTimeFromStart <= mShakeTime) {
				return true;
			}
			return false;
		}
	}

	public void ShakeStart(float aTime) {
		mShakeTime = aTime;
		mShakeTimeFromStart = 0.0f;
	}
}
