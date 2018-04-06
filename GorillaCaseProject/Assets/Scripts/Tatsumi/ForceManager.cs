using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceManager : MonoBehaviour {
	[Header("InspectorReadOnry")]
	[SerializeField] Vector3 vel;		// 現在の力
	[SerializeField] Vector3 weightVel; // 重さによる力
	public Vector3 WeightVel { get { return weightVel; } set { weightVel = value; } }

	WeightManager weightMng = null;	// 重さレベルを管理するコンポーネント

	// Use this for initialization
	void Start () {
		weightMng = GetComponent<WeightManager>();
		if(weightMng == null) {
			Debug.LogError("WeightManagerが見つかりませんでした。\n" +
				"name:" + name + " position:" + transform.position);
			return;
		}
		WeightVel = weightMng.GetWeightFallVel();
	}
	
	// Update is called once per frame
//	void Update () {}

	void FixedUpdate() {
		// 落下/浮上
		Vector3 fall = -weightMng.GetWeightFallVel();

		// 合力
		Vector3 totalVel = (fall);

		// 抵抗
		vel = AirResistance(totalVel);

		transform.position += vel;
	}

	public void AddForce(Vector3 vec) {
	}

	Vector3 AirResistance(Vector3 _vel) {
		Vector3 resistanceVec = -vel.normalized;
		float v = vel.magnitude;
		return (_vel - resistanceVec * v);
	}
}
