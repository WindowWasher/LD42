using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorFixer : MonoBehaviour {

    //public List<GameObject> gameDoors;
    public GameObject door1;
    public GameObject door2;
    public GameObject door3;

    Health health;


	// Use this for initialization
	void Start () {

        health = GetComponent<Health>();
        setDoor();
		
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("Health " + health.currentHealth);
	}

    public void heal(Item item)
    {
        //health.HealDamage(item.healAmount);
        float percent = ((float)health.currentHealth) / health.MaxHealth;

        if (health.currentHealth >= 400)
        {
            setHealth(600);
        }
        else if (health.currentHealth >= 200)
        {
            setHealth(400);
        }
        else
        {
            setHealth(200);
        }
        setDoor();
    }

    void setHealth(int amount)
    {
        health.TakeDamage(health.currentHealth);
        health.HealDamage(amount);
    }

    public void setDoor()
    {
        float percent = ((float)health.currentHealth) / health.MaxHealth;
        //Debug.Log("Percent; " + percent.ToString());

        if (health.currentHealth > 400)
        {
            door1.SetActive(true);
            door2.SetActive(false);
            door3.SetActive(false);
        }
        else if (health.currentHealth > 200)
        {
            door1.SetActive(false);
            door2.SetActive(true);
            door3.SetActive(false);
        }
        else if (health.currentHealth > 0)
        {
            door1.SetActive(false);
            door2.SetActive(false);
            door3.SetActive(true);
        }
        else
        {
            door1.SetActive(false);
            door2.SetActive(false);
            door3.SetActive(false);
        }
    }
}
