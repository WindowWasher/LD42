using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {

    public GameObject gameOverScreen;
    public GameObject player;
    public GameObject fire;

	// Use this for initialization
	void Start () {
        gameOverScreen.SetActive(false);
        player.GetComponent<Health>().OnHealthChange += PlayerDeadListener;
	}

    private void OnDisable()
    {
        player.GetComponent<Health>().OnHealthChange -= PlayerDeadListener;
    }

    public void PlayerDeadListener(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0)
        {
            // Pause the game
            Time.timeScale = 0;

            gameOverScreen.SetActive(true);
        }
    }

    void Update()
    {
        if (fire == null)
        {
            // Pause the game
            Time.timeScale = 0;

            gameOverScreen.SetActive(true);
        }
    }

}
