using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer (typeof(Saito_Range2Attribute))]
internal sealed class Saito_RangeDrawer : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		Saito_Range2Attribute range2 = (Saito_Range2Attribute)attribute;

		if(property.propertyType == SerializedPropertyType.Integer) {
			EditorGUI.IntSlider(position, property, range2.min, range2.max, label);
		}
		else {
			EditorGUI.PropertyField(position, property, label);
		}
	}

}
