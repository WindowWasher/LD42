using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyPart : MonoBehaviour
{
    /// <summary>
    /// Enemy object we are part of
    /// </summary>
    Enemy enemy;

    public GameObject jointBodyObj;
    public bool hasRigidBody;

    // Use this for initialization
    void Start()
    {
        this.enemy = this.GetComponentInParent<Enemy>();
        IgnoreLocalCollisions();

        //GetComponent<Rigidbody>().isKinematic = true;
        //Destroy(this.GetComponent<CharacterJoint>());
        //Destroy(this.GetComponent<Rigidbody>());

        CharacterJoint joint = this.GetComponent<CharacterJoint>();
        Rigidbody body = this.GetComponent<Rigidbody>();

        if(joint != null)
        {
            jointBodyObj = joint.connectedBody.gameObject;
            Destroy(this.GetComponent<CharacterJoint>());
        }
        if(body != null)
        {
            hasRigidBody = true;
            //Destroy(this.GetComponent<Rigidbody>());
            this.GetComponent<Rigidbody>().isKinematic = true;
        }


        this.GetComponent<Collider>().isTrigger = true;

    }

    // Update is called once per frame
    void Update()
    {

    }

    ///// <summary>
    ///// Ignore collisions between body parts
    ///// </summary>
    void IgnoreLocalCollisions()
    {
        foreach (EnemyBodyPart sisterBodyPart in enemy.GetComponentsInChildren<EnemyBodyPart>())
        {
            Physics.IgnoreCollision(this.GetComponent<Collider>(), sisterBodyPart.GetComponent<Collider>());
        }
        
        //Physics.IgnoreCollision(this.GetComponent<Collider>(), this.enemy.bodyController.controller.GetComponent<Collider>());

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EnemyBodyPart>())
            return;
        //Debug.Log("Trigger on " + other.gameObject.name);

        PlayerInventory playerInventory = GameObject.Find("Player").GetComponent<PlayerInventory>();


        Item item = other.GetComponent<Item>();

        if(item != null && item == playerInventory.heldItem && !playerInventory.hittingEnemiesTimer.Expired())
        {
            var heading = enemy.transform.position - other.transform.position;
            var distance = heading.magnitude;
            var direction = heading / distance;
            enemy.bodyController.externalForces += (direction * 1f);
            enemy.attackManager.UnFreezeIfHolding();
        }

        if (other.GetComponent<Obstacle>() != null)
        {
            //Debug.Log("Obstabcle hit " + other.gameObject.name);

            if (playerInventory.heldItem == null || playerInventory.heldItem != this.GetComponent<Item>())
            {
                //Debug.Log("Switching target to " + this.transform.gameObject.name);
                enemy.switchTarget(other.gameObject);

                //var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //obj.transform.position = this.transform.position;

            }
        }

        Arrow arrow = other.GetComponent<Arrow>();

        if(arrow)
        {
            hitWithArrow(arrow, other);
        }
    }

    public void hitWithArrow(Arrow arrow, Collider other)
    {

            enemy.HitWithArrow(this, arrow);


        arrow.transform.position = this.GetComponent<Collider>().ClosestPointOnBounds(arrow.transform.position);


        // do this to avoid scaling issues
        GameObject newObj = new GameObject();
        arrow.transform.parent = newObj.transform;
        arrow.transform.parent = this.transform;


        //newObj.transform.parent = other.GetComponentInParent<Enemy>().GetComponentInChildren<EnemyBodyPart>().gameObject.transform;

        arrow.GetComponent<Rigidbody>().velocity = Vector3.zero;
        arrow.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        arrow.GetComponent<Rigidbody>().useGravity = false;
        arrow.GetComponent<Rigidbody>().isKinematic = true;
        //this.transform.parent = other.transform;
        arrow.GetComponent<CapsuleCollider>().enabled = false;


    }
    

    //private void OnCollisionEnter(Collision other)
    //{
    //    Debug.Log("Trigger on " + other.gameObject.name);
    //    if (other.gameObject.GetComponent<Obstacle>() != null)
    //    {
    //        PlayerInventory playerInventory = GameObject.Find("Player").GetComponent<PlayerInventory>();
    //        Debug.Log("Obstabcle hit " + other.gameObject.name);

    //        Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();
    //        if (enemy && (playerInventory.heldItem == null || playerInventory.heldItem != this.GetComponent<Item>()))
    //        {
    //            //Debug.Log("Switching target to " + this.transform.gameObject.name);
    //            enemy.switchTarget(this.gameObject);

    //            //var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //            //obj.transform.position = this.transform.position;

    //        }
    //    }
    //}

    ///// <summary>
    ///// Body part has hit something
    ///// </summary>
    ///// <param name="collision"></param>
    //private void OnCollisionEnter(Collision collision)
    //{
    //    this.enemy.BodyPartCollision(this, collision);
    //}
}
