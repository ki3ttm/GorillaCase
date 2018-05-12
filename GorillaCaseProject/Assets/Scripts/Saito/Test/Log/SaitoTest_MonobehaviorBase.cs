using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaitoTest_MonobehaviorBase : MonoBehaviour {

	// Use this for initialization
	public void Start () {
		try {
			StartEx();
		}
		catch (NullReferenceException ex) {
			System.Diagnostics.StackTrace sTrace = new System.Diagnostics.StackTrace();
			Debug.Log(sTrace.ToString());
			throw;
		}
	}
	protected virtual void StartEx() { }


	// Update is called once per frame
	public void Update () {
		UpdateEx();
	}
	protected virtual void UpdateEx() { }

	public void FixedUpdate() {
		FixedUpdateEx();
	}
	protected virtual void FixedUpdateEx() { }
}
