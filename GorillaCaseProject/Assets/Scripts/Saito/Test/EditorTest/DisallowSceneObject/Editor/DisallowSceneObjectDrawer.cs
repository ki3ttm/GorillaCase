using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DisallowSceneObject))]
public class DisallowSceneObjectDrawer : PropertyDrawer
{

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		label.text += " (Prefab Only)";
		EditorGUI.PropertyField(position, property, label);

		if (property.objectReferenceValue != null)
		{
			var prefabType = PrefabUtility.GetPrefabType(property.objectReferenceValue);
			switch (prefabType)
			{
				case PrefabType.Prefab:
				case PrefabType.ModelPrefab:
					break;
				default:
					// Prefab以外がアタッチされた場合アタッチを外す
					property.objectReferenceValue = null;
					break;
			}
		}
	}
}
