using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceManager : MonoBehaviour {
	[SerializeField] float drag = 1.0f;	// 空気抵抗係数
	WeightManager weightMng = null;     // 重さレベルを管理するコンポーネント
	List<Vector3> forceList = new List<Vector3>();

	// Use this for initialization
	void Start () {
		weightMng = GetComponent<WeightManager>();
		if (weightMng == null) {
			Debug.LogError("WeightManagerが見つかりませんでした。\n" + MessageLog.GetNameAndPos(gameObject));
			return;
		}
	}
	
	// Update is called once per frame
//	void Update () {}

	void FixedUpdate() {
		Vector3 force = Vector3.zero;

		// 落下/浮上
		force = -weightMng.GetWeightFallVel();

		// その他の力
		while (forceList.Count > 0) {
			force += forceList[0];
			forceList.RemoveAt(0);
		}

		// 抵抗
		force = AirResistance(force);

		GetComponent<Rigidbody>().AddForce(force);
	}

	public void AddForce(Vector3 _force) {
		forceList.Add(_force);
	}

	Vector3 AirResistance(Vector3 _vel) {
		Vector3 resistanceVec = -_vel.normalized;
		float kv = _vel.magnitude * drag;
		return (_vel - resistanceVec * kv);
	}
}
