using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public BodyController bodyController;
    private AgentMovementController agentController;
    public AttackManager attackManager;
    private Animator animator;

    //private AttackBarrier attackSpawnPoint;
    //private AttackBarrier attackFire;
    private FollowPlayerOnSight followPlayerOnSight;
    public AttackBarrier attackBarrier;
    public float animationSpeed = 0.25f;
    private float sightRange = 10f;

    private bool playerKiller = false;

    private Timer rightAfterDeathTimer = null;

    public AudioSource deathSound;
    public AudioSource hitSound;

    private EnemyManager enemyManager;

	// Use this for initialization
	void Start () {

        bodyController = GetComponent<BodyController>();
        agentController = GetComponent<AgentMovementController>();
        attackManager = GetComponent<AttackManager>();
        animator = GetComponent<Animator>();

        animator.speed = animationSpeed; 
        enemyManager = GameObject.Find("ZombieSpawner").GetComponent<EnemyManager>();
        playerKiller = (Random.value > 0.5f);

        followPlayerOnSight = new FollowPlayerOnSight(this.gameObject, attackManager.meeleAttackRange - 0.5f);
        //attackFire = new AttackBarrier(this.gameObject, attackManager.meeleAttackRange - 0.5f, GameObject.Find("BonFire"));
        defaultTarget();

    }

    void defaultTarget()
    {
        if (playerKiller)
        {
            agentController.SetBehavior(followPlayerOnSight);
        }
        else
        {
            
            if (enemyManager.spawnPointsToUncover.Count > 0)
            {
                GameObject randomSpawn = RandomUtil.choice(enemyManager.spawnPointsToUncover);
                attackBarrier = new AttackBarrier(this.gameObject, attackManager.meeleAttackRange - 0.5f, randomSpawn);
                agentController.SetBehavior(attackBarrier);
            }
            else
            {
                attackBarrier = new AttackBarrier(this.gameObject, attackManager.meeleAttackRange - 0.5f, GameObject.Find("BonFire"));
                agentController.SetBehavior(attackBarrier);
            }
            
        }
    }
	
	// Update is called once per frame
	void Update () {
        //bodyController.Ragdoll();	

        //playerInSight() && 
        //if (attackBarrier != null && attackBarrier.target != null)
        //{
        //    agentController.SetBehavior(followPlayerOnSight);
        //}

        
        if(agentController.movementBehavior == attackBarrier && attackBarrier != null)
        {
            if(attackBarrier.target == null || attackBarrier.target.GetComponent<Health>().currentHealth <=0)
            {
                // target just died, switch back to player
                //agentController.SetBehavior(followPlayerOnSight);
                defaultTarget();
            }
            
        }

        if(rightAfterDeathTimer != null && rightAfterDeathTimer.Expired())
        {
            agentController.agent.enabled = false;
        }
    }

    public void switchTarget(GameObject barrier)
    {
        if (barrier.GetComponent<Health>().currentHealth > 0)
        {
            attackBarrier = new AttackBarrier(this.gameObject, attackManager.meeleAttackRange - 0.5f, barrier);
            //attackManager.addBarrierTarget();
            agentController.SetBehavior(attackBarrier);
        }
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

        if (hitSound && !hitSound.isPlaying)
        {
            hitSound.pitch = Random.Range(0.8f, 1.2f);
            hitSound.Play();
        }

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
            if(headHit || bodyPart.gameObject.name == "SpineBone")
            {
                Vector3 hitDirection = Vector3.Normalize(bodyPart.transform.position - arrow.transform.position) * -1;
                // bodyPart.GetComponent<Rigidbody>().velocity = hitDirection * arrow.transform.GetComponent<Rigidbody>().velocity.magnitude * 0.9f;
                bodyPart.GetComponent<Rigidbody>().velocity = hitDirection * 10f;
                Debug.Log("Hit in " + this.name + " for velocity " + bodyPart.GetComponent<Rigidbody>().velocity);
            }
            rightAfterDeathTimer = new Timer();
            rightAfterDeathTimer.Start(5f);

            if (deathSound && !deathSound.isPlaying)
            {
                deathSound.pitch = Random.Range(0.8f, 1.2f);
                deathSound.Play();
            }

        }
    }
}
