using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomPropertyDrawer(typeof(ListAttribute))]
public class ListAttributeDrawer : PropertyDrawer {

	ReorderableList _reorderableList;

	public ListAttributeDrawer() {
		_reorderableList = null;
	}

	public override void OnGUI(Rect position,
					  SerializedProperty property, GUIContent label) {

		Rect lLabelRect = position;
		position.height = EditorGUIUtility.singleLineHeight;
		EditorGUI.LabelField(position, label);

		SerializedProperty listProperty = GetListProperty(property);
		ReorderableList reorderableList = GetList(listProperty);

		float height = 0f;

		for (var i = 0; i < listProperty.arraySize; i++) {
			height = Mathf.Max(height, EditorGUI.GetPropertyHeight(listProperty.GetArrayElementAtIndex(i)));
		}

		reorderableList.elementHeight = height;

		position.y += EditorGUIUtility.singleLineHeight;
		reorderableList.DoList(position);
	}
	
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		return GetList(GetListProperty(property)).GetHeight() + EditorGUIUtility.singleLineHeight;
	}

	private ReorderableList GetList(SerializedProperty aListProperty) {
		if (_reorderableList == null) {
			_reorderableList = new ReorderableList(aListProperty.serializedObject, aListProperty);

			_reorderableList.drawElementCallback = (rect, index, isActive, isFocused) => {
				var element = aListProperty.GetArrayElementAtIndex(index);
				rect.height -= 4;
				rect.y += 2;

				EditorGUI.PropertyField(rect, element);
				if(element.propertyType == SerializedPropertyType.ObjectReference) {
					GameObject go = element.objectReferenceValue as GameObject;
					if(go) {
						if(PrefabUtility.GetPrefabType(go) == PrefabType.Prefab || PrefabUtility.GetPrefabType(go) == PrefabType.ModelPrefab) {
							element.objectReferenceValue = null;
						}
					}
				}
			};
		}
		return _reorderableList;
	}
	private SerializedProperty GetListProperty(SerializedProperty aProperty)
	{
		SerializedProperty lListProperty = aProperty.FindPropertyRelative("list_");
		return lListProperty;
	}
}
