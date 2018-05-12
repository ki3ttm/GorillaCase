using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DebugSwitch {

	const string sMenuPath = "Debug/DebugDisable";

	[MenuItem(sMenuPath)]
	static void DebugDisable() {

		bool lIsChecked = Menu.GetChecked(sMenuPath);

		foreach(var a in Resources.FindObjectsOfTypeAll<DebugObject>()) {

			//アセットなら操作しない
			if (a.hideFlags == HideFlags.NotEditable || a.hideFlags == HideFlags.HideAndDontSave) {
				continue;
			}

			a.gameObject.SetActive(lIsChecked);
		}

		Menu.SetChecked(sMenuPath, !lIsChecked);

	}

}
