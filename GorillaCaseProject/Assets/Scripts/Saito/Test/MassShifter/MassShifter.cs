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

		mLightBall = Instantiate(mLightBallPrefab, transform);
		mLightBall.name = "LightBall";

		Cursor.visible = false;
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
			case CSelectState.cMoveSourceToDest:
				UpdateMoveSourceToDest();
				break;
			case CSelectState.cReturnLightBall:
				UpdateReturnLightBall();
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

	void UpdateNormal()
	{
		if (mInitState == true) {
			mInitState = false;
			mShotLineSourceToDest.SetActive(false);
			mLightBall.SetActive(false);
			ShowModelHilight(mSelect, false, Color.white);
			ShowModelHilight(mSource, false, Color.white);
			ShowModelHilight(mDest, false, Color.white);

			ChangeCursor(true);

			mSource = null;
			mDest = null;
			mSelect = null;
		}


		mBeforeSelect = mSelect;
		mSelect = GetNearestObject(GetNearWeightObject());

		UpdateModelHilight();

		if (Input.GetKeyDown(KeyCode.Mouse0)) {

			if(CanShiftSource(mSelect))
			{
				mSource = mSelect;
				ChangeState(CSelectState.cClick);
			}
			else
			{
				if(mSelect) {
					mSelect.GetComponent<Animator>().Play("BoxCantShift", 0);
					GetComponent<AudioSource>().PlayOneShot(mCantShiftSE);
				}
				ChangeState(CSelectState.cFail);
			}
		}
	}

	void UpdateDrag()
	{
		if (mInitState == true)
		{
			mInitState = false;
			mShotLineSourceToDest.SetActive(true);
			mLightBall.SetActive(false);
			ShowModelHilight(mSource, true, mSourceColor);

			ChangeCursor(false);
		}

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
		if (!Input.GetKey(KeyCode.Mouse0)) {
			if (CanShift(mSource, mSelect)) {
				mDest = mSelect;
				ChangeState(CSelectState.cMoveSourceToDest);
			}
			else {
				if (mSelect)
				{
					mSelect.GetComponent<Animator>().Play("BoxCantShift", 0);
					GetComponent<AudioSource>().PlayOneShot(mCantShiftSE);
				}
				ChangeState(CSelectState.cFail);
			}
		}
	}

	void UpdateMoveSourceToDest()
	{
		if(mInitState == true) {
			mInitState = false;
			mShotLineSourceToDest.SetActive(true);
			ShowModelHilight(mSelect, false, Color.white);
			ShowModelHilight(mDest, true, mDestColor);
			mLightBall.SetActive(true);

			mLightBall.GetComponent<LightBall>().mIgnoreList.Clear();
			mLightBall.GetComponent<LightBall>().mIgnoreList.Add(mSource);
			mLightBall.GetComponent<LightBall>().mIgnoreList.Add(mDest);

			mLightBall.GetComponent<LightBall>().InitPoint(mSource.transform.position, mDest.transform.position);

			GetComponent<AudioSource>().PlayOneShot(mShiftSourceSE);

			mSource.GetComponent<WeightManager>().SeemWeightLv -= 1;
		}

		UpdateShotLine(mShotLineSourceToDest, mSource, mDest);

		var lLightBall = mLightBall.GetComponent<LightBall>();

		lLightBall.SetPoint(mSource.transform.position, mDest.transform.position);
		lLightBall.UpdatePoint();

		if(lLightBall.IsReached) {
			ChangeState(CSelectState.cSuccess);
			GetComponent<AudioSource>().PlayOneShot(mShiftDestSE);
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

		ChangeState(CSelectState.cNormal);
	}

	void UpdateReturnLightBall() {
		if (mInitState == true) {

			mInitState = false;
			ShowModelHilight(mSource, true, mSourceColor);
			ShowModelHilight(mDest, false, mDestColor);
			mLightBall.SetActive(true);

			mLightBall.GetComponent<LightBall>().InitPoint(mLightBall.transform.position, mSource.transform.position);

			GetComponent<AudioSource>().PlayOneShot(mCancelShiftSE);
		}

		mLightBall.GetComponent<LightBall>().SetPoint(mLightBall.GetComponent<LightBall>().FromPoint, mSource.transform.position);
		mLightBall.GetComponent<LightBall>().UpdatePoint();

		if (mLightBall.GetComponent<LightBall>().IsReached) {
			mSource.GetComponent<WeightManager>().SeemWeightLv += 1;
			ChangeState(CSelectState.cFail);
		}

		
	}

	void UpdateFail() {

		if(mInitState == true) {
			mInitState = false;
			ShowModelHilight(mSelect, false, Color.white);
			ShowModelHilight(mSource, false, Color.white);
			ShowModelHilight(mDest, false, Color.white);
			mShotLineSourceToDest.SetActive(false);
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
		cReturnLightBall,
		cFail,
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
			mLightBall.SetActive(false);
			ShowModelHilight(mSelect, false, Color.white);
			ShowModelHilight(mSource, true, mSourceColor);

			ChangeCursor(false);

			mBeforeSelect = null;
			mSelect = null;
		}

		UpdateShotLine(mShotLineSourceToDest, mSource, mCursor);

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
			ChangeCursorColor(Color.green);
		}
		else
		{
			aShotLine.GetComponent<ShiftShotLine>().SetColor(Color.red);
			ChangeCursorColor(Color.red);
		}
	}

	void UpdateModelHilight() {
		if(mBeforeSelect != mSelect) {
			if(mBeforeSelect != null) {
				ShowModelHilight(mBeforeSelect, false, Color.white);
			}
			if(mSelect != null) {
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
			if (aGameObject.GetComponent<WeightManager>().WeightLv != WeightManager.Weight.flying)
			{
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
			if (lSourceUnion != null && lDestUnion != null)
			{
				//同じ共有グループなら
				if (lSourceUnion.IsSameUnionGroup(lDestUnion))
				{
					return false;
				}
			}

			if (aDest.GetComponent<WeightManager>().WeightLv == WeightManager.Weight.heavy)
			{
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
	AudioClip mCantShiftSE;

	[SerializeField]
	AudioClip mCancelShiftSE;

	[SerializeField]
	AudioClip mShiftSourceSE;

	[SerializeField]
	AudioClip mShiftDestSE;
}
