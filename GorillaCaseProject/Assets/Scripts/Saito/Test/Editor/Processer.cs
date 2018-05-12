using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEditor.Build;

using UnityEditor.Callbacks;

using UnityEditor;

using UnityEngine.UI;

public class Processer : IProcessScene
{

	public void OnProcessScene(UnityEngine.SceneManagement.Scene scene) {

		foreach(var t in GameObject.FindObjectsOfType<Floor>()) {
			t.Resize();
		}
	}



	// 実行順

	public int callbackOrder { get { return 0; } }

}