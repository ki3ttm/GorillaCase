using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomPropertyDrawer(typeof(InstanceOverride))]
public class InstanceOverrideDrawer : PropertyDrawer
{


	public override void OnGUI(Rect position,
					  SerializedProperty property, GUIContent label)
	{
		label.text += " (InstanceOverride)";

		if (IsPrefab(property))
		{
			using (new EditorGUI.DisabledGroupScope(true))
			{
				EditorGUI.PropertyField(position, property, label);
			}
		}
		else
		{
			//PrefabUtility.SetPropertyModifications(property.serializedObject.targetObject, );
			EditorGUI.PropertyField(position, property, label);
		}
	}


	static bool IsPrefab(SerializedProperty property) {
		var prefabType = PrefabUtility.GetPrefabType(property.serializedObject.targetObject);
		switch (prefabType)
		{
			case PrefabType.Prefab:
			case PrefabType.ModelPrefab:
				return true;
			default:
				return false;
		}
	}
}