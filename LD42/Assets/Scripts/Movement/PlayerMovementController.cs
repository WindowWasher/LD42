﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovementController : MonoBehaviour
{
    public GameObject playerView;
    public BodyController bodyController;

    private float mouseXSensitivity = 5.0f;
    private float mouseYSensitivity = 2.0f;
    private float mouseSmoothing = 1.0f;
    private Vector2 smoothV;
    private Vector2 mouseLook;

    void Start()
    {
        bodyController = GetComponent<BodyController>();
        lockCursor();
    }

    void bodyMove()
    {
        Vector3 targetDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        targetDirection = transform.TransformDirection(targetDirection);

        bool isGrounded = bodyController.controller.isGrounded;
        if (isGrounded && Input.GetButton("Jump"))
        {
            bodyController.jump();
        }

        if (targetDirection != Vector3.zero)
        {
            bodyController.moveInDirection(targetDirection);
        }
    }

    void cameraMove()
    {
        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        md = Vector2.Scale(md, new Vector2(mouseXSensitivity * mouseSmoothing, mouseYSensitivity * mouseSmoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / mouseSmoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / mouseSmoothing);
        mouseLook += smoothV;

        playerView.transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        this.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, this.transform.up);
    }

    private void FixedUpdate()
    {
        bodyMove();
        cameraMove();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        float pushPower = 20f;
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3F)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;
    }

    private void OnTriggerEnter(Collider other)
    {
        AttackManager attackController = other.gameObject.GetComponentInParent<AttackManager>();

        if (attackController != null)
        {
            attackController.HitHealth(GetComponent<Health>(), other.gameObject);
        }
    }

    void setCursorState()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            unlockCursor();
        }
        else if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            lockCursor();
        }
    }

    void lockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void unlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
