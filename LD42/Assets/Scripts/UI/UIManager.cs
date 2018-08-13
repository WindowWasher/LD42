using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Image healthBarFg;
    public GameObject player;

    public Image gameBarFg;
    public Image bonfireFg;

    public Timer gameTimer = new Timer();
    public float startTime;
    float totalTime = 440f;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Health>().OnHealthChange += UpdateHealthBar;
        GameObject.Find("BonFire").GetComponent<Health>().OnHealthChange += UpdateFireHealthBar;

        gameTimer.Start(totalTime);
        startTime = Time.time;
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateGameTimerBar();

        if (GameObject.Find("BonFire").GetComponent<Health>().currentHealth == 0)
        {
            Debug.Log("Bonfire was destroyed!");
        }
    }

    void UpdateGameTimerBar()
    {
        float percent = (gameTimer.endTime - Time.time) / totalTime;
        gameBarFg.fillAmount = 1 - percent;

        if(gameTimer.Expired())
        {
            //Debug.Log("Player Wins!");
        }
    }

    void UpdateFireHealthBar(int currentHealth, int maxHealth)
    {
        bonfireFg.fillAmount = (float)currentHealth / maxHealth;

        
    }

    void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthBarFg.fillAmount = (float)currentHealth / maxHealth;
    }


}
