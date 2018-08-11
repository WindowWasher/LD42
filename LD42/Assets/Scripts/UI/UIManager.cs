using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Image healthBarFg;
    public GameObject player;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Health>().OnHealthChange += UpdateHealthBar;
    }
	
	// Update is called once per frame
	void Update () {
    }

    void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthBarFg.fillAmount = (float)currentHealth / maxHealth;
    }
}
