using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SaitoTest_DisableAttribute))]
public class SaitoTest_DisableDrawer : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		using (new EditorGUI.DisabledGroupScope(true))
		{
			EditorGUI.PropertyField(position, property, label);
		}
	}

}