using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private BodyController bodyController;
    private AgentMovementController agentController;
    private AttackManager attackManager;
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
}
