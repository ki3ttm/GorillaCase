using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SaitoTest_LayerMaskAttribute))]
public class SaitoTest_LayerMaskDrawer : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		if(property.propertyType == SerializedPropertyType.LayerMask) {
			SaitoTest_EditorTools.LayerMaskField(property.displayName, property.intValue);
		}
	}
}
