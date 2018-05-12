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
		
	}
}