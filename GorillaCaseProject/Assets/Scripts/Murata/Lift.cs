using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour {
    [SerializeField] float maxDis = 2.0f;
	[SerializeField] Transform liftPoint = null;
    [SerializeField] GameObject liftObj = null;

    // test用
    [SerializeField] float LR = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Debug.DrawRay(transform.position, transform.forward * maxDis);

        //  Debug.Log("LiftUpdate");

        // 左右
        LR = Vector3.Dot(transform.forward, Vector3.right);

        // 持ち上げ下げ
        LiftUpDown();

        // 持ち続け
        Lifting();
	}
    void LiftUpDown()
    {
        if (Input.GetButtonDown("Lift")) {
//            Debug.Log("LSDOWN");

            if (liftObj == null)
            {
                LiftUp();
            }
            else
            {
                LiftDown();
            }
        }
    }

    GameObject LiftUp()
    {
//        Debug.Log("LU");
        RaycastHit hitInfo;
		Physics.Raycast(transform.position, transform.forward, out hitInfo, maxDis, LayerMask.GetMask(new string[] { "Box" }));

        // ブロックがある
        if (hitInfo.collider != null)
        {
			WeightManager boxWeight = hitInfo.collider.GetComponent<WeightManager>();
			if (boxWeight == null)
			{
				return null;
			}

            // 自分より重くて持てなかった
			if (boxWeight.WeightLv > GetComponent<WeightManager>().WeightLv)
			{
				// 持ち上げようとする
				Debug.Log("もてへん");
				return null;
			}

            liftObj = hitInfo.collider.gameObject;
            // 持ち上げた箱の当たり判定を消す
            BoxCollider col = liftObj.GetComponent<BoxCollider>();
            if (col != null)
            {
                col.enabled = false;
				col.GetComponent<BlockMove>().mValid = false;
            }

			// プレイヤーのショットを不可に
			GetComponent<Player>().ShotFlg = false;

            return liftObj;
        }
        return null;
    }

    void LiftDown()
    {
        Debug.Log("LD");

        // 降ろす
        liftObj.transform.position = transform.position + new Vector3(LR, 0, 0);

        // 持ち上げた箱の当たり判定を戻す
        BoxCollider col = liftObj.GetComponent<BoxCollider>();
        if (col != null)
        {
            col.enabled = true;
			col.GetComponent<BlockMove>().mValid = true;
		}
		
		// プレイヤーのショットを可能に
		GetComponent<Player>().ShotFlg = true;

		// もう持ってないから捨てる
		liftObj = null;
    }

    void Lifting()
    {
       // Debug.Log("a");
        if(liftObj != null)
        {
			//  Debug.Log("b");
			//            liftObj.transform.position = transform.position + new Vector3(0, 1.2f, 0);
			liftObj.transform.position = liftPoint.position;
        }
    }

}
