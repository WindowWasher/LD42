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

    void Start()
    {
        currentHealth = MaxHealth;
        // Make sure everything is displayed correctly at the start
        if (OnHealthChange != null)
        {
            OnHealthChange(currentHealth, maxHealth);
        }

    }


    public void TakeDamage(int amount)
    {
        if (isAlive)
        {
            currentHealth -= amount;

            if (OnHealthChange != null)
            {
                OnHealthChange(currentHealth, maxHealth);
            }

            if (currentHealth == 0)
            {
                isAlive = false;
                Die();
            }
        }
    }

    public void HealDamage(int amount)
    {
        if (isAlive)
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
        //Debug.Log("Death!!!!");
        //Destroy(gameObject);
        //if(this.GetComponent<Enemy>() != null)
        //{
        //    this.GetComponent<BodyController>().Ragdoll();
        //}
    }
}


