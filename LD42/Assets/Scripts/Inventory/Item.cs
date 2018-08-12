using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    bool falling = false;

    private float gravitySpeed = 4f;

    //private Timer settleTimer = new Timer();
    //private float timeUntilSettle = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (falling)
        {
            this.GetComponent<Rigidbody>().MovePosition(this.transform.position + Vector3.down * gravitySpeed * Time.deltaTime);
        }

        //if (!falling && settleTimer.Expired() && this.GetComponent<Rigidbody>().isKinematic == false)
        //{
            
        //    //this.GetComponent<Rigidbody>().useGravity = false;

        //}
    }

    public void pickup()
    {
        falling = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
        //this.GetComponent<Rigidbody>().useGravity = false;
    }

    public void falldown(LayerMask itemLayerMask)
    {
        falling = true;
       // this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Hit " + collision.gameObject.name);
        if(collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            falling = false;
            this.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

}
