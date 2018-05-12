using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MassShifter : MonoBehaviour {

	// Use this for initialization
	void Start () {

		mShotLineSourceToPlayer = Instantiate(mShotLinePrefab, transform);
		mShotLineSourceToPlayer.name = "ShotLineSourceToPlayer";
		mShotLinePlayerToDest = Instantiate(mShotLinePrefab, transform);
		mShotLinePlayerToDest.name = "ShotLinePlayerToDest";

		mCursor = Instantiate(mCursorPrefab, transform);
		mCursor.name = "ShiftCursor";

		mLightBall = Instantiate(mLightBallPrefab, transform);
		mLightBall.name = "LightBall";

		OnSelectChange += MyOnSelectChange;
	}
	
	// Update is called once per frame
	void Update () {
		FollowMousePosition();
		UpdateState();
	}


	void UpdateState()
	{
		switch(mState) {
			case CSelectState.cNormal:
				UpdateNormal();
				break;
			case CSelectState.cClick:
				UpdateClickRightClick();
				break;
			case CSelectState.cDrag:
				UpdateDrag();
				break;
			case CSelectState.cMoveSourceToPlayer:
				UpdateMoveSourceToPlayer();
				break;
			case CSelectState.cMovePlayerToDest:
				UpdateMovePlayerToDest();
				break;
			case CSelectState.cSuccess:
				UpdateSuccess();
				break;
			case CSelectState.cFail:
				UpdateFail();
				break;
		}
		
	}

	void ChangeState(CSelectState aState) {
		mBeforeState = mState;
		mState = aState;
		mInitState = true;
	}


	void MyOnSelectChange(GameObject aSelectObject) {
		if(aSelectObject != null) {
			
		}
		else {
			
		}
	}

	void UpdateNormal()
	{
		if (mInitState == true) {
			mInitState = false;
			mShotLineSourceToPlayer.SetActive(true);
			mShotLinePlayerToDest.SetActive(false);
			mLightBall.SetActive(false);
		}


		mSelect = GetNearestObject(GetNearWeightObject());

		//tの選択表示処理
		OnSelectChange(mSelect);

		UpdateShotLine(mShotLineSourceToPlayer, mCursor, GetPlayer());
		

		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			if(mSelect != null) {
				if(mSelect.GetComponent<WeightManager>().WeightLv != WeightManager.Weight.flying) {
					ChangeState(CSelectState.cClick);
					mSource = mSelect;
				}
				else {
					ChangeState(CSelectState.cFail);
				}
			}
			else {
				ChangeState(CSelectState.cFail);
			}
		}
	}

	void UpdateDrag()
	{
		if (mInitState == true)
		{
			mInitState = false;
			mShotLineSourceToPlayer.SetActive(true);
			mShotLinePlayerToDest.SetActive(true);
			mLightBall.SetActive(false);
		}

		mSelect = GetNearestObject(GetNearWeightObject(), mSource);

		//tの選択表示処理
		OnSelectChange(mSelect);

		UpdateShotLine(mShotLineSourceToPlayer, mSource, GetPlayer());
		UpdateShotLine(mShotLinePlayerToDest, GetPlayer(), mCursor);

		//左クリックされなくなると
		if (!Input.GetKey(KeyCode.Mouse0)) {
			if(mSelect != null) {
				mDest = mSelect;

				UnionBlock lSourceUnion = mSource.GetComponent<UnionBlock>();
				UnionBlock lDestUnion = mDest.GetComponent<UnionBlock>();

				//両方とも共有ブロックで
				if (lSourceUnion != null && lDestUnion != null) {
					//同じ共有グループなら
					if (lSourceUnion.IsSameUnionGroup(lDestUnion)) {
						//失敗
						ChangeState(CSelectState.cFail);
						return;
					}
				}

				if(mDest.GetComponent<WeightManager>().WeightLv == WeightManager.Weight.heavy) {
					//これ以上移せないので失敗
					ChangeState(CSelectState.cFail);
					return;
				}
				ChangeState(CSelectState.cMoveSourceToPlayer);
			}
			else {
				ChangeState(CSelectState.cFail);
			}
		}
	}

	void UpdateMoveSourceToPlayer()
	{
		if(mInitState == true) {
			mInitState = false;
			mShotLineSourceToPlayer.SetActive(true);
			mShotLinePlayerToDest.SetActive(true);
			mLightBall.SetActive(true);
			mLightBall.GetComponent<LightBall>().SetPoint(mSource.transform.position, GetPlayer().transform.position);
		}


		UpdateShotLine(mShotLineSourceToPlayer, mSource, GetPlayer());
		UpdateShotLine(mShotLinePlayerToDest, GetPlayer(), mDest);

		var lLightBall = mLightBall.GetComponent<LightBall>();

		lLightBall.mIgnoreList.Clear();
		lLightBall.mIgnoreList.Add(mSource);

		lLightBall.UpdatePoint(mSource.transform.position, GetPlayer().transform.position);

		if(lLightBall.IsReached) {
			ChangeState(CSelectState.cMovePlayerToDest);
		}
		
		if(lLightBall.IsHit) {
			ChangeState(CSelectState.cFail);
		}
	}

	void UpdateMovePlayerToDest()
	{
		if (mInitState == true)
		{
			mInitState = false;
			mShotLineSourceToPlayer.SetActive(true);
			mShotLinePlayerToDest.SetActive(true);
			mLightBall.SetActive(true);
			mLightBall.GetComponent<LightBall>().SetPoint(GetPlayer().transform.position, mDest.transform.position);
		}

		UpdateShotLine(mShotLineSourceToPlayer, mSource, GetPlayer());
		UpdateShotLine(mShotLinePlayerToDest, GetPlayer(), mDest);

		var lLightBall = mLightBall.GetComponent<LightBall>();

		lLightBall.mIgnoreList.Clear();
		lLightBall.mIgnoreList.Add(mDest);

		lLightBall.UpdatePoint(GetPlayer().transform.position, mDest.transform.position);

		if (lLightBall.IsReached) {
			ChangeState(CSelectState.cSuccess);
		}

		if (lLightBall.IsHit) {
			ChangeState(CSelectState.cFail);
		}
	}

	void UpdateSuccess()
	{
		UnionBlock lSourceUnion = mSource.GetComponent<UnionBlock>();
		UnionBlock lDestUnion = mDest.GetComponent<UnionBlock>();

		int lMoveMassNum = 1;
		if(mSelectDouble) {
			lMoveMassNum = 2;
		}
		mSource.GetComponent<WeightManager>().PushWeight(mDest.GetComponent<WeightManager>(), lMoveMassNum);

		//もし移し元が共有ブロックなら
		if(lSourceUnion != null) {
			foreach(var a in lSourceUnion.GetUnionList()) {
				a.GetComponent<WeightManager>().WeightLv = mSource.GetComponent<WeightManager>().WeightLv;
			}
		}

		//もし移し先が共有ブロックなら
		if (lDestUnion != null) {
			foreach (var a in lDestUnion.GetUnionList()) {
				a.GetComponent<WeightManager>().WeightLv = mDest.GetComponent<WeightManager>().WeightLv;
			}
		}

		mSource = null;
		mDest = null;

		ChangeState(CSelectState.cNormal);
	}

	void UpdateFail() {

		if(mInitState == true) {
			mInitState = false;
			mShotLineSourceToPlayer.SetActive(false);
			mShotLinePlayerToDest.SetActive(false);
			mLightBall.SetActive(false);
		}

		//押されていないなら
		if (Input.GetKey(KeyCode.Mouse0) == false) {
			ChangeState(CSelectState.cNormal);
		}
	}

	GameObject[] GetNearWeightObject() {
		Transform t = mCursor.transform.Find("Collider");
		LayerMask l = LayerMask.GetMask(new string[] { "SelectArea" });
		return Physics.OverlapBox(t.position, t.localScale / 2, t.rotation, l).Select(x => x.transform.parent.gameObject).ToArray();
	}
	GameObject GetNearestObject(GameObject[] aGameObject) {
		return GetNearestObject(aGameObject, null);
	}
	GameObject GetNearestObject(GameObject[] aGameObject, GameObject aRemove) {
		float lMinDistQuad = float.MaxValue;
		GameObject lRet = null;
		foreach(var t in aGameObject) {
			if (t == aRemove) continue;
			float lDistQuad = (mCursor.transform.position - t.transform.position).sqrMagnitude;
			if (lDistQuad <= lMinDistQuad) {
				lRet = t;
				lMinDistQuad = lDistQuad;
			}
		}
		return lRet;
	}

	enum CSelectState {
		cNormal,
		cClick,
		cDrag,
		cMoveSourceToPlayer,
		cMovePlayerToDest,
		cSuccess,
		cFail,
	}
	[SerializeField]
	CSelectState mState;

	CSelectState mBeforeState;
	bool mInitState = true;

	GameObject mSource;	//うつし元
	GameObject mDest;   //うつし先

	GameObject mSelect;	//現在選択している奴

	bool mSelectDouble;
	float mClickTime;


	[SerializeField, DisallowSceneObject]
	GameObject mShotLinePrefab;

	GameObject mShotLineSourceToPlayer;
	GameObject mShotLinePlayerToDest;
	
	[SerializeField, DisallowSceneObject]
	GameObject mCursorPrefab;

	GameObject mCursor;

	[SerializeField, DisallowSceneObject]
	GameObject mLightBallPrefab;

	GameObject mLightBall;


	public delegate void OnSelectChangeEvent(GameObject aSelectObject);

	[SerializeField]
	public event OnSelectChangeEvent OnSelectChange;


	void UpdateClickRightClick() {

		if(mInitState == true)
		{
			mInitState = false;
			mClickTime = 0.0f;
			mSelectDouble = false;
			mShotLineSourceToPlayer.SetActive(true);
			mShotLinePlayerToDest.SetActive(true);
			mLightBall.SetActive(false);
		}

		mSelect = GetNearestObject(GetNearWeightObject(), mSource);

		UpdateShotLine(mShotLineSourceToPlayer, mSource, GetPlayer());
		UpdateShotLine(mShotLinePlayerToDest, GetPlayer(), mCursor);

		//カーソルがコライダーから外れると
		if (!GetNearWeightObject().Contains(mSource)) {
			ChangeState(CSelectState.cDrag);
		}

		//右クリックが押されると
		if (Input.GetKey(KeyCode.Mouse1)) {
			if (mSource.GetComponent<WeightManager>().WeightLv == WeightManager.Weight.heavy) {
				mSelectDouble = true;
			}
		}
		//右クリックが離されると
		else {
			mSelectDouble = false;
		}

		//左クリックがされなくなると
		if (!Input.GetKey(KeyCode.Mouse0))
		{
			ChangeState(CSelectState.cFail);
		}
	}
	

	void FollowMousePosition() {
		//		targetPoint.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0.0f, 0.0f, -Camera.main.transform.position.z));	
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//		Debug.DrawRay(ray.origin, ray.direction);
		//		RaycastHit hitInfo;
		//		Physics.Raycast(ray, out hitInfo);
		//		targetPoint.position = hitInfo.point;
		Plane plane = new Plane(new Vector3(0.0f, 0.0f, -1.0f), 0.0f);
		float enter = 0.0f;
		if (plane.Raycast(ray, out enter)) {
			mCursor.transform.position = ray.GetPoint(enter);
		}
	}

	void UpdateShotLine(GameObject aShotLine, GameObject aFrom, GameObject aTo) {

		aShotLine.GetComponent<ShiftShotLine>().SetLinePosition(aFrom.transform.position, aTo.transform.position);

		if (mLightBall.GetComponent<LightBall>().ThroughShotLine(aFrom.transform.position, aTo.transform.position, new GameObject[] { mSource, mDest, mSelect }.ToList()))
		{
			aShotLine.GetComponent<ShiftShotLine>().SetColor(Color.green);
		}
		else
		{
			aShotLine.GetComponent<ShiftShotLine>().SetColor(Color.red);
		}
	}

	GameObject GetPlayer() {
		return FindObjectOfType<Player>().gameObject;
	}
}
