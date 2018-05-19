using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	[SerializeField, EditOnPrefab]
	GameObject mSounds;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public GameObject Play(GameObject aSoundPrefab, float aDelay) {
		var lSoundInstance = Instantiate(aSoundPrefab, mSounds.transform);
		lSoundInstance.GetComponent<AudioSource>().PlayDelayed(aDelay);
		return lSoundInstance;
	}
	public GameObject Play(GameObject aSoundPrefab) {
		return Play(aSoundPrefab, 0.0f);
	}

	public void Stop(GameObject aSoundInstance) {
		aSoundInstance.GetComponent<AudioSource>().Stop();
		Destroy(aSoundInstance);
	}

	public void Pause(GameObject aSoundInstance, bool aPause) {
		if(aPause) {
			aSoundInstance.GetComponent<AudioSource>().Pause();
		}
		else {
			aSoundInstance.GetComponent<AudioSource>().UnPause();
		}
	}
}
