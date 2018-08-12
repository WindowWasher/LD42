using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

    PlayerInventory playerInventory;

	// Use this for initialization
	void Start () {
        playerInventory = GameObject.Find("Player").GetComponent<PlayerInventory>();


    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Obstabcle hit " + other.gameObject.name);

        Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();
        if (enemy && (playerInventory.heldItem == null || playerInventory.heldItem != this.GetComponent<Item>()))
        {
            Debug.Log("Switching target to " + this.transform.gameObject.name);
            enemy.switchTarget(this.gameObject);

            //var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //obj.transform.position = this.transform.position;
            
        }
    }
}
