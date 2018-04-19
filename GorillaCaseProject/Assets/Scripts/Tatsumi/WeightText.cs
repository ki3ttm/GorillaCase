using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightText : MonoBehaviour {
	[SerializeField] TextMesh textMesh = null;
	[SerializeField] WeightManager weightMng = null;

	// Use this for initialization
//	void Start () {}
	
	// Update is called once per frame
	void Update () {
		if ((textMesh != null) || (weightMng != null)) {
			textMesh.text = ((int)weightMng.WeightLv + "." + weightMng.WeightLv);
		}
	}
}
