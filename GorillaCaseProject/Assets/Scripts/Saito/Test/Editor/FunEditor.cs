using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Fun))]
public class FunEditor : Editor
{
	/*
	public override bool HasPreviewGUI()
	{
		return true;
	}

	public override GUIContent GetPreviewTitle()
	{
		return new GUIContent("プレビュー名");
	}
	public override void OnPreviewGUI(Rect r, GUIStyle background)
	{
		GUI.Box(r, "Preview");
	}
	*/
	private void OnSceneGUI()
	{
		Fun f = target as Fun;
	}
}