using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSelfDestrouyer : MonoBehaviour {

	AudioSource mAS;

	// Use this for initialization
	void Start () {
		mAS = GetComponent<AudioSource>();
	}

	GameObject m;

	// Update is called once per frame
	void Update () {
		if(mAS.isPlaying == false) {
			Destroy(gameObject);
		}
	}
}
