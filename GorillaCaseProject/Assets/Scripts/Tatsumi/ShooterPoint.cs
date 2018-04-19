using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterPoint : MonoBehaviour {
	[SerializeField] bool isStandby = true;
	public bool IsStandby { get { return isStandby; } private set { isStandby = value; } }
	[SerializeField] float coolTime = 0.0f;
	public float CoolTime { get { return coolTime; } set { coolTime = value; } }

	// Use this for initialization
//	void Start () {}
	
	// Update is called once per frame
	void Update () {
		// 使用不可中であれば
		if (!IsStandby) {
			// 使用不可時間を減少
			CoolTime -= Time.deltaTime;
			// 使用不可時間が終了したら
			if (CoolTime <= 0.0f) {
				// 使用可能
				IsStandby = true;
				CoolTime = 0.0f;
			}
		}
	}
}
