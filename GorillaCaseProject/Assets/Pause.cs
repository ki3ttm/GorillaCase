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
            PauseFunc();
        }
    }

    public void PauseFunc() {
        // ポーズ
        if (Time.timeScale != 0.0f) {
            Time.timeScale = 0.0f;
        }
        // ポーズ解除
        else {
            Time.timeScale = 1.0f;
        }

        // 登録された関数を実行
        pauseEvent.Invoke();
    }
}
