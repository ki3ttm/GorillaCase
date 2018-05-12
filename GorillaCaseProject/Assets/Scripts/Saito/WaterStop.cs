using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterStop : MonoBehaviour {

	// Use this for initialization
	void Start () {
		mCollider = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		mIsInWater = true;
		DisableCollision();
	}

	private void OnTriggerExit(Collider other)
	{
		mIsInWater = false;
	}

	//水との当たり判定を取る
	public void EnableCollision() {
		mCollider.isTrigger = false;
	}
	//水との当たり判定を取らないようにする
	public void DisableCollision() {
		mCollider.isTrigger = true;
	}

	public bool IsInWater() {
		return mIsInWater;
	}

	//現在水の中に居るか
	[SerializeField, SaitoTest_Disable]
	bool mIsInWater;

	//水との当たり判定を取るコライダー＾
	BoxCollider mCollider;
}
