using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine.Events;

public class Saito_EditorGUI : EditorWindow {

	static Saito_EditorGUI window;

	Object g;

	[MenuItem("Window/SaitoEditorGUI")]
	static void Open() {
		if(window == null) {
			window = CreateInstance<Saito_EditorGUI>();
		}
		window.ShowUtility();
	}

	private void OnGUI() {
		
		g = EditorGUILayout.ObjectField(g, typeof(Object));

		if (GUI.Button(new Rect(new Vector2(10.0f, 10.0f), new Vector2(200.0f, 20.0f)), "Button")) {
			Debug.Log("押されました" + g);
		}
	}
}