using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    bool falling = false;

    private float gravitySpeed = 4f;

    private bool heldByPlayer = false;
    public int healAmount;
    public bool isHealthPack;
    //private Timer settleTimer = new Timer();
    //private float timeUntilSettle = 0f;

    private PlayerInventory playerInventory;

	// Use this for initialization
	void Start () {

        playerInventory = GameObject.Find("Player").GetComponent<PlayerInventory>();
		
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
        heldByPlayer = true;
        falling = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.GetComponent<Rigidbody>().useGravity = false;
        this.GetComponent<Collider>().isTrigger = true;

    }

    public void falldown(LayerMask itemLayerMask)
    {
        heldByPlayer = false;
        falling = true;
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Collider>().isTrigger = true;
        //this.GetComponent<Rigidbody>().AddForce(GameObject.Find("Player").GetComponent<Rigidbody>().velocity);
        //this.transform.LookAt(Vector3.up);
        //this.transform.Translate(Vector3.back * 2f);
        //var heading = GameObject.Find("Player").transform.position - this.transform.position;
        //var distance = heading.magnitude;
        //var direction = heading / distance;
        //direction.y = 0;
        //this.transform.Translate(direction * 5f);
        //this.GetComponent<Rigidbody>().isKinematic = false;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    //Debug.Log("Hit " + collision.gameObject.name);
    //    if(collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
    //    {
    //        falling = false;
    //        this.GetComponent<Rigidbody>().isKinematic = true;
    //        return;
    //    }


    //    Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
    //    if (enemy)
    //    {
    //        if(heldByPlayer && !playerInventory.hittingEnemiesTimer.Expired())
    //        {
    //            var heading = enemy.transform.position - this.transform.position;
    //            var distance = heading.magnitude;
    //            var direction = heading / distance;
    //            enemy.bodyController.externalForces += (direction * 2f);
    //            enemy.attackManager.UnFreezeIfHolding();
    //        }
    //        else
    //        {
    //            enemy.switchTarget(this.gameObject);
    //        }
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("collision item ");
    //    if (falling && collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
    //    {
    //        falling = false;
    //        //this.GetComponent<Rigidbody>().isKinematic = true;
    //        this.GetComponent<Rigidbody>().useGravity = false;
    //        return;
    //    }


    //    Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
    //    if (enemy)
    //    {
    //        if (heldByPlayer && !playerInventory.hittingEnemiesTimer.Expired())
    //        {
    //            var heading = enemy.transform.position - this.transform.position;
    //            var distance = heading.magnitude;
    //            var direction = heading / distance;
    //            enemy.bodyController.externalForces += (direction * 2f);
    //            enemy.attackManager.UnFreezeIfHolding();
    //        }
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hit " + collision.gameObject.name);
        if (falling && other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            falling = false;
            //this.GetComponent<Rigidbody>().isKinematic = true;
            this.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<Collider>().isTrigger = false;
            //this.transform.LookAt(Vector3.down);

            return;
        }


        //Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();
        //if (enemy)
        //{
        //    if (heldByPlayer && !playerInventory.hittingEnemiesTimer.Expired())
        //    {
        //        var heading = enemy.transform.position - this.transform.position;
        //        var distance = heading.magnitude;
        //        var direction = heading / distance;
        //        enemy.bodyController.externalForces += (direction * 2f);
        //        enemy.attackManager.UnFreezeIfHolding();
        //    }
        //}
    }



    public void finish()
    {
        Destroy(this.gameObject);
    }

}
