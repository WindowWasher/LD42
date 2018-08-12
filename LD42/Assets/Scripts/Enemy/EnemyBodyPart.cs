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
            Destroy(this.GetComponent<Rigidbody>());
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

    ///// <summary>
    ///// Body part has hit something
    ///// </summary>
    ///// <param name="collision"></param>
    //private void OnCollisionEnter(Collision collision)
    //{
    //    this.enemy.BodyPartCollision(this, collision);
    //}
}
