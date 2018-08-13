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
        Debug.Log("Health " + health.currentHealth);
	}

    public void heal(Item item)
    {
        //health.HealDamage(item.healAmount);
        float percent = ((float)health.currentHealth) / health.MaxHealth;

        if (percent >= 0.33)
        {
            setHealth(100);
        }
        else if (percent > 0)
        {
            setHealth(65);
        }
        else
        {
            setHealth(32);
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

        if (percent >= 0.66)
        {
            door1.SetActive(true);
            door2.SetActive(false);
            door3.SetActive(false);
        }
        else if (percent >= 0.33)
        {
            door1.SetActive(false);
            door2.SetActive(true);
            door3.SetActive(false);
        }
        else if (percent > 0)
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
