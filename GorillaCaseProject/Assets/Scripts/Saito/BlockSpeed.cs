using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlockSpeed : MonoBehaviour {
	
	[SerializeField]
	float mMaxSpeedInAir_Heavy;
	[SerializeField]
	float mMaxSpeedInAir_Light;
	[SerializeField]
	float mMaxSpeedInAir_Hover;

	[SerializeField]
	float mMaxSpeedInWater_Heavy;
	[SerializeField]
	float mMaxSpeedInWater_Light;
	[SerializeField]
	float mMaxSpeedInWater_Hover;


	[SerializeField]
	float mMaxSpeedSecondInAir_Heavy;
	[SerializeField]
	float mMaxSpeedSecondInAir_Light;
	[SerializeField]
	float mMaxSpeedSecondInAir_Hover;

	[SerializeField]
	float mMaxSpeedSecondInWater_Heavy;
	[SerializeField]
	float mMaxSpeedSecondInWater_Light;
	[SerializeField]
	float mMaxSpeedSecondInWater_Hover;

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


	float GetMaxSpeedInAir(WeightManager.Weight aWeight) {
		switch (aWeight)
		{
			case WeightManager.Weight.flying:
				return mMaxSpeedInAir_Hover;
			case WeightManager.Weight.light:
				return mMaxSpeedInAir_Light;
			case WeightManager.Weight.heavy:
				return mMaxSpeedInAir_Heavy;
		}
		Debug.LogError("Weightの値がおかしいです", this);
		return 0.0f;
	}

	float GetMaxSpeedInWater(WeightManager.Weight aWeight)
	{
		switch (aWeight)
		{
			case WeightManager.Weight.flying:
				return mMaxSpeedInWater_Hover;
			case WeightManager.Weight.light:
				return mMaxSpeedInWater_Light;
			case WeightManager.Weight.heavy:
				return mMaxSpeedInWater_Heavy;
		}
		Debug.LogError("Weightの値がおかしいです", this);
		return 0.0f;
	}

	public float GetMaxSpeed(WeightManager.Weight aWeight, CEnviroment aEnviroment) {
		
		switch (aEnviroment) {
			case CEnviroment.cAir:
				return GetMaxSpeedInAir(aWeight);
			case CEnviroment.cWater:
				return GetMaxSpeedInWater(aWeight);
		}
		Debug.LogError("Weightの値がおかしいです", this);
		return 0.0f;
	}


	Vector3 GetAccelInAir(WeightManager.Weight aWeight)
	{
		switch (aWeight)
		{
			case WeightManager.Weight.flying:
				return new Vector3(0.0f, mMaxSpeedInAir_Hover / mMaxSpeedSecondInAir_Hover, 0.0f);
			case WeightManager.Weight.light:
				return new Vector3(0.0f, mMaxSpeedInAir_Light / mMaxSpeedSecondInAir_Light, 0.0f);
			case WeightManager.Weight.heavy:
				return new Vector3(0.0f, mMaxSpeedInAir_Heavy / mMaxSpeedSecondInAir_Heavy, 0.0f);
		}
		Debug.LogError("Weightの値がおかしいです", this);
		return new Vector3(0.0f, 0.0f, 0.0f);
	}

	Vector3 GetAccelInWater(WeightManager.Weight aWeight)
	{
		switch (aWeight)
		{
			case WeightManager.Weight.flying:
				return new Vector3(0.0f, mMaxSpeedInWater_Hover / mMaxSpeedSecondInWater_Hover, 0.0f);
			case WeightManager.Weight.light:
				return new Vector3(0.0f, mMaxSpeedInWater_Light / mMaxSpeedSecondInWater_Light, 0.0f);
			case WeightManager.Weight.heavy:
				return new Vector3(0.0f, mMaxSpeedInWater_Heavy / mMaxSpeedSecondInWater_Heavy, 0.0f);
		}
		Debug.LogError("Weightの値がおかしいです", this);
		return new Vector3(0.0f, 0.0f, 0.0f);
	}

	public Vector3 GetAccel(WeightManager.Weight aWeight, CEnviroment aEnviroment)
	{
		switch (aEnviroment)
		{
			case CEnviroment.cAir:
				return GetAccelInAir(aWeight);
			case CEnviroment.cWater:
				return GetAccelInWater(aWeight);
		}
		Debug.LogError("Weightの値がおかしいです", this);
		return new Vector3(0.0f, 0.0f, 0.0f);
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

	public static int GetUpForce(WeightManager.Weight aWeight, CEnviroment aEnviroment)
	{
		return Mathf.Clamp(GetForce(aWeight, aEnviroment), -10, 0) * -1;
	}
	public static int GetDownForce(WeightManager.Weight aWeight, CEnviroment aEnviroment)
	{
		return Mathf.Clamp(GetForce(aWeight, aEnviroment), 0, 10);
	}
	public static int GetForce(WeightManager.Weight aWeight, CEnviroment aEnviroment) {
		switch (aWeight) {
			case WeightManager.Weight.flying:
				if (aEnviroment == CEnviroment.cAir) {
					return -1;
				}
				else {
					return -2;
				}
			case WeightManager.Weight.light:
				if (aEnviroment == CEnviroment.cAir) {
					return 1;
				}
				else {
					return -1;
				}
			case WeightManager.Weight.heavy:
				if (aEnviroment == CEnviroment.cAir) {
					return 2;
				}
				else {
					return 2;
				}
		}
		return -1;
	}
}
