using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceManager : MonoBehaviour {
	struct Force {
		float vel;
	}
	
	[SerializeField] bool isGravity = true;			// 重力を適用
	[SerializeField] bool isAirResistance = true;	// 空気抵抗を適用
	[SerializeField] List<Force> forceList = new List<Force>();

	// Use this for initialization
//	void Start () {}
	
	// Update is called once per frame
//	void Update () {}

	void FixedUpdate() {
		
	}

	public void AddForce(Vector3 vec) {

	}
}
