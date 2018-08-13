using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMovementController : MonoBehaviour
{
    public BodyController bodyController;
    public MovementBehavior movementBehavior = null;

    public NavMeshAgent agent;

    public GameObject rotateOverride = null;
    private AttackManager attackManager;

    // Use this for initialization
    void Start()
    {
        bodyController = GetComponent<BodyController>();
        initAgent();
        attackManager = GetComponent<AttackManager>();
        //Destroy(this.GetComponent<CharacterController>());
    }

    // Update is called once per frame
    void Update()
    {
        if (movementBehavior != null)
        {
            moveTowardsTarget();
        }
    }


    private void initAgent()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = bodyController.speed * 1.75f; /*0.5f*/
        //agent.speed = 0f;
        // setting a higher acceleration helps the agent rotate faster
        agent.acceleration = bodyController.speed *= 2;
        agent.autoBraking = true;
        //agent.updatePosition = false;
        //agent.updateRotation = false;
        agent.updatePosition = true;
        agent.updateRotation = true;
    }

    Vector3 getNextPosition()
    {
        Vector3 targetDestination = movementBehavior.getTargetDestination();
        agent.SetDestination(targetDestination);
        return agent.nextPosition;
    }

    void moveTowardsTarget()
    {
        Vector3 directionToTarget = bodyController.getDirectionToTarget(getNextPosition());
        //if(attackManager.currentAttack == null || attackManager.findTargetInRange(attackManager.currentAttack) == null)
        //{
        //    //bodyController.moveInDirection(directionToTarget);
        //    //this.transform.Translate(directionToTarget * bodyController.speed * 10 * Time.deltaTime);
        //}
        if (rotateOverride != null)
        {
            bodyController.lookInDirection(bodyController.getDirectionToTarget(rotateOverride.transform.position));
        }
        else
        {
            bodyController.lookInDirection(directionToTarget);
        }

        //if (attackManager.currentAttack == null || attackManager.findTargetInRange(attackManager.currentAttack) == null)
        //{
        //    //bodyController.moveInDirection(directionToTarget);
        //    this.transform.Translate(this.transform.forward * bodyController.speed * 0.25f * Time.deltaTime);
        //}

        // since the agent is not actively moving, we have to keep it updated to where it is
        //agent.nextPosition = this.transform.position;
    }

    public void SetBehavior(MovementBehavior behavior)
    {
        this.movementBehavior = behavior;
    }


}
