using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyPart : MonoBehaviour
{
    /// <summary>
    /// Enemy object we are part of
    /// </summary>
    Enemy enemy;

    // Use this for initialization
    void Start()
    {
        this.enemy = this.GetComponentInParent<Enemy>();
        IgnoreLocalCollisions();

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
