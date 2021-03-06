﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BodyController : MonoBehaviour
{


   
    public float speed;
    public float jumpSpeed;
    public float rotationSpeed;
    private float gravity = 7.8f;
    private float terminalVelocity = 5f;
    private float physicsToForceModifer = 10f;
    public Vector3 externalForces = Vector3.zero;
    public CharacterController controller;

   

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
        controller.detectCollisions = false;
    }

    // Update is called once per frame
    void Update()
    {
        updateExternalForces();
        updatePosition();

    }

    public Vector3 getDirectionToTarget(Vector3 targetPosition)
    {
        return targetPosition - this.transform.position;
    }


    public void moveInDirection(Vector3 direction)
    {
        controller.Move(direction * speed * Time.deltaTime * physicsToForceModifer);
    }

    public void jump()
    {
        Debug.Log("Jumping!");
        this.externalForces.y = jumpSpeed;
    }

    public void lookInDirection(Vector3 direction)
    {
        // Look rotation should work for something like a head, but you don't want a body to look at a player
        Vector3 lookRotation = Vector3.RotateTowards(transform.forward, direction, rotationSpeed * Time.deltaTime, 0.0f);
        Vector3 spinRotation = new Vector3(lookRotation.x, 0, lookRotation.z);
        if (spinRotation != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(spinRotation);
        }

    }

    private void updatePosition()
    {
        if(externalForces != Vector3.zero)
        {
            //Debug.Log(this.gameObject.name + " External Forces: " + externalForces.ToString());
            controller.Move(this.transform.InverseTransformDirection(externalForces * Time.deltaTime * physicsToForceModifer));
        }
        //controller.Move(this.transform.InverseTransformDirection(externalForces * Time.deltaTime * physicsToForceModifer));
    }

    private void updateExternalForces()
    {
        // add friction and gravity
        float externalForceReduction = 2f;
        float externalFrameReduction = (Time.deltaTime * externalForceReduction);
        float maxExternalForce = 3f;
        if (Mathf.Abs(externalForces.x) > maxExternalForce)
        {
            //Ragdoll();
            //return;
            externalForces.x = maxExternalForce * (externalForces.x > 0 ? 1 : -1);
        }
        if (Mathf.Abs(externalForces.x) <= externalFrameReduction)
        {
            externalForces.x = 0;
        }
        else
        {
            externalForces.x = externalForces.x - externalFrameReduction * (externalForces.x > 0 ? 1 : -1);
        }

        if(this.gameObject.GetComponent<Enemy>() == null)
        {
            if (this.controller.isGrounded && externalForces.y < 0)
            {
                externalForces.y = -0.5f;
            }
            //if(this.controller.isGrounded)
            //{
            //    externalForces.y = 0f;
            //}
            //else
            //{
            externalForces.y = Mathf.Max(externalForces.y - gravity * Time.deltaTime, -terminalVelocity);
            //    if(externalForces.y > -0.3f && externalForces.y < 0f)
            //{
            //    externalForces.y = -0.5f;
            //    externalForces.x = 1f;
            //}
            //Debug.Log("ExternalForces2: " + externalForces.ToString());
            //}

        }

        //if ((this.gameObject.GetComponent<Enemy>() != null || this.controller.isGrounded) && externalForces.y < 0)
        //{
        //    externalForces.y = 0f;
        //}
        

        if (Mathf.Abs(externalForces.z) > maxExternalForce)
        {
            //Ragdoll();
            //return;
            externalForces.z = maxExternalForce * (externalForces.z > 0 ? 1 : -1);

        }

        if (Mathf.Abs(externalForces.z) <= externalFrameReduction)
        {
            externalForces.z = 0;
        }
        else
        {
            externalForces.z = externalForces.z - externalFrameReduction * (externalForces.z > 0 ? 1 : -1);
        }
    }

    //public bool isGrounded()
    //{
        
    //}

    public void Ragdoll()
    {
        //this.GetComponent<NavMeshAgent>().enabled = false;
        foreach(EnemyBodyPart bodyPart in this.GetComponentsInChildren<EnemyBodyPart>())
        {
            //if(bodyPart.hasRigidBody)
            //{
            //    bodyPart.gameObject.AddComponent<Rigidbody>();
            //}
            if(bodyPart.jointBodyObj != null)
            {
                bodyPart.gameObject.AddComponent<CharacterJoint>();
                bodyPart.gameObject.GetComponent<CharacterJoint>().connectedBody = bodyPart.jointBodyObj.GetComponent<Rigidbody>();
            }
        }
        
        foreach (Collider collider in this.GetComponentsInChildren<Collider>())
        {
            collider.isTrigger = false;
        }

        foreach (Rigidbody part in this.GetComponentsInChildren<Rigidbody>())
        {
            part.isKinematic = false;
        }
        GetComponent<AgentMovementController>().enabled = false;
        GetComponent<Animator>().enabled = false;
        controller.enabled = false;
        

        this.enabled = false;

    }
}
