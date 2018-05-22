using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPause : MonoBehaviour {

	[SerializeField]
	GameObject mPauseText;

	[SerializeField]
	GameObject mPlayText;

	float mBeforeTimeScale;

	[SerializeField]
	GameObject mStageBGM;

	// Use this for initialization
	void Start () {
		mBeforeTimeScale = Time.timeScale;
		ShowText(Time.timeScale != 0.0f);

		//FindObjectOfType<SoundManager>().Play(mStageBGM, 4.0f);
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
			mPauseText.SetActive(false);
			mPlayText.SetActive(true);
			mPlayText.GetComponent<TextMesh>().text = "×" + Time.timeScale;
		}
		else {
			mPauseText.SetActive(true);
			mPlayText.SetActive(false);
		}
	}
}
