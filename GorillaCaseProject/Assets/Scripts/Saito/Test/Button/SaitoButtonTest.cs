using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaitoButtonTest : MonoBehaviour {

	[SerializeField]
	List<GameObject> mButtonList;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		int lTotal = 0;
		foreach(var lButton in mButtonList) {
			if (lButton.GetComponent<Button>() == null) continue;
			if(lButton.GetComponent<Button>().IsButtonOn() == true) {
				lTotal += 1;
			}
		}
		Debug.Log("ButtonOn:" + lTotal);
	}
}
