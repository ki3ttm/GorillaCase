using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
	ReorderableList reorderableList;

	void OnEnable()
	{
		var prop = serializedObject.FindProperty("mPrefabList");

		reorderableList = new ReorderableList(serializedObject, prop);

		reorderableList.drawElementCallback = (rect, index, isActive, isFocused) => {
			var element = prop.GetArrayElementAtIndex(index);
			rect.height -= 4;
			rect.y += 2;
			EditorGUI.PropertyField(rect, element);
		};
	}

	public override void OnInspectorGUI() {
		serializedObject.Update();
		reorderableList.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
	}
}