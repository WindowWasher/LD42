using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeyFloorHole : MonoBehaviour {

    EnemyManager enemyManager;
    Health health;
    public bool needsUncovering = true;
	// Use this for initialization
	void Start () {
        enemyManager = GameObject.Find("ZombieSpawner").GetComponent<EnemyManager>();
        health = GetComponent<Health>();

        if(needsUncovering == false)
        {
            showHole();
        }

    }

    // Update is called once per frame
    void Update () {
        if(needsUncovering && health.currentHealth == 0)
        {
            enemyManager.uncover(this);
            needsUncovering = false;
            showHole();
        }
		
	}

    void showHole()
    {
        this.GetComponent<MeshFilter>().mesh = enemyManager.newMesh;
    }
}
