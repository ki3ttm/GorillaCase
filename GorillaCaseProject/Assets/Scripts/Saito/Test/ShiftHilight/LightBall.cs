using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LightBall : MonoBehaviour {

	Vector3 mFrom;
	Vector3 mTo;

	public Vector3 FromPoint {
		get { return mFrom; }
	}
	public Vector3 ToPoint {
		get { return mTo; }
	}

	[SerializeField]
	GameObject mCollider;

	[SerializeField]
	GameObject mModel;

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

	[Tooltip("光の弾が進む速度")]
	public float mMoveSpeed = 1.0f;

	float mFromDistance;    //Fromから進んだ距離

	Vector3 mBeforePosition;

	bool lBeforeCheck = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {

		if(!mModel.activeSelf) {
			if(lBeforeCheck == true) {
				mModel.SetActive(true);
			}
			else {
				lBeforeCheck = true;
			}
		}
	}


	public void UpdatePoint() {
		
		mFromDistance += Time.deltaTime * mMoveSpeed;
		ReachCheck();

		mFromDistance = Mathf.Min((mFrom - mTo).magnitude, mFromDistance);
		UpdatePosition();
		HitCheck();
	}
	public void SetPoint(Vector3 aFrom, Vector3 aTo) {
		mFrom = aFrom;
		mTo = aTo;
	}
	public void InitPoint(Vector3 aFrom, Vector3 aTo) {
		mFromDistance = 0.0f;
		mIsHit = false;
		transform.position = aFrom;
		SetPoint(aFrom, aTo);
		mModel.SetActive(false);
		lBeforeCheck = false;
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
		mBeforePosition = transform.position;
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
			if (aIgnoreList == null) return false;
			if (aIgnoreList.Contains(rc.collider.gameObject) == false) {
				return false;
			}
		}
		return true;
	}
	public bool ThroughShotLine(Vector3 aFrom, Vector3 aTo, GameObject aIgnore)
	{
		return ThroughShotLine(aFrom, aTo, new GameObject[] { aIgnore }.ToList());
	}

	void HitCheck() {
		if(ThroughShotLine(mBeforePosition, transform.position, mIgnoreList)) {
			mIsHit = false;
		}
		else {
			mIsHit = true;
		}
	}

	public void Play() {
		foreach(var p in mModel.GetComponentsInChildren<ParticleSystem>()) {
			p.Play();
		}
		foreach (var p in mModel.GetComponentsInChildren<MeshRenderer>()) {
			p.enabled = true;
		}
	}
	public void Stop() {
		foreach (var p in mModel.GetComponentsInChildren<ParticleSystem>()) {
			p.Stop();
		}
		foreach (var p in mModel.GetComponentsInChildren<MeshRenderer>()) {
			p.enabled = false;
		}
	}
}
