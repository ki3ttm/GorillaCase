using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaitoTest_PlayerMove : MonoBehaviour {

	CapsuleCollider mCollider;

	[SerializeField, SaitoTest_Disable]
	List<GameObject> mHitList;

	// Use this for initialization
	void Start () {
		mCollider = GetComponent<CapsuleCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void FixedUpdate()
	{
		LayerMask lLayerMask = LayerMask.GetMask(new string[] { "Stage", "Box" });

		Vector3 lMove = Vector3.zero;
		float lSpeed = 0.05f;
		if (Input.GetKey(KeyCode.A)) {
			lMove.x -= lSpeed;
		}
		if (Input.GetKey(KeyCode.D)) {
			lMove.x += lSpeed;
		}
		if (Input.GetKey(KeyCode.W)) {
			lMove.y += lSpeed;
		}
		if (Input.GetKey(KeyCode.S)) {
			lMove.y -= lSpeed;
		}

		
		Vector3 lDir = lMove;
		if (lDir.magnitude == 0.0f) {
			lDir = Vector3.up;
		}

		var r = Physics.CapsuleCastAll(mCollider.transform.position - mCollider.transform.rotation * Vector3.up * (mCollider.height / 2.0f - mCollider.radius),
			mCollider.transform.position + mCollider.transform.rotation * Vector3.up * (mCollider.height / 2.0f - mCollider.radius),
			mCollider.radius, lDir, lMove.magnitude, lLayerMask);

		float lDistance = float.MaxValue;
		foreach (var a in r) {
			float lNewDistance = a.distance - 0.0001f;

			if (lNewDistance < lDistance) {
				lDistance = lNewDistance;
			}
		}

		if (lDistance != float.MaxValue) {
			transform.position += lMove.normalized * lDistance;
		}
		else
		{
			transform.position += lMove;
		}






		//*/

		GetComponent<Rigidbody>().velocity = Vector3.zero;
	}
	
}
