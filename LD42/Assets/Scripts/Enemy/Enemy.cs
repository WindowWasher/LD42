﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public BodyController bodyController;
    private AgentMovementController agentController;
    public AttackManager attackManager;
    private Animator animator;


    private FollowPlayerOnSight followPlayerOnSight;
    private float sightRange = 10f;

	// Use this for initialization
	void Start () {

        bodyController = GetComponent<BodyController>();
        agentController = GetComponent<AgentMovementController>();
        attackManager = GetComponent<AttackManager>();
        animator = GetComponent<Animator>();

        animator.speed = 0.25f;

        followPlayerOnSight = new FollowPlayerOnSight(this.gameObject, attackManager.meeleAttackRange - 0.5f);
        agentController.SetBehavior(followPlayerOnSight);

        if (playerInSight())
        {
            agentController.SetBehavior(followPlayerOnSight);
        }


    }
	
	// Update is called once per frame
	void Update () {
        //bodyController.Ragdoll();	
	}

    bool playerInSight()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        RaycastHit hit;
        // adding 1 unit up for now for rayOrigin, TODO: make ray originate from head
        Vector3 rayOrigin = this.transform.position + new Vector3(0, 1, 0);
        Vector3 direction = player.transform.position - rayOrigin;

        return Physics.Raycast(rayOrigin, direction, out hit, sightRange);
    }

    public void HitWithArrow(EnemyBodyPart bodyPart, Arrow arrow)
    {
        Health health = GetComponent<Health>();
        if (health.currentHealth <= 0)
            return;
        bool headHit = (bodyPart.gameObject.name == "HeadBone");
        health.TakeDamage(arrow.damage);
        if(headHit)
        {
            health.TakeDamage(arrow.damage * 10);
        }

        if (health.currentHealth <= 0)
        {
            //Destroy(this.gameObject);
            foreach(Rigidbody body in GetComponentsInChildren<Rigidbody>())
            {
                body.velocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
            }
            
            bodyController.Ragdoll();
            // If we just died, also make it look like enemy was shot
            if(headHit)
            {
                Vector3 hitDirection = Vector3.Normalize(bodyPart.transform.position - arrow.transform.position);
                bodyPart.GetComponent<Rigidbody>().velocity = hitDirection * arrow.transform.GetComponent<Rigidbody>().velocity.magnitude * 0.75f;
                Debug.Log("Hit in " + this.name + " for velocity " + bodyPart.GetComponent<Rigidbody>().velocity);
            }

        }
    }
}
