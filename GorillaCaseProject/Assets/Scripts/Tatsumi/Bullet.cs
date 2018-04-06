using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	// 列挙型
	public enum Type {
		pull,
		push,
		hold,
	}

	[SerializeField] float lifeTime = 1.0f; // 消滅までの時間(秒)
	public float LifeTime { get { return lifeTime; } set { lifeTime = value; } }
	[SerializeField] WeightManager.Weight weight;

	[SerializeField] float vel = 0.0f; // 移動量
	public float Vel {
		get { return vel; }
		set {
			vel = value;
			// 最高移動量を超えていたら補正
			if (vel > MaxSpd) {
				vel = MaxSpd;
			}
		}
	}

	[SerializeField] float spd = 0.05f; // 加速量
	public float Spd { get { return spd; } private set { spd = value; } }

	[SerializeField] float maxSpd = 0.05f; // 最高移動量
	public float MaxSpd { get { return maxSpd; } private set { maxSpd = value; } }

	[SerializeField] ShooterManager shooterMng = null;   // 発生源のキャラクター
	public ShooterManager ShooterMng { get { return shooterMng; } set { shooterMng = value; } }

	[SerializeField] Type bulType;
	public Type BulType { get { return bulType; } private set { bulType = value; } }

	// Use this for initialization
	//	void Start () {}

	// Update is called once per frame
	void Update() {
		// 生存時間減少
		lifeTime -= Time.deltaTime;

		// 生存時間が切れたら
		if (lifeTime <= 0.0f) {
			// hold弾なら既存のホールドを解除
			if(bulType == Type.hold) {
				ShooterMng.HoldTarget = null;
			}

			// 消滅
			Destroy(gameObject);
			return;
		}

		// 加速
		vel += spd;

		// 移動
		transform.position += transform.forward * vel;
	}

	void OnTriggerEnter(Collider _col) {
		WeightManager colWeightMng = _col.GetComponent<WeightManager>();
		if (colWeightMng != null) {
			// 発生源オブジェクトが設定されていなければ
			if (ShooterMng == null) {
				Debug.LogError("発生源オブジェクトが設定されていません。\n" +
					"name:" + name + " position:" + transform.position);
				return;
			}

			switch (bulType) {
			case Type.pull: {
					// もう一方の対象を取得
					WeightManager otherWeightMng = GetOtherWeightManager();

					// 二つの対象の重さレベルを変化
					colWeightMng.PushWeight(otherWeightMng, 1);	// col -> other
					break;
				}
			case Type.push: {
					// もう一方の対象を取得
					WeightManager otherWeightMng = GetOtherWeightManager();

					// 二つの対象の重さレベルを変化
					colWeightMng.PullWeight(otherWeightMng, 1);	// col <- other
					break;
				}
			case Type.hold:
				// もう一方の対象として選択
				ShooterMng.HoldTarget = _col.gameObject;
				break;
			}
		}

		// 球消滅
		Destroy(gameObject);		
	}
	WeightManager GetOtherWeightManager() {
		// もう一方の対象を取得
		WeightManager otherWeightMng = null;
		// 保留中ターゲットがいれば
		if (ShooterMng.HoldTarget != null) {
			// 保留中ターゲットが対象になる
			otherWeightMng = ShooterMng.HoldTarget.GetComponent<WeightManager>();
			// 保留中ターゲットがいなければ
		} else {
			// 発生源オブジェクト自身が対象になる
			otherWeightMng = ShooterMng.GetComponent<WeightManager>();
		}
		return otherWeightMng;
	}
}
