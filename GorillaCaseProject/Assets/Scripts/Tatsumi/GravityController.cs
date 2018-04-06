using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour {
	[SerializeField] float gravityLv = 0.0f;
	public float GravityLv { get { return gravityLv; } set { gravityLv = value; } }

	// Use this for initialization
//	void Start () {}
	
	// Update is called once per frame
//	void Update () {}

	void FixedUpdate() {
		GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, -GravityLv, 0.0f), ForceMode.Acceleration);
	}
}
