using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pause : MonoBehaviour {
    // ポーズイベント
    public UnityEvent pauseEvent = new UnityEvent();

    void Update() {
        // Escキーでポーズ / ポーズ解除
        if (Input.GetButtonDown("Pause")) {
            //PauseFunc();
        }
    }
	
    public void PauseFunc() {
		Speed(0.0f);
    }
	public void Play() {
		Speed(1.0f);
	}


	public void Speed(float aSpeed) {
		Time.timeScale = aSpeed;

		// 登録された関数を実行
		pauseEvent.Invoke();
	}

	public float Speed() {
		return Time.timeScale;
	}
}
