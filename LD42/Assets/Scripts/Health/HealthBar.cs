using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    private Health health;
    public Image healthFg;

    bool firstUpdate = false;

    // Use this for initialization
    void Start()
    {

        health = gameObject.GetComponentInParent<Health>();
        health.OnHealthChange += UpdateHealth;
    }

    private void OnDisable()
    {
        if (health.gameObject.activeSelf)
        {
            health.OnHealthChange -= UpdateHealth;
        }
    }

    void UpdateHealth(int currentHealth, int damageTaken)
    {
        Debug.Log("updaingt helath");
        healthFg.fillAmount = (float)currentHealth / health.MaxHealth;
    }

    public void Reset()
    {
        healthFg.fillAmount = 100;
        //health.OnHealthChange += UpdateHealth;
    }

    private void Update()
    {
        if(!firstUpdate)
        {
            UpdateHealth(health.currentHealth, health.MaxHealth);
            firstUpdate = true;
        }
        //if (this.gameObject.transform.parent.GetComponent<Building>() == null)
        //{
        //    this.transform.LookAt(Camera.main.transform);
        //}

    }
}
