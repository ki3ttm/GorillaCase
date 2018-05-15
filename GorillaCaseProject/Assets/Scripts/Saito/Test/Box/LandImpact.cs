using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandImpact : MonoBehaviour {

	[SerializeField]
	GameObject mLandCollider;

	bool mBeforeLanding = false;
	Vector3 mHighestPosition;

	[SerializeField, Tooltip("この距離以上を落下すると、落下演出が起きる")]
	float mImpactDistance = 1.0f;

	[SerializeField, Tooltip("重さ1のときに跳ねる大きさ")]
	float mBoundMagnitude = 1.0f;

	[SerializeField, Tooltip("重さ1のときに跳ねた音"), EditOnPrefab]
	AudioClip mBoundLightSE;

	[SerializeField, Tooltip("重さ2のときに落ちた音"), EditOnPrefab]
	AudioClip mBoundHeavySE;

	// Use this for initialization
	void Start () {
		mHighestPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		
	}

	private void FixedUpdate()
	{
		//上向きに進んでいたら
		if (GetComponent<Rigidbody>().velocity.y >= 0.0f)
		{
			mHighestPosition = transform.position;  //最高地点を更新
		}

		bool lIsLanding = IsLanding();

		//このフレームに接地し始めていて
		if (mBeforeLanding == false && lIsLanding == true)
		{

			//一定距離以上落ちていたら
			if (mHighestPosition.y - transform.position.y >= mImpactDistance)
			{
				OnLanding();
			}
		}

		mBeforeLanding = lIsLanding;
	}

	void OnLanding() {
		
		if(GetComponent<WeightManager>().WeightLv == WeightManager.Weight.heavy) {
			foreach (var c in FindObjectsOfType<CameraShake>()) {
				c.ShakeStart(0.3f);
				GetComponent<AudioSource>().PlayOneShot(mBoundHeavySE);
			}
		}
		if (GetComponent<WeightManager>().WeightLv == WeightManager.Weight.light) {
			GetComponent<Rigidbody>().AddForce(Vector3.up * mBoundMagnitude, ForceMode.VelocityChange);
			GetComponent<AudioSource>().PlayOneShot(mBoundLightSE);
		}
	}

	bool IsLanding() {
		return Physics.OverlapBox(mLandCollider.transform.position, mLandCollider.transform.lossyScale / 2, mLandCollider.transform.rotation, LayerMask.GetMask(new string[] { "Stage", "Box" })).Length > 0;
	}
}
