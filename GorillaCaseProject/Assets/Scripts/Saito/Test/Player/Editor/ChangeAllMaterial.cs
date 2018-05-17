using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChangeAllMaterial : EditorWindow {

	static ChangeAllMaterial window;

	[MenuItem("Window/ChangeAllMaterial")]
	static void Open() {
		if(window == null)
		{
			window = CreateInstance<ChangeAllMaterial>();
		}
		window.ShowUtility();
	}

	Material mMat;
	Material mTargetMat;

	private void OnGUI() {

		mMat = EditorGUILayout.ObjectField("Material", mMat, typeof(Material), false) as Material;
		mTargetMat = EditorGUILayout.ObjectField("Target", mTargetMat, typeof(Material), false) as Material;

		if(GUILayout.Button("Change")) {
			foreach(var s in Selection.gameObjects) {
				ChangeMaterial(s, mMat, mTargetMat);
			}
		}
	}


	public static void ChangeMaterial(GameObject aGameObject, Material aMaterial, Material aTarget) {

		Renderer[] renderers = aGameObject.GetComponentsInChildren<Renderer>();
		foreach (var r in renderers) {

			Material[] materials = r.sharedMaterials;
			bool lIsChange = false;
			for(int i = 0; i < materials.Length; i++) {
				if(materials[i] == aTarget || aTarget == null) {
					lIsChange = true;
					materials[i] = aMaterial;
				}
			}
			if (lIsChange) {
				r.sharedMaterials = materials;
			}
		}
	}
}
