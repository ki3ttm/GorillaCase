using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaitoTest_LayerMask))]
public class SaitoTest_LayerMaskEditor : Editor
{
	SaitoTest_LayerMask layerMask = null;

	private void OnEnable()
	{
		layerMask = (SaitoTest_LayerMask)target;
	}

	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();

		var lLayerMask = SaitoTest_EditorTools.LayerMaskField("Mask", layerMask.mLayerMask);

		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(layerMask, "Change hp");

			layerMask.mLayerMask = lLayerMask;
		}
	}
}