using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour {
    [SerializeField] float maxDis = 10.0f;
    [SerializeField] GameObject liftObj = null;
    [SerializeField] bool LR = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(LR == false)
            {
                LR = true;
            }
            else
            {
                LR = false;
            }
        }


        // 持ち上げ下げ
        LiftUpDown();

        // 持ち続け
        Lifting();
	}

    GameObject LiftUp()
    {
        Debug.Log("LS");
        RaycastHit hitInfo;
        Debug.DrawRay(transform.position, transform.forward);
        Physics.Raycast(transform.position, transform.forward, out hitInfo, maxDis);

        liftObj = hitInfo.collider.gameObject;
        return liftObj;
    }

    void Lifting()
    {
        Debug.Log("a");
        if(liftObj != null)
        {
            Debug.Log("b");
            liftObj.transform.position = transform.position + new Vector3(0, 1.2f, 0);
        }
    }

    void LiftDown()
    {
        // 降ろす
        if(LR == false)
        {
            // 左
            liftObj.transform.position = transform.position + new Vector3(1.0f, 0, 0);
        }
        else
        {
            // 右
            liftObj.transform.position = transform.position + new Vector3(-1.0f, 0, 0);
        }

        // もう持ってないから捨てる
        liftObj = null;
    }

    void LiftUpDown()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(liftObj == null)
            {
                LiftUp();
            }else
            {
                LiftDown();
            }
        }
    }
}
