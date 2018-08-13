using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHelper : MonoBehaviour {

    public GameObject welcomeInformation;

    public void Start()
    {
        Time.timeScale = 0;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            welcomeInformation.SetActive(false);
            Time.timeScale = 1;
            //Time.timeScale = 5f;

        }
    }
}
