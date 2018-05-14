using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

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
}
