﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBow : MonoBehaviour {

    /// <summary>
    /// Don't hit objects that are on certain layers such as "PlayerLayer" or "Ignore Raycast"
    /// </summary>
    public LayerMask aimMask;

    /// <summary>
    /// Max distance of raycast when detecting targed object
    /// </summary>
    private float maxAimDistance = 500;
    /// <summary>
    /// If we don't hit anything with raycast, use this as the distance
    /// </summary>
    private float defaulAimtDistance = 50;
    /// <summary>
    /// Min aim distance (to avoid weird behavior for objects right in player's face)
    /// </summary>
    private float minAimDistance = 2;

    private GameObject playerArrow;
    private GameObject arrowModel;
    private GameObject arrowModelDisplay;

    private Animator playerAnimator;
    private Animator playerArrowAnimator;

    private Timer arrowReloadTimer = null;
    private float arrowReloadSpeed = 0.75f;

    private Timer aimTimer = new Timer();
    private Timer drawTimer = new Timer();
    public bool aiming = false;


    public AudioSource bowDrawSound;
    public AudioSource arrowFireSound;

    private PlayerInventory playerInventory;

    // Use this for initialization
    void Start () {

        playerArrow = GameObject.Find("PlayerArrow");
        playerAnimator = GameObject.Find("PlayerModel").GetComponent<Animator>();
        arrowModel = GameObject.Find("ArrowModel");
        arrowModelDisplay = GameObject.Find("ArrowModelDisplay");
        playerArrowAnimator = arrowModel.GetComponent<Animator>();

        playerInventory = GetComponent<PlayerInventory>();

        //arrowModel = GameObject.Find("ArrowModel");

        playerAnimator.speed = 0.25f;
        playerArrowAnimator.speed = 0.25f;
        playerArrow = GameObject.Find("PlayerArrow");
        playerArrow.SetActive(false);

    }

    public void SteadyBow()
    {
        arrowModel.SetActive(true);
        startBowAnimation("PlayerSteadyBowBlendTree");
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetButton("Fire1") && aimTimer.Expired() && !aiming && playerInventory.heldItem == null)
        {
            //playerAnimator.Play("PlayerFireBowBlendTree");
            //playerArrowAnimator.Play("PlayerFireBowBlendTree");
            aiming = true;
            drawTimer.Start(0.25f);
            startBowAnimation("PlayerAimBlendTree");

            if (bowDrawSound && !bowDrawSound.isPlaying)
                bowDrawSound.Play();
        }

        if(Input.GetButtonUp("Fire1") && aiming && drawTimer.Expired() && playerInventory.heldItem == null)
        {
            aiming = false;
            aimTimer.Start(1f);
            startBowAnimation("PlayerFireBowBlendTree");

            //GameObject newArrow = GameObject.Instantiate(playerArrow, playerArrow.transform.position, playerArrow.transform.rotation);
            //GameObject newArrow = GameObject.Instantiate(playerArrow, playerArrow.transform.position, playerArrow.transform.rotation);
            GameObject newArrow = GameObject.Instantiate(playerArrow);


            //newArrow.transform.rotation = Quaternion.identity;
            //newArrow.transform.localRotation = Quaternion.identity;
            //newArrow.transform.position = playerArrow.transform.position;
            newArrow.SetActive(true);
            newArrow.AddComponent<Arrow>();
            newArrow.GetComponent<Arrow>().setTargetPosition(playerArrow.transform.position, getTargetPosition());

            arrowModelDisplay.SetActive(false);
            arrowReloadTimer = new Timer();
            arrowReloadTimer.Start(arrowReloadSpeed);

            if (arrowFireSound && !arrowFireSound.isPlaying)
                arrowFireSound.Play();
        }

        if(arrowReloadTimer != null && arrowReloadTimer.Expired())
        {
            arrowModelDisplay.SetActive(true);
            arrowReloadTimer = null;
        }
    }

    void startBowAnimation(string animationName)
    {
        playerAnimator.Play(animationName);
        playerArrowAnimator.Play(animationName);
    }

    public GameObject getTargetedObject()
    {
        //// rayOrigin is the center of the camera's screen (0.5f/0.5f) at the player (0)
        RaycastHit hit;
        if (getRaycastFromCamera(out hit))
        {
            return hit.transform.gameObject;
        }
        return null;
    }

    bool getRaycastFromCamera(out RaycastHit hit)
    {
        Vector3 rayOrigin = getCameraWorldPoint();
        if (Physics.Raycast(rayOrigin, getPlayerCamera().transform.forward, out hit, maxAimDistance, aimMask))
        {
            return true;
        }
        return false;
    }

    public Vector3 getCameraWorldPoint()
    {
        return getPlayerCamera().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
    }

    Camera getPlayerCamera()
    {
        return GetComponentInChildren<Camera>();
    }

    Vector3 getTargetPosition()
    {
        Vector3 targetPosition;
        RaycastHit hit;
        if (getRaycastFromCamera(out hit))
        {
            float distance = Vector3.Distance(playerArrow.transform.position, hit.point);
            //Debug.Log("Distance: " + distance.ToString());
            if (distance < minAimDistance)
            {
                // ensure we don't get odd gun physics trying to shoot something in front player's face
                targetPosition = playerArrow.transform.position + (getPlayerCamera().transform.forward * minAimDistance);
            }
            else
            {
                targetPosition = hit.point;

                //var heading = targetPosition - getCameraWorldPoint();
                //var distance2 = heading.magnitude;
                //var direction2 = heading / distance2; //
                //targetPosition = direction2 * defaulAimtDistance;
            }
        }
        else
        {
            // shoot out X units from the player camera to where the player is looking
            targetPosition = getCameraWorldPoint() + (getPlayerCamera().transform.forward * defaulAimtDistance);
        }

        return targetPosition;

    }
}
