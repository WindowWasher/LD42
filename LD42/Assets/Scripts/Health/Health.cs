using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public delegate void OnHealthChangeAction(int currentHealth, int maxHealth);
    public event OnHealthChangeAction OnHealthChange;

    [SerializeField]
    private int maxHealth;

    public int MaxHealth { get { return maxHealth; } private set { maxHealth = value; } }

    public int currentHealth { get; private set; }

    private bool isAlive = true;
    private bool isDoorFixer = false;

    public bool playerCanFix = false;

    void Start()
    {
        currentHealth = MaxHealth;
        // Make sure everything is displayed correctly at the start
        if (OnHealthChange != null)
        {
            OnHealthChange(currentHealth, maxHealth);
        }

        if(this.GetComponent<DoorFixer>() != null)
        {
            currentHealth = 0;
            isDoorFixer = true;
        }

    }


    public void TakeDamage(int amount)
    {
        if (isAlive || isDoorFixer)
        {
            currentHealth -= amount;

            if (OnHealthChange != null)
            {
                OnHealthChange(currentHealth, maxHealth);
            }

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                if (this.GetComponent<DoorFixer>() != null)
                {
                    isAlive = false;
                    Die();
                }
            }
        }
    }

    public void HealDamage(int amount)
    {
        if (isAlive || isDoorFixer)
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

            if (OnHealthChange != null)
            {
                OnHealthChange(currentHealth, maxHealth);
            }
        }
    }

    private void Die()
    {
        Debug.Log(this.gameObject.name + " death!!!");
        if (this.gameObject.name == "Player")
        {
            Debug.Log("Player died!");
        }
        else if (this.gameObject.GetComponent<Enemy>())
        {
            // enemy handles its own death
        }
        else if (this.gameObject.GetComponent<DoorFixer>())
        {
            // doors can't die
        }
        else { 
            Destroy(this.gameObject);
        }
        //Debug.Log("Death!!!!");
        //Destroy(gameObject);
        //if(this.GetComponent<Enemy>() != null)
        //{
        //    this.GetComponent<BodyController>().Ragdoll();
        //}
    }
}


