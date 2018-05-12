using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine.Events;

public class Saito_EditorGUI : EditorWindow {

	static Saito_EditorGUI window;

	[MenuItem("Window/SaitoEditorGUI")]
	static void Open() {
		if(window == null) {
			window = CreateInstance<Saito_EditorGUI>();
		}
		window.ShowUtility();
	}
}