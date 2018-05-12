using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.AttributeUsage (System.AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class Saito_Range2Attribute : PropertyAttribute {

	public readonly int min;
	public readonly int max;
	
	public Saito_Range2Attribute(int min, int max) {
		this.min = min;
		this.max = max;
	}
}
