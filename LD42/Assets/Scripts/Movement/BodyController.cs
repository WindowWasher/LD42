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
    }

    // Update is called once per frame
    void FixedUpdate()
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
        controller.Move(direction * speed * Time.fixedDeltaTime * physicsToForceModifer);
    }

    public void jump()
    {
        this.externalForces.y = jumpSpeed;
    }

    public void lookInDirection(Vector3 direction)
    {
        // Look rotation should work for something like a head, but you don't want a body to look at a player
        Vector3 lookRotation = Vector3.RotateTowards(transform.forward, direction, rotationSpeed * Time.fixedDeltaTime, 0.0f);
        Vector3 spinRotation = new Vector3(lookRotation.x, 0, lookRotation.z);
        if (spinRotation != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(spinRotation);
        }

    }

    private void updatePosition()
    {
        controller.Move(this.transform.InverseTransformDirection(externalForces * Time.fixedDeltaTime * physicsToForceModifer));
    }

    private void updateExternalForces()
    {
        // add friction and gravity
        float externalForceReduction = 2f;
        float externalFrameReduction = (Time.fixedDeltaTime * externalForceReduction);
        float maxExternalForce = 3f;
        if (Mathf.Abs(externalForces.x) > maxExternalForce)
        {
            Ragdoll();
            return;
            //externalForces = PhysicUtil.SetVector(externalForces, x: maxExternalForce * (externalForces.x > 0 ? 1 : -1));
        }
        if (Mathf.Abs(externalForces.x) <= externalFrameReduction)
        {
            externalForces.x = 0;
        }
        else
        {
            externalForces.x = externalForces.x - externalFrameReduction * (externalForces.x > 0 ? 1 : -1);
        }

        if (this.controller.isGrounded && externalForces.y < 0)
        {
            externalForces.y = 0f;
        }
        externalForces.y = Mathf.Max(externalForces.y - gravity * Time.fixedDeltaTime, -terminalVelocity);

        if (Mathf.Abs(externalForces.z) > maxExternalForce)
        {
            Ragdoll();
            return;
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

    public void Ragdoll()
    {
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
        GetComponent<CharacterController>().enabled = false;

        this.enabled = false;

    }
}