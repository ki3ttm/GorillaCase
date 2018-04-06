using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceManager : MonoBehaviour {
	[SerializeField] float drag = 1.0f;	// 空気抵抗係数

	[Header("InspectorReadOnry")]
	[SerializeField] Vector3 vel;		// 現在の力
	[SerializeField] Vector3 velMove;	// 直前から現在への力の変化量(確認用)
	[SerializeField] Vector3 weightVel; // 重さによる力
	public Vector3 WeightVel { get { return weightVel; } set { weightVel = value; } }

	WeightManager weightMng = null;	// 重さレベルを管理するコンポーネント

	// Use this for initialization
	void Start () {
		weightMng = GetComponent<WeightManager>();
		if (weightMng == null) {
			Debug.LogError("WeightManagerが見つかりませんでした。\n" + MessageLog.GetNameAndPos(gameObject));
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
		Vector3 totalVel = (vel + fall);

		// 抵抗
		Vector3 befVel = vel;
		vel = AirResistance(totalVel);
		velMove = vel - befVel;

		transform.position += vel;
	}

	public void AddForce(Vector3 vec) {
	}

	Vector3 AirResistance(Vector3 _vel) {
		Vector3 resistanceVec = -_vel.normalized;
		float kv = _vel.magnitude * drag;
		return (_vel - resistanceVec * kv);
	}
}
