using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Arrow : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(this.transform.forward * Time.deltaTime);
    }

    public void setTargetPosition(Vector3 targetPosition)
    {
        this.transform.LookAt(targetPosition);
    }
}
