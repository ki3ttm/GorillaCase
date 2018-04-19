using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour {
    [SerializeField] float maxDis = 10.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        RaycastHit hitInfo;
        Debug.DrawRay(transform.position, transform.forward);
        Physics.Raycast(transform.position, transform.forward, out hitInfo, maxDis);

        if(hitInfo.collider.name == "WeightBox")
        {

        }

	}
}
