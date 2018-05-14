﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomPropertyDrawer(typeof(EditOnPrefab))]
public class EditOnPrefabDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position,
					  SerializedProperty property, GUIContent label)
	{
		label.text += " (EditOnPrefab)";

		if (IsPrefab(property))
		{
			EditorGUI.PropertyField(position, property, label);
		}
		else
		{
			property.prefabOverride = false;

			using (new EditorGUI.DisabledGroupScope(true))
			{
				EditorGUI.PropertyField(position, property, label);
			}
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