using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MassShifter : MonoBehaviour {

	// Use this for initialization
	void Start () {

		mShotLineSourceToDest = Instantiate(mShotLinePrefab, transform);
		mShotLineSourceToDest.name = "ShotLineSourceToDest";

		mCursor = Instantiate(mCursorPrefab, transform);
		mCursor.name = "ShiftCursor";

		mLightBallAlways = Instantiate(mLightBallPrefab, transform);
		mLightBallAlways.name = "LightBallAlways";
		mLightBallAlways.SetActive(false);

		mLightBall = null;

		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeScale == 0.0f) return;

		UpdateState();
	}


	void UpdateState() {

		switch(mState) {
			case CSelectState.cNormal:
				FollowMousePosition();
				UpdateNormal();
				break;
			case CSelectState.cClick:
				FollowMousePosition();
				UpdateClickRightClick();
				break;
			case CSelectState.cDrag:
				FollowMousePosition();
				UpdateDrag();
				break;
			case CSelectState.cMoveSourceToDest:
				FollowMousePosition();
				UpdateMoveSourceToDest();
				break;
			case CSelectState.cMoveFromShare:
				FollowMousePosition();
				UpdateMoveFromShare();
				break;
			case CSelectState.cMoveToShare:
				FollowMousePosition();
				UpdateMoveToShare();
				break;
			case CSelectState.cReturnToShare:
				FollowMousePosition();
				UpdateReturnToShare();
				break;
			case CSelectState.cReturnLightBall:
				FollowMousePosition();
				UpdateReturnLightBall();
				break;
			case CSelectState.cSuccess:
				FollowMousePosition();
				UpdateSuccess();
				break;
			case CSelectState.cFail:
				UpdateFail();
				break;
			case CSelectState.cCantShift:
				UpdateCantShift();
				break;
		}
	}

	void ChangeState(CSelectState aState) {
		mBeforeState = mState;
		mState = aState;
		mInitState = true;
	}

	void UpdateNormal() {

		if (mInitState == true) {
			mInitState = false;
			mShotLineSourceToDest.SetActive(false);
			ShowModelHilight(mSelect, false, Color.white);
			ShowModelHilight(mSource, false, Color.white);
			ShowModelHilight(mDest, false, Color.white);

			mCursor.SetActive(true);
			ChangeCursor(true);

			mSource = null;
			mDest = null;
			mSelect = null;
		}


		mBeforeSelect = mSelect;
		mSelect = GetNearestObject(GetNearWeightObject());

		UpdateModelHilight();

		if (GetShiftButton()) {
			//移す元が選ばれていない場合
			if(mSelect != null) {
				mSource = mSelect;
				ChangeState(CSelectState.cClick);
			}
			else {
				ChangeState(CSelectState.cFail);
			}
		}
	}

	void UpdateDrag() {

		if (mInitState == true) {
			mInitState = false;
			mShotLineSourceToDest.SetActive(true);
			ShowModelHilight(mSource, true, mSourceColor);

			ChangeCursor(false);
		}

		mClickTime += Time.deltaTime;

		mBeforeSelect = mSelect;
		mSelect = GetNearestObject(GetNearWeightObject(), mSource);

		UpdateModelHilight();

		if(mSelect == null) {
			UpdateShotLine(mShotLineSourceToDest, mSource, mCursor);
		}
		else {
			UpdateShotLine(mShotLineSourceToDest, mSource, mSelect);
		}
		

		//左クリックされなくなると
		if (!GetShiftButton()) {

			//移す先が存在しない場合
			if(mSelect == null) {
				ChangeState(CSelectState.cFail);
			}
			//ソースから重さを移せない時
			else if (!CanShiftSource(mSource)) {
				mSource.GetComponent<Animator>().Play("BoxCantShift", 0);
				FindObjectOfType<SoundManager>().Play(mCantShiftSE);
				ChangeState(CSelectState.cFail);
			}
			else {
				mDest = mSelect;
				if(mSource.GetComponent<UnionBlock>()) {
					ChangeState(CSelectState.cMoveFromShare);
				}
				else {
					ChangeState(CSelectState.cMoveSourceToDest);
				}
			}
		}
	}

	void UpdateMoveFromShare() {
		if (mInitState == true) {

			mInitState = false;
			mShotLineSourceToDest.SetActive(false);
			ShowModelHilight(mSelect, false, Color.white);
			ShowModelHilight(mDest, true, mDestColor);

			UnionBlock lSourceUnion = mSource.GetComponent<UnionBlock>();
			mLightBallShare.Clear();

			float lMinDistance = float.MaxValue;

			foreach (var s in lSourceUnion.GetUnionAllListExceptOwn()) {
				GameObject l = Instantiate(mLightBallPrefab, transform);
				l.GetComponent<LightBall>().InitPoint(s.transform.position, mSource.transform.position);
				lMinDistance = Mathf.Min((s.transform.position - mSource.transform.position).magnitude, lMinDistance);
				l.GetComponent<LightBall>().Play();
				mLightBallShare.Add(l);

				if(mSelectDouble == false) {
					s.GetComponent<WeightManager>().SeemWeightLv -= 1;
				}
				else {
					s.GetComponent<WeightManager>().SeemWeightLv -= 2;
				}
			}

			foreach(var l in mLightBallShare) {
				LightBall li = l.GetComponent<LightBall>();
				li.mMoveSpeed = (li.FromPoint - li.ToPoint).magnitude / lMinDistance * mLightBallAlways.GetComponent<LightBall>().mMoveSpeed;
			}

			FindObjectOfType<SoundManager>().Play(mShiftSourceSE);

		}

		var ul = mSource.GetComponent<UnionBlock>().GetUnionAllListExceptOwn();

		bool lAllReach = true;
		for(int i = 0; i < mLightBallShare.Count; i++) {
			LightBall li = mLightBallShare[i].GetComponent<LightBall>();
			UnionBlock u = ul[i].GetComponent<UnionBlock>();
			li.SetPoint(u.transform.position, mSource.transform.position);
			li.UpdatePoint();
			if(li.IsReached == false) {
				lAllReach = false;
			}
		}

		if(lAllReach == true) {
			foreach(var s in mLightBallShare) {
				DestroyLightBall(s);
			}
			mLightBallShare.Clear();
			ChangeState(CSelectState.cMoveSourceToDest);
		}
	}

	void UpdateMoveToShare() {

		if (mInitState == true) {

			mInitState = false;

			UnionBlock lDestUnion = mDest.GetComponent<UnionBlock>();
			mLightBallShare.Clear();

			float lMinDistance = float.MaxValue;

			foreach (var s in lDestUnion.GetUnionAllListExceptOwn()) {

				GameObject l = Instantiate(mLightBallPrefab, transform);
				l.GetComponent<LightBall>().InitPoint(mDest.transform.position, s.transform.position);
				lMinDistance = Mathf.Min((mDest.transform.position - s.transform.position).magnitude, lMinDistance);
				l.GetComponent<LightBall>().Play();
				mLightBallShare.Add(l);
			}

			foreach (var l in mLightBallShare) {
				LightBall li = l.GetComponent<LightBall>();
				li.mMoveSpeed = (li.FromPoint - li.ToPoint).magnitude / lMinDistance * mLightBallAlways.GetComponent<LightBall>().mMoveSpeed;
			}
		}

		var ul = mDest.GetComponent<UnionBlock>().GetUnionAllListExceptOwn();

		bool lAllReach = true;
		for (int i = 0; i < mLightBallShare.Count; i++)
		{
			LightBall li = mLightBallShare[i].GetComponent<LightBall>();
			UnionBlock u = ul[i].GetComponent<UnionBlock>();
			li.SetPoint(mDest.transform.position, u.transform.position);
			li.UpdatePoint();
			if (li.IsReached == false) {
				lAllReach = false;
			}
		}

		if (lAllReach == true) {
			foreach (var s in mLightBallShare) {
				DestroyLightBall(s);
			}
			FindObjectOfType<SoundManager>().Play(mShiftDestSE);
			foreach (var s in mDest.GetComponent<UnionBlock>().GetUnionAllListExceptOwn()) {
				if(mSelectDouble == false)
				{
					s.GetComponent<WeightManager>().SeemWeightLv += 1;
				}
				else
				{
					s.GetComponent<WeightManager>().SeemWeightLv += 2;
				}
				
			}
			mLightBallShare.Clear();
			ChangeState(CSelectState.cSuccess);
		}
	}

	void UpdateReturnToShare() {

		if (mInitState == true)
		{

			mInitState = false;

			UnionBlock lSourceUnion = mSource.GetComponent<UnionBlock>();
			mLightBallShare.Clear();

			float lMinDistance = float.MaxValue;

			foreach (var s in lSourceUnion.GetUnionAllListExceptOwn()) {

				GameObject l = Instantiate(mLightBallPrefab, transform);
				l.GetComponent<LightBall>().InitPoint(mSource.transform.position, s.transform.position);
				lMinDistance = Mathf.Min((mSource.transform.position - s.transform.position).magnitude, lMinDistance);
				l.GetComponent<LightBall>().Play();
				mLightBallShare.Add(l);
			}

			foreach (var l in mLightBallShare) {
				LightBall li = l.GetComponent<LightBall>();
				li.mMoveSpeed = (li.FromPoint - li.ToPoint).magnitude / lMinDistance * mLightBallAlways.GetComponent<LightBall>().mMoveSpeed;
			}
		}

		var ul = mSource.GetComponent<UnionBlock>().GetUnionAllListExceptOwn();

		bool lAllReach = true;
		for (int i = 0; i < mLightBallShare.Count; i++)
		{
			LightBall li = mLightBallShare[i].GetComponent<LightBall>();
			UnionBlock u = ul[i].GetComponent<UnionBlock>();
			li.SetPoint(mSource.transform.position, u.transform.position);
			li.UpdatePoint();
			if (li.IsReached == false) {
				lAllReach = false;
			}
		}

		if (lAllReach == true) {

			foreach (var s in mLightBallShare) {
				DestroyLightBall(s);
			}
			mLightBallShare.Clear();

			FindObjectOfType<SoundManager>().Play(mShiftDestSE);
			foreach (var s in mSource.GetComponent<UnionBlock>().GetUnionAllListExceptOwn()) {
				if (mSelectDouble == false) {
					s.GetComponent<WeightManager>().SeemWeightLv += 1;
				}
				else {
					s.GetComponent<WeightManager>().SeemWeightLv += 2;
				}
			}
			ChangeState(CSelectState.cFail);
		}
	}


	void UpdateMoveSourceToDest()
	{
		if(mInitState == true) {

			mInitState = false;
			mShotLineSourceToDest.SetActive(false);
			ShowModelHilight(mSelect, false, Color.white);
			ShowModelHilight(mDest, true, mDestColor);

			mLightBall = Instantiate(mLightBallPrefab, transform);

			mLightBall.GetComponent<LightBall>().mIgnoreList.Clear();
			mLightBall.GetComponent<LightBall>().mIgnoreList.Add(mSource);
			mLightBall.GetComponent<LightBall>().mIgnoreList.Add(mDest);

			mLightBall.GetComponent<LightBall>().InitPoint(mSource.transform.position, mDest.transform.position);

			mLightBall.GetComponent<LightBall>().Play();

			FindObjectOfType<SoundManager>().Play(mShiftSourceSE);

			if(mSelectDouble == false) {
				mSource.GetComponent<WeightManager>().SeemWeightLv -= 1;
			}
			else {
				mSource.GetComponent<WeightManager>().SeemWeightLv -= 2;
			}
		}

		UpdateShotLine(mShotLineSourceToDest, mSource, mDest);

		var lLightBall = mLightBall.GetComponent<LightBall>();

		lLightBall.SetPoint(mSource.transform.position, mDest.transform.position);
		lLightBall.UpdatePoint();

		if(lLightBall.IsReached) {
			if (!CanShift(mSource, mDest)) {
				//mSource.GetComponent<Animator>().Play("BoxCantShift", 0);
				//FindObjectOfType<SoundManager>().Play(mCantShiftSE);
				ChangeState(CSelectState.cReturnLightBall);
			}
			else {
				if (mDest.GetComponent<UnionBlock>()) {
					ChangeState(CSelectState.cMoveToShare);
				}
				else {
					ChangeState(CSelectState.cSuccess);
				}
				FindObjectOfType<SoundManager>().Play(mShiftDestSE);
			}
		}
		
		if(lLightBall.IsHit) {
			ChangeState(CSelectState.cReturnLightBall);
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

		DestroyLightBall(mLightBall);
		ChangeState(CSelectState.cNormal);
	}

	void UpdateReturnLightBall() {
		if (mInitState == true) {

			mInitState = false;
			ShowModelHilight(mSource, true, mSourceColor);
			ShowModelHilight(mDest, false, mDestColor);

			mLightBall.GetComponent<LightBall>().InitPoint(mLightBall.transform.position, mSource.transform.position);

			mLightBall.GetComponent<LightBall>().Play();

			FindObjectOfType<SoundManager>().Play(mCancelShiftSE);
		}

		mLightBall.GetComponent<LightBall>().SetPoint(mLightBall.GetComponent<LightBall>().FromPoint, mSource.transform.position);
		mLightBall.GetComponent<LightBall>().UpdatePoint();

		if (mLightBall.GetComponent<LightBall>().IsReached) {
			if(mSelectDouble == false) {
				mSource.GetComponent<WeightManager>().SeemWeightLv += 1;
			}
			else {
				mSource.GetComponent<WeightManager>().SeemWeightLv += 2;
			}
			
			DestroyLightBall(mLightBall);
			if (mSource.GetComponent<UnionBlock>()) {
				ChangeState(CSelectState.cReturnToShare);
			}
			else {
				ChangeState(CSelectState.cFail);
			}	
		}
	}

	void UpdateFail() {

		if(mInitState == true) {
			mInitState = false;
			ShowModelHilight(mSelect, false, Color.white);
			ShowModelHilight(mSource, false, Color.white);
			ShowModelHilight(mDest, false, Color.white);
			mShotLineSourceToDest.SetActive(false);
		}

		//押されていないなら
		if (GetShiftButton() == false) {
			ChangeState(CSelectState.cNormal);
		}
	}

	void UpdateCantShift() {

		if (mInitState == true) {

			mInitState = false;
			ShowModelHilight(mSelect, false, Color.white);
			ShowModelHilight(mSource, false, Color.white);
			ShowModelHilight(mDest, false, Color.white);
			mShotLineSourceToDest.SetActive(false);

			mCursor.SetActive(false);
		}

		//外部からmCantShiftにtrueを入れられないと、この状態からは変化しない
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

	void ChangeCursor(bool mIsNormal) {

		GameObject lNormal = mCursor.transform.Find("Model/Normal").gameObject;
		GameObject lSelect = mCursor.transform.Find("Model/Select").gameObject;

		if (mIsNormal)
		{
			lNormal.SetActive(true);
			lSelect.SetActive(false);
		}
		else
		{
			lNormal.SetActive(false);
			lSelect.SetActive(true);
		}
		
	}
	void ChangeCursorColor(Color aColor) {
		mCursor.transform.Find("Model/Select").GetComponentInChildren<Renderer>().material.SetColor("_Color", aColor);
	}

	enum CSelectState {
		cNormal,
		cClick,
		cDrag,
		cMoveSourceToDest,
		cSuccess,
		cMoveFromShare,
		cMoveToShare,
		cReturnToShare,
		cReturnLightBall,
		cFail,
		cCantShift
	}
	[SerializeField]
	CSelectState mState;

	CSelectState mBeforeState;
	bool mInitState = true;

	GameObject mSource;	//うつし元
	GameObject mDest;   //うつし先

	GameObject mBeforeSelect; //現在選択している奴
	GameObject mSelect;	//現在選択している奴

	bool mSelectDouble;
	float mClickTime;


	[SerializeField, DisallowSceneObject]
	GameObject mShotLinePrefab;

	GameObject mShotLineSourceToDest;
	
	[SerializeField, DisallowSceneObject]
	GameObject mCursorPrefab;

	GameObject mCursor;

	[SerializeField, DisallowSceneObject]
	GameObject mLightBallPrefab;

	GameObject mLightBall;
	GameObject mLightBallAlways;
	List<GameObject> mLightBallShare = new List<GameObject>();

	[SerializeField, ColorUsage(false, true, 0f, 8f, 0.125f, 3f)]
	Color mCanSelectColorShotLine;

	[SerializeField, ColorUsage(false, true, 0f, 8f, 0.125f, 3f)]
	Color mCanNotSelectColorShotLine;


	[SerializeField, ColorUsage(false, true, 0f, 8f, 0.125f, 3f)]
	Color mCanSelectColor;

	[SerializeField, ColorUsage(false, true, 0f, 8f, 0.125f, 3f)]
	Color mCanNotSelectColor;

	[SerializeField, ColorUsage(false, true, 0f, 8f, 0.125f, 3f)]
	Color mSourceColor;

	[SerializeField, ColorUsage(false, true, 0f, 8f, 0.125f, 3f)]
	Color mDestColor;

	void UpdateClickRightClick() {

		if(mInitState == true)
		{
			mInitState = false;
			mClickTime = 0.0f;
			mSelectDouble = false;
			mShotLineSourceToDest.SetActive(true);
			ShowModelHilight(mSelect, false, Color.white);
			ShowModelHilight(mSource, true, mSourceColor);

			ChangeCursor(false);

			mBeforeSelect = null;
			mSelect = null;
		}

		UpdateShotLine(mShotLineSourceToDest, mSource, mCursor);
		mClickTime += Time.deltaTime;

		//カーソルがコライダーから外れると
		if (GetNearestObject(GetNearWeightObject()) != mSource) {
			ChangeState(CSelectState.cDrag);
		}

		//右クリックが押されると
		if (GetDoubleShiftButton()) {
			if (mSource.GetComponent<WeightManager>().WeightLv == WeightManager.Weight.heavy) {
				mSelectDouble = true;
			}
		}
		//右クリックが離されると
		else {
			mSelectDouble = false;
		}

		//移すボタンが押されなくなると
		if (!GetShiftButton()) {
			ChangeState(CSelectState.cFail);
		}
	}
	
	bool GetShiftButton() {

		if(IsJoystickConnect()) {
			return Input.GetAxis("JoyShift") >= mShiftOnValue;
		}
		return Input.GetKey(KeyCode.Mouse0);
	}
	bool GetDoubleShiftButton(){
		return Input.GetKey(KeyCode.Mouse1);
	}

	float GetShiftXAxis() {
		if (IsJoystickConnect()) {
			return Input.GetAxis("JoyShiftHorizontal");
		}
		else {
			if (Input.GetKey(KeyCode.J)) return -1.0f;
			if (Input.GetKey(KeyCode.L)) return 1.0f;
		}
		return 0.0f;
	}
	float GetShiftYAxis() {
		if (IsJoystickConnect()) {
			return Input.GetAxis("JoyShiftVertical");
		}
		else {
			if (Input.GetKey(KeyCode.I)) return 1.0f;
			if (Input.GetKey(KeyCode.K)) return -1.0f;
		}
		return 0.0f;
	}

	public static bool IsJoystickConnect() {
		foreach (var j in Input.GetJoystickNames()) {
			if(j != "") {
				return true;
			}
		}
		return false;
	}


	void FollowMousePosition() {

		//ジョイスティックが接続されていたら
		if (IsJoystickConnect()) {
			MoveCursorByJoistick();
			return;
		}


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

	[SerializeField]
	float mCursorMoveXSpeed = 10.0f;

	[SerializeField]
	float mCursorMoveYSpeed = 10.0f;

	[SerializeField]
	float mShiftOnValue = 0.8f;

	void MoveCursorByJoistick() {

		Vector3 lMoveDelta = new Vector3();
		lMoveDelta.x = GetShiftXAxis() * Time.deltaTime * mCursorMoveXSpeed;
		lMoveDelta.y = GetShiftYAxis() * Time.deltaTime * mCursorMoveYSpeed;


		//移動先の座標が画面内に収まっているかの判定
		Vector3 lNewPosition = mCursor.transform.position + lMoveDelta;
	
		Vector3 lNewPositionInScreen = Camera.main.WorldToViewportPoint(lNewPosition);
		if(-1.0f <= lNewPositionInScreen.x && lNewPositionInScreen.x <= 1.0f) {
			if (-1.0f <= lNewPositionInScreen.y && lNewPositionInScreen.y <= 1.0f) {
				mCursor.transform.position = lNewPosition;
			}
		}
	}

	void UpdateShotLine(GameObject aShotLine, GameObject aFrom, GameObject aTo) {

		aShotLine.GetComponent<ShiftShotLine>().SetLinePosition(aFrom.transform.position, aTo.transform.position);

		if (mLightBallAlways.GetComponent<LightBall>().ThroughShotLine(aFrom.transform.position, aTo.transform.position, new GameObject[] { mSource, mDest, mSelect }.ToList())) {
			aShotLine.GetComponent<ShiftShotLine>().SetColor(mCanSelectColorShotLine, true);
			ChangeCursorColor(Color.green);
		}
		else
		{
			aShotLine.GetComponent<ShiftShotLine>().SetColor(mCanNotSelectColorShotLine, false);
			ChangeCursorColor(Color.red);
		}
	}

	void UpdateModelHilight() {
		if(mBeforeSelect != mSelect) {
			if(mBeforeSelect != null) {
				if(mState == CSelectState.cClick || mState == CSelectState.cDrag) {
					ShowModelHilight(mBeforeSelect, false, Color.white);
				}
				else {
					ShowModelHilight(mBeforeSelect, false, Color.white);
				}
				
			}
			if(mSelect != null) {

				/*
				bool lCanSelect = false;

				if(mState == CSelectState.cNormal)
				{
					if(CanShiftSource(mSelect)) {
						lCanSelect = true;
					}
				}
				else {
					if (CanShift(mSource, mSelect)) {
						lCanSelect = true;
					}
				}

				if (lCanSelect) {
					ShowModelHilight(mSelect, true, mCanSelectColor);
				}
				else {
					ShowModelHilight(mSelect, true, mCanNotSelectColor);
				}
				*/

				ShowModelHilight(mSelect, true, mCanSelectColor);
			}
		}
	}

	void ShowModelHilight(GameObject aModel, bool aIsShow, Color aColor) {

		if (aModel == null) return;

		Transform lFrame = aModel.transform.Find("Model/Frame");
		
		if (aIsShow == false) {
			lFrame.gameObject.SetActive(false);
		}
		else
		{
			lFrame.gameObject.SetActive(true);
			foreach (var r in lFrame.GetComponentsInChildren<Renderer>()) {
				Material[] lMat = r.materials;
				foreach (var m in lMat)
				{
					m.SetColor("_Color", aColor);
				}
				r.materials = lMat;
			}
		}
	}

	bool CanShiftSource(GameObject aGameObject) {
		if (aGameObject != null) {
			if (aGameObject.GetComponent<WeightManager>().WeightLv != WeightManager.Weight.flying) {
				return true;
			}
		}
		return false;
	}

	bool CanShift(GameObject aSource, GameObject aDest) {

		if (aDest != null) {

			UnionBlock lSourceUnion = aSource.GetComponent<UnionBlock>();
			UnionBlock lDestUnion = aDest.GetComponent<UnionBlock>();

			//両方とも共有ブロックで
			if (lSourceUnion != null && lDestUnion != null) {
				//同じ共有グループなら
				if (lSourceUnion.IsSameUnionGroup(lDestUnion)) {
					return false;
				}
			}

			if (aDest.GetComponent<WeightManager>().WeightLv == WeightManager.Weight.heavy) {
				return false;
			}
		}
		else
		{
			return false;
		}

		return true;
	}

	[SerializeField]
	GameObject mCantShiftSE;

	[SerializeField]
	GameObject mCancelShiftSE;

	[SerializeField]
	GameObject mShiftSourceSE;

	[SerializeField]
	GameObject mShiftDestSE;

	bool _mCanShift = true;

	public bool mCanShift {
		get {
			return _mCanShift;
		}
		set {
			if(value == true) {
				if(mState == CSelectState.cCantShift) {
					ChangeState(CSelectState.cNormal);
				}
			}
			else {
				ChangeState(CSelectState.cCantShift);
			}
			_mCanShift = value;
		}
	}

	void DestroyLightBall(GameObject g) {
		g.GetComponent<LightBall>().Stop();
		Destroy(g, 1.0f);
	}
}
