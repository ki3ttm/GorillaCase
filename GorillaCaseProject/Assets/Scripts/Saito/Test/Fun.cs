using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Fun : MonoBehaviour {

	[SerializeField]
	GameObject mRotateModel;

	[SerializeField, Tooltip("何秒で1回転するか")]
	float mRotateSecond = 2.0f;

	enum CDirection {
		cUp,
		cDown,
		cLeft,
		cRight,
	}
	static Vector3 GetDirection(CDirection aDirection) {
		switch(aDirection) {
			case CDirection.cDown:
				return Vector3.down;
			case CDirection.cRight:
				return Vector3.right;
			case CDirection.cUp:
				return Vector3.up;
			case CDirection.cLeft:
				return Vector3.left;
		}
		return Vector3.right;
	}

	[SerializeField, Tooltip("風の向き")]
	CDirection mWindDirection;

	[SerializeField, SaitoTest_Disable]
	GameObject mWindHitObject;


	[SerializeField]
	BoxCollider mHit;

	[SerializeField]
	List<BoxCollider> mNotHit;
	

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Rotate();

		WindHit();
	}

	void Rotate() {
		mRotateModel.transform.rotation *= Quaternion.Euler(360.0f / mRotateSecond * Time.deltaTime, 0.0f, 0.0f);
	}

	void WindHit() {
		var lHit = GetBoxCast(mHit).ToList();

		var lNotHit = new List<RaycastHit>();
		foreach(var n in mNotHit) {
			lNotHit = lNotHit.Union(GetBoxCast(n)).ToList();
		}

		foreach(var n in lNotHit) {
			if(lHit.Any(r => r.collider == n.collider)) {
				if(n.collider.gameObject.layer != LayerMask.NameToLayer("Stage")) {
					lHit.RemoveAll(r => r.collider == n.collider);
				}
			}
		}

		GameObject lNear = null;
		float lDistance = float.MaxValue;
		foreach(var h in lHit) {
			if (h.distance < lDistance) {
				lDistance = h.distance;
				lNear = h.collider.gameObject;
			}
		}

		if (lNear == null)
		{
			mWindHitObject = null;
			return;
		}
		if (lNear.layer == LayerMask.NameToLayer("Stage"))
		{
			mWindHitObject = null;
			return;
		}

		mWindHitObject = lNear;
	}

	RaycastHit[] GetBoxCast(BoxCollider c) {

		LayerMask l = LayerMask.GetMask(new string[] { "Player", "Box", "Stage" });
		
		var res = Physics.BoxCastAll(c.bounds.center, c.bounds.size / 2.0f, GetDirection(mWindDirection), Quaternion.identity, float.MaxValue, l);

		return res;
	}

	private void OnDrawGizmos()
	{
		Vector3 lWindDirection = GetDirection(mWindDirection);

		float lDistance = 1.0f;
		Vector3 lEndPoint = transform.position + lWindDirection * lDistance;

		Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
		Gizmos.DrawLine(transform.position, lEndPoint);
		Gizmos.DrawWireSphere(lEndPoint, 0.2f);
	}

	private void OnValidate()
	{
		Vector3 lWindDirection = GetDirection(mWindDirection);

		if (lWindDirection.normalized == Vector3.up || lWindDirection.normalized == Vector3.down) {
			transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
		}
		else {
			transform.rotation = Quaternion.identity;
		}
	}
}
