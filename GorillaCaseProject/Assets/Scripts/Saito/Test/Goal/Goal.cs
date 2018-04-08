using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	// Use this for initialization
	void Start () {
		mOpenRate = 0.0f;

		mDoorMoveStart = transform.Find("Door").localPosition;
		mDoorMoveEnd = mDoorMoveStart + new Vector3(0.0f, 3.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {

		//扉の開いている状態をリセット
		mIsOpen = false;

		//もし全てのボタンがオンなら
		if(IsAllButtonOn() == true) {
			mIsOpen = true;
		}


		//デバッグ用
		/*
		if(mDebug_) {
			mIsOpen = mDebug_Open;
		}
		//*/


		//扉の開き具合を更新する
		UpdateOpenRate();


		//扉の移動部分を移動させる
		MoveDoor();


		//デバッグ表示の更新
		DrawDebug();
	}



	//扉の開き具合を更新する
	void UpdateOpenRate() {
		//もし空いているなら
		if(mIsOpen == true) {
			mOpenRate += Time.deltaTime * (1.0f / mOpenTakeTime);
		}
		else {
			mOpenRate -= Time.deltaTime * (1.0f / mCloseTakeTime);
		}

		mOpenRate = Mathf.Clamp01(mOpenRate);	//開き具合を0.0f~1.0fの間に収める
	}


	//扉の移動部分を移動させる
	void MoveDoor() {
		transform.Find("Door").transform.localPosition = Vector3.Lerp(mDoorMoveStart, mDoorMoveEnd, mOpenRate);
	}


	//デバッグ表示
	void DrawDebug() {

		GameObject l = transform.Find("Debug/ButtonOnText").gameObject;
		l.GetComponent<TextMesh>().text = TotalButtonOn().ToString() + "/" + TotalButton().ToString();

		if (IsGoalOpen()) {
			l.GetComponent<TextMesh>().color = Color.blue;
		}
		else {
			l.GetComponent<TextMesh>().color = Color.white;
		}
	}


	//ボタンが全てオンかどうか
	bool IsAllButtonOn() {

		if(TotalButtonOn() == TotalButton()){
			return true;
		}
		return false;
	}


	//オンになっているボタンの数を取得する
	int TotalButtonOn() {

		int lTotal = 0;

		foreach (var g in mButtonList) {
			if (g == null) {
				Debug.LogWarning("Goal's Button is null");
				return -1;
			}
			if (g.GetComponent<Button>() == null) {
				Debug.LogWarning("Goal's Button is not button");
				return -1;
			}
			if (g.GetComponent<Button>().IsButtonOn() == true) {
				lTotal += 1;
			}
		}

		return lTotal;
	}

	//オンする必要のあるボタンの数を取得する
	int TotalButton() {
		return mButtonList.Count;
	}


	//ゴールが完全に開いているかどうか
	public bool IsGoalOpen() {
		if (mOpenRate >= 1.0f) return true;
		return false;
	}



	[SerializeField]
	List<GameObject> mButtonList;   //ゴールを有効にするのに押す必要のある、ボタンのリスト

	[SerializeField]
	float mOpenTakeTime;    //扉が開くのにかかる時間

	[SerializeField]
	float mCloseTakeTime;    //扉が閉まるのにかかる時間

	float mOpenRate;    //扉の現在開いている割合
	bool mIsOpen;   //扉が現在開いているか

	Vector3 mDoorMoveStart;	//ドアが動く開始位置
	Vector3 mDoorMoveEnd;   //ドアが動く終了位置


	//デバッグ用
	/*
	[SerializeField]
	bool mDebug_;
	[SerializeField]
	bool mDebug_Open;
	//*/
}
