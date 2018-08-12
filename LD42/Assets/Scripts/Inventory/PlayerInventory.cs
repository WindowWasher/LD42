using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

    private Animator playerAnimator;
    private GameObject arrowModel;

    private PlayerBow playerBow;

    private GameObject heldItemHolder;

    //private Item reachableItem;

    private float reach = 5f;

    public Item heldItem;

    private Timer heldItemTimer = new Timer();
    private float timeUntilDropable = 0.5f;


	// Use this for initialization
	void Start () {
        playerAnimator = GameObject.Find("PlayerModel").GetComponent<Animator>();
        arrowModel = GameObject.Find("ArrowModel");
        heldItemHolder = GameObject.Find("HeldItemHolder");

        playerBow = GetComponent<PlayerBow>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetButtonDown("Fire2"))
        {
            if (heldItem == null)
            {
                GameObject obj = playerBow.getTargetedObject();

                Item item = obj == null ? null : obj.GetComponent<Item>();

                if (item != null && Vector3.Distance(obj.transform.position, transform.position) < 5f)
                {
                    pickUp(item);

                }
            }
            else
            {
                if(heldItemTimer.Expired())
                {
                    dropItem();

                }
            }


        }
	}

    private void pickUp(Item item)
    {
        Debug.Log("picked up " + item.gameObject.ToString());
        playerAnimator.Play("PlayerHoldingItem");
        arrowModel.SetActive(false);
        heldItem = item;
        heldItemTimer.Start(timeUntilDropable);

        item.gameObject.transform.parent = heldItemHolder.transform;
        item.gameObject.transform.position = heldItemHolder.transform.position;
        item.gameObject.transform.rotation = heldItemHolder.transform.rotation;

        item.gameObject.GetComponent<Rigidbody>().useGravity = false;
    }

    private void dropItem()
    {

        heldItem.gameObject.GetComponent<Rigidbody>().useGravity = true;
        heldItem.gameObject.transform.parent = null;

        heldItem = null;
        playerBow.SteadyBow();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Item item = other.GetComponent<Item>();
    //    if(item != null && reachableItem == null)
    //    {
    //        reachableItem = item;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    Item item = other.GetComponent<Item>();
    //    if (item == reachableItem)
    //    {
    //        reachableItem = null;
    //    }
    //}

}
