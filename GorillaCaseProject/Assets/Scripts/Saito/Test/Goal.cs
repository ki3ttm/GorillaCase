using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ReorderableList<type>
{
	public List<type> list_;
}

[System.Serializable]
public class GameObjectReorderableList : ReorderableList<GameObject>
{
}

public class Goal : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		//扉が開くかどうかの確認
		CheckOpen();

		CheckPlayerInGoal();
	}

	void CheckOpen() {

		

		//扉が現在開いているかどうか
		bool lBeforeIsButtonOn = mIsButtonOn;

		mIsButtonOn = IsAllButtonOn();

		//デバッグ用
		///*
		if (mDebug_) {
			mIsButtonOn = mDebug_Open;
		}
		//*/


		if (mIsButtonOn) {
			mOpenRate += 1.0f / mOpenTakeTime * Time.deltaTime;
		}
		else {
			mOpenRate -= 1.0f / mCloseTakeTime * Time.deltaTime;
		}
		mOpenRate = Mathf.Clamp01(mOpenRate);


		if(!lBeforeIsButtonOn) {
			if(mIsButtonOn) {
				//開く瞬間の処理
				mGoalModel.GetComponent<Animator>().Play("Open", 0, mOpenRate);
				mGoalModel.GetComponent<Animator>().SetFloat("Speed", 1.0f / mOpenTakeTime * mGoalModel.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
			}
		}

		if (lBeforeIsButtonOn) {
			if (!mIsButtonOn) {
				//閉じる瞬間の処理
				mGoalModel.GetComponent<Animator>().Play("Open", 0, mOpenRate);
				mGoalModel.GetComponent<Animator>().SetFloat("Speed", -1.0f / mCloseTakeTime * mGoalModel.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
			}
		}

		if(mOpenRate >= 1.0f) {
			mIsOpen = true;
		}
		else {
			mIsOpen = false;
		}


		

		//デバッグ表示の更新
		DrawDebug();
	}

	void CheckPlayerInGoal() {
		mPlayerInGoal = IsCollisionComplete(mGoalTrigger.GetComponent<BoxCollider>(), FindObjectOfType<Player>().GetComponent<Collider>());
	}

	//デバッグ表示
	void DrawDebug() {

		if(mDebugObject) {
			GameObject l = mDebugObject.transform.Find("ButtonOnText").gameObject;
			l.GetComponent<TextMesh>().text = TotalButtonOn().ToString() + "/" + TotalButton().ToString();

			if (IsGoalOpen())
			{
				l.GetComponent<TextMesh>().color = Color.blue;
			}
			else
			{
				l.GetComponent<TextMesh>().color = Color.white;
			}
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
		return mIsOpen;
	}


	[SerializeField]
	List<GameObject> mButtonList;   //ゴールを有効にするのに押す必要のある、ボタンのリスト

	[SerializeField, ListAttribute]
	//List<GameObject> mButtonListsss;
	GameObjectReorderableList mButtonListsss;


	bool mIsButtonOn = false;   //全てのボタンが押されているか
	bool mIsOpen = false;   //ゴールが完全に開いているか

	float mAnimationClipLength;

	float mOpenRate = 0.0f;
	[SerializeField, Tooltip("扉が開くのに何秒かかるか")]
	float mOpenTakeTime = 1.0f;
	[SerializeField, Tooltip("扉が閉まるのに何秒かかるか")]
	float mCloseTakeTime = 1.0f;

	[SerializeField, SaitoTest_Disable]
	bool mPlayerInGoal;


	[SerializeField]
	GameObject mGoalModel;

	[SerializeField]
	GameObject mGoalTrigger;

	[SerializeField]
	GameObject mDebugObject;

	//デバッグ用
	///*
	[SerializeField]
	bool mDebug_;
	[SerializeField]
	bool mDebug_Open;
	//*/


	static bool IsCollisionComplete(BoxCollider aArea, Collider aObject)
	{

		Vector3 dir;
		float dis;

		bool res = Physics.ComputePenetration(aArea, aArea.bounds.center, aArea.transform.rotation, aObject, aObject.transform.position, aObject.transform.rotation, out dir, out dis);
		if (res == false) return false;

		res = Physics.ComputePenetration(aArea, GetPosition(aArea, Vector3.up), aArea.transform.rotation, aObject, aObject.transform.position, aObject.transform.rotation, out dir, out dis);
		if (res == true) return false;

		res = Physics.ComputePenetration(aArea, GetPosition(aArea, Vector3.down), aArea.transform.rotation, aObject, aObject.transform.position, aObject.transform.rotation, out dir, out dis);
		if (res == true) return false;

		res = Physics.ComputePenetration(aArea, GetPosition(aArea, Vector3.right), aArea.transform.rotation, aObject, aObject.transform.position, aObject.transform.rotation, out dir, out dis);
		if (res == true) return false;

		res = Physics.ComputePenetration(aArea, GetPosition(aArea, Vector3.left), aArea.transform.rotation, aObject, aObject.transform.position, aObject.transform.rotation, out dir, out dis);
		if (res == true) return false;

		res = Physics.ComputePenetration(aArea, GetPosition(aArea, Vector3.forward), aArea.transform.rotation, aObject, aObject.transform.position, aObject.transform.rotation, out dir, out dis);
		//if (res == true) return false;

		res = Physics.ComputePenetration(aArea, GetPosition(aArea, Vector3.back), aArea.transform.rotation, aObject, aObject.transform.position, aObject.transform.rotation, out dir, out dis);
		//if (res == true) return false;

		return true;
	}

	static Vector3 GetPosition(BoxCollider aCollider, Vector3 aOffset)
	{
		Vector3 aPositionOffset = aCollider.transform.rotation * new Vector3(aCollider.bounds.size.x * aOffset.x, aCollider.bounds.size.y * aOffset.y, aCollider.bounds.size.z * aOffset.z);
		return aCollider.transform.position + aPositionOffset;
	}

}
