using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaitoTest_BlockAccel_Joint : MonoBehaviour {

	[SerializeField]
	Vector3 downAcceleration = new Vector3(0.0f, -9.8f, 0.0f);

	[SerializeField]
	Vector3 upAcceleration = new Vector3(0.0f, 9.8f, 0.0f);

	[SerializeField]
	bool isReverse = false;


	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	private void FixedUpdate()
	{

		Vector3 lAccel;
		if (isReverse == false)
		{
			lAccel = downAcceleration;
		}
		else
		{
			lAccel = upAcceleration;
		}

		foreach(var a in GetComponentsInChildren<Rigidbody>()) {
			
			a.AddForce(lAccel, ForceMode.Acceleration);
		}
	}
}
