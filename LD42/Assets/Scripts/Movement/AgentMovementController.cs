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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (movementBehavior != null)
        {
            moveTowardsTarget();
        }
    }


    private void initAgent()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = bodyController.speed;
        // setting a higher acceleration helps the agent rotate faster
        agent.acceleration = bodyController.speed *= 2;
        agent.autoBraking = true;
        agent.updatePosition = false;
        agent.updateRotation = false;
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
        if(attackManager.currentAttack == null || attackManager.findTargetInRange(attackManager.currentAttack) == null)
        {
            bodyController.moveInDirection(directionToTarget);
        }
        if (rotateOverride != null)
        {
            bodyController.lookInDirection(bodyController.getDirectionToTarget(rotateOverride.transform.position));
        }
        else
        {
            bodyController.lookInDirection(directionToTarget);
        }
        // since the agent is not actively moving, we have to keep it updated to where it is
        agent.nextPosition = this.transform.position;
    }

    public void SetBehavior(MovementBehavior behavior)
    {
        this.movementBehavior = behavior;
    }


}
