using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaitoTest_BlockAccel : MonoBehaviour {

	[SerializeField]
	Vector3 acceleration = new Vector3(0.0f, -9.8f, 0.0f);

	[SerializeField]
	bool isReverse = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void FixedUpdate() {

		Vector3 lAccel;
		if(isReverse == false) {
			lAccel = acceleration;
		}
		else {
			lAccel = -acceleration;
		}

		GetComponent<Rigidbody>().AddForce(lAccel, ForceMode.Acceleration);
	}
}
