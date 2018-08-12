using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Arrow : MonoBehaviour {

    private float speed = 10f;

    private Vector3 targetPosition;
    private Vector3 direction;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.LookAt(targetPosition);
        //this.transform.Translate(this.transform.forward * Time.deltaTime * speed);
        float step = speed * Time.deltaTime;

        // Move our position a step closer to the target.
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        //this.transform.Translate(this.transform.forward * Time.deltaTime * speed);

        //this.transform.Translate(direction * Time.deltaTime * speed);
    }

    public void setTargetPosition(Vector3 targetPosition)
    {
        //this.gameObject.transform.LookAt(targetPosition);
        this.targetPosition = targetPosition;

        //this.transform.position = Vector3.zero;
        //this.transform.localPosition = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        this.transform.localRotation = Quaternion.identity;
        this.transform.localScale = new Vector3(1, 1, 1);
       

        this.transform.LookAt(targetPosition);


        direction = Vector3.Normalize(targetPosition - transform.position);

        Debug.DrawRay(this.transform.position, direction, Color.blue);
        Debug.Log("Direction: " + direction.ToString());

        //this.transform.forward = direction;

        //Vector3 targetDir = Vector3.Normalize(targetPosition - transform.position);

        //Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 100f, 0.0f);
        //transform.rotation = Quaternion.LookRotation(newDir);


        //var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = targetPosition;
    }
}
