using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPause : MonoBehaviour {

	[SerializeField]
	GameObject mPlayText;

	[SerializeField]
	GameObject mPauseText;

	float mBeforeTimeScale;

	[SerializeField]
	GameObject mStageBGM;

	// Use this for initialization
	void Start () {
		mBeforeTimeScale = Time.timeScale;
		ShowText(Time.timeScale != 0.0f);

		FindObjectOfType<SoundManager>().Play(mStageBGM, 4.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeScale != mBeforeTimeScale) {
			ShowText(Time.timeScale != 0.0f);
		}
		mBeforeTimeScale = Time.timeScale;
	}

	void ShowText(bool aIsPlaying) {
		if(aIsPlaying) {
			mPlayText.SetActive(true);
			mPauseText.SetActive(false);
		}
		else {
			mPlayText.SetActive(false);
			mPauseText.SetActive(true);
		}
	}
}
