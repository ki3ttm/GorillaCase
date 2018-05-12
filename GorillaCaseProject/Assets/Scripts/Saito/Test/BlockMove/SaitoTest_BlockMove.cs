using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaitoTest_BlockMove : MonoBehaviour {

	[SerializeField]
	float mFallMaxSecond;	//何秒で最高速度に到達するか

	[SerializeField]
	float mFallMaxSpeed;	//一秒間で進む最高速度

	Vector3 mSpeed;

	[SerializeField]
	bool mIsReverse;

	enum CState {
		cStop,
		cWater
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void FixedUpdate()
	{
		if(!mIsReverse) {
			mSpeed.y -= mFallMaxSpeed / mFallMaxSecond * Time.fixedDeltaTime;
		}
		else {
			mSpeed.y += mFallMaxSpeed / mFallMaxSecond * Time.fixedDeltaTime;
		}
		mSpeed.y = Mathf.Clamp(mSpeed.y, -mFallMaxSpeed, mFallMaxSpeed);

		//速度を0にする
		GetComponent<Rigidbody>().velocity = Vector3.zero;

		RaycastHit r;

		LayerMask l = LayerMask.GetMask(new string[] { "Default" });
		var lc = Physics.BoxCast(transform.position, transform.localScale / 2.0f, mSpeed, out r, Quaternion.identity, mSpeed.magnitude * Time.fixedDeltaTime, l);

		if (lc)
		{
			GetComponent<Rigidbody>().MovePosition(transform.position + mSpeed.normalized * r.distance);
			mSpeed.y = 0.0f;
		}
		else
		{
			GetComponent<Rigidbody>().MovePosition(transform.position + mSpeed.normalized * mSpeed.magnitude * Time.fixedDeltaTime);
		}
		
	}

}
