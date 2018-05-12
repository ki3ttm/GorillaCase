using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Redirect : MonoBehaviour {

	[SerializeField]
	GameObject mRedirect;

	public GameObject GetRedirect() {
		return mRedirect;
	}
}
