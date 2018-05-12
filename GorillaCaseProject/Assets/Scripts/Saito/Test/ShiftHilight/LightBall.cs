using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LightBall : MonoBehaviour {

	Vector3 mFrom;
	Vector3 mTo;

	[SerializeField]
	GameObject mCollider;

	public List<GameObject> mIgnoreList = new List<GameObject>();

	//Toまで到達したか
	bool mIsReached = false;
	public bool IsReached { 
		get { return mIsReached; }
	}

	bool mIsHit = false;
	public bool IsHit {
		get { return mIsHit; }
	}

	[SerializeField, Tooltip("光の弾が進む速度")]
	float mMoveSpeed = 1.0f;

	float mFromDistance;    //Fromから進んだ距離

	Vector3 mBeforePosition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		mFromDistance += Time.deltaTime * mMoveSpeed;

		UpdatePosition();

		ReachCheck();
		HitCheck();
	}


	public void UpdatePoint(Vector3 aFrom, Vector3 aTo) {
		mFrom = aFrom;
		mTo = aTo;

		UpdatePosition();
		ReachCheck();
		HitCheck();
	}
	public void SetPoint(Vector3 aFrom, Vector3 aTo) {
		mFromDistance = 0.0f;
		mIsHit = false;
		mBeforePosition = aFrom;
		UpdatePoint(aFrom, aTo);
	}
	
	void ReachCheck() {
		if ((mTo - mFrom).magnitude <= mFromDistance) {
			mIsReached = true;
		}
		else {
			mIsReached = false;
		}
	}

	void UpdatePosition() {

		Vector3 lDir = mTo - mFrom;
		transform.rotation = Quaternion.FromToRotation(Vector3.right, lDir);

		//距離を進む
		transform.position = mFrom + lDir.normalized * mFromDistance;
	}

	//射線が通っているか
	public bool ThroughShotLine(Vector3 aFrom, Vector3 aTo, List<GameObject> aIgnoreList = null) {

		Vector3 lDir = (aTo - aFrom);
		if(lDir.magnitude == 0.0f) {
			lDir = Vector3.up;
		}

		LayerMask l = LayerMask.GetMask(new string[] { "Box", "Stage" });
		var rcs = Physics.SphereCastAll(aFrom, mCollider.transform.lossyScale.x / 2.0f, lDir, (aTo - aFrom).magnitude, l);
		foreach(var rc in rcs) {
			Debug.Log("Hit!");
			if (aIgnoreList == null) return false;
			if (aIgnoreList.Contains(rc.collider.gameObject) == false) return false;
		}
		return true;
	}
	public bool ThroughShotLine(Vector3 aFrom, Vector3 aTo, GameObject aIgnore)
	{
		return ThroughShotLine(aFrom, aTo, new GameObject[] { aIgnore }.ToList());
	}

	bool HitCheck() {
		bool res = ThroughShotLine(mBeforePosition, transform.position, mIgnoreList);
		mBeforePosition = transform.position;
		return !res;
	}
}
