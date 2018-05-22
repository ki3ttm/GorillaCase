using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRule : MonoBehaviour {

	MassShifter mMassShifter;
	Player mPlayer;
	Pause mPause;
	Goal mGoal;

	[SerializeField]
	TextMesh mStateText;

	[SerializeField]
	List<string> mSceneList;

	// Use this for initialization
	void Start () {
		mMassShifter = FindObjectOfType<MassShifter>();
		mPlayer = FindObjectOfType<Player>();
		mPause = FindObjectOfType<Pause>();
		mGoal = FindObjectOfType<Goal>();

		StartCoroutine(GameMain());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator GameMain() {

		float lTakeTime;

		//ステージ開始時の演出
		mPlayer.JumpFlg = false;
		mPlayer.MoveFlg = false;
		mMassShifter.mCanShift = false;

		mStateText.gameObject.SetActive(true);
		mStateText.text = "Start";

		lTakeTime = 0.0f;

		while(true) {
			lTakeTime += Time.deltaTime;
			if(lTakeTime >= 2.0f) {
				break;
			}
			yield return null;
		}


		//操作可能になる

		mPlayer.JumpFlg = true;
		mPlayer.MoveFlg = true;
		mMassShifter.mCanShift = true;

		mStateText.gameObject.SetActive(false);

		//ゲームメイン

		while(true) {

			//マウスホイールの回転によって、ゲームの速度を変更する
			var lMouseWheelRotate = Input.GetAxis("Mouse ScrollWheel");
			if(lMouseWheelRotate > 0.0f) {
				lMouseWheelRotate = 1.0f;
			}
			if (lMouseWheelRotate < 0.0f) {
				lMouseWheelRotate = -1.0f;
			}

			//マウスホイールが押されたら、ゲームの速度を等速に戻す
			if(Input.GetMouseButtonDown(2)) {
				mPause.Speed(1.0f);
			}
			else {
				mPause.Speed(Mathf.Max(mPause.Speed() + 1.0f * Time.unscaledDeltaTime * lMouseWheelRotate * 5.0f, 0.0f));
			}

			//ゴール判定
			if (mGoal.IsAllButtonOn) {
				if(mGoal.IsInPlayer(mPlayer)) {
					break;
				}
			}

			yield return null;
		}

		//ゴール時の演出
		mPlayer.JumpFlg = false;
		mPlayer.MoveFlg = false;
		mMassShifter.mCanShift = false;

		mStateText.gameObject.SetActive(true);
		mStateText.text = "Goal";

		yield return new WaitForSeconds(2.0f);

		//次のシーンへ移動
		int lSceneIndex = mSceneList.IndexOf(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
		lSceneIndex++;
		if(lSceneIndex >= mSceneList.Count) {
			lSceneIndex = 0;
		}

		UnityEngine.SceneManagement.SceneManager.LoadScene(mSceneList[lSceneIndex]);

		yield return null;
	}
}
