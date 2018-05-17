using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Play(GameObject aSoundPrefab, float aDelay) {
		var g = Instantiate(aSoundPrefab, transform);
		g.GetComponent<AudioSource>().PlayDelayed(aDelay);
	}
	public void Play(GameObject aSoundPrefab) {
		Play(aSoundPrefab, 0.0f);
	}
}
