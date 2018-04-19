using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlockSpeed : MonoBehaviour {

	[Serializable]
	class AccelSet {

		public AccelSet(float aHoverAccel, float aLightAccel, float aHeavyAccel) {
			mHoverAccel = aHoverAccel;
			mLightAccel = aLightAccel;
			mHeavyAccel = aHeavyAccel;
		}

		public float mHoverAccel;
		public float mLightAccel;
		public float mHeavyAccel;
	}

	[SerializeField]
	AccelSet mAccelSetInAir = new AccelSet(0.5f, -3.0f, -3.0f);

	[SerializeField]
	AccelSet mAccelSetInWater = new AccelSet(0.5f, -3.0f, -3.0f);
	

	public enum CEnviroment {
		cAir,
		cWater,
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	Vector3 GetAccel(AccelSet aAccelSet, WeightManager.Weight aWeight) {
		switch (aWeight) {
			case WeightManager.Weight.flying:
				return new Vector3(0.0f, aAccelSet.mHoverAccel, 0.0f);
			case WeightManager.Weight.light:
				return new Vector3(0.0f, aAccelSet.mLightAccel, 0.0f);
			case WeightManager.Weight.heavy:
				return new Vector3(0.0f, aAccelSet.mHeavyAccel, 0.0f);
		}
		return new Vector3(0.0f, 0.0f, 0.0f);
	}

	public Vector3 GetAccel(WeightManager.Weight aWeight, CEnviroment aEnviroment) {

		AccelSet lAccelSet = null;

		switch(aEnviroment) {
			case CEnviroment.cAir:
				lAccelSet = mAccelSetInAir;
				break;
			case CEnviroment.cWater:
				lAccelSet = mAccelSetInWater;
				break;
		}
		
		return GetAccel(lAccelSet, aWeight);
	}

	public static int GetWeight(WeightManager.Weight aWeight) {
		switch (aWeight)
		{
			case WeightManager.Weight.flying:
				return 0;
			case WeightManager.Weight.light:
				return 1;
			case WeightManager.Weight.heavy:
				return 2;
		}
		return -1;
	}
}
