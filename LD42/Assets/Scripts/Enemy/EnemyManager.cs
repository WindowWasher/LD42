﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{

    //public float waveSpawnRadius;
    //public Vector3 center;

    public Mesh newMesh;

    float interval = 60f;
    //int numberOfSpawnLocations = 3;
    public float localSpawnRadius;

    public List<GameObject> spawnPoints;
    public List<GameObject> spawnPointsToUncover  = new List<GameObject>();

    Timer intervalTimer = new Timer();

    public List<GameObject> enemyPrefabs;

    //GameObject waveCountPanel;

    int waveNumber = 1;
    //int numPerWave = 5;
    int lastWaveCount = 0;
    bool waveRunning = false;

    int enemyTotalSpawned = 0;

    public Dictionary<string, List<GameObject>> enemyObjectPool = new Dictionary<string, List<GameObject>>();

    GameObject CreateEnemy(GameObject enemyPrefab, Vector3 position)
    {
        enemyTotalSpawned += 1;
        //Debug.Log("Enemeies: " + enemyTotalSpawned.ToString());
        GameObject enemy;
        NavMeshHit hit = new NavMeshHit();
        if (NavMesh.SamplePosition(position, out hit, 10f, NavMesh.AllAreas))
        {
            position = hit.position;
        }
        else
        {
            //var a = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //a.transform.position = position;
            return null;
        }
        string name = enemyPrefab.GetComponent<Enemy>().name;
        if (enemyObjectPool[name].Count > 0)
        {
            enemy = enemyObjectPool[name].First();
            enemyObjectPool[name].RemoveAt(0);

        }
        else
        {
            enemy = GameObject.Instantiate(enemyPrefab, position, Quaternion.identity);
        }
        enemy.SetActive(true);
        enemy.transform.position = position;
        //enemy.GetComponent<Enemy>().Init();

        return enemy;
    }

    // Use this for initialization
    void Start()
    {


        //enemyPrefabs = new List<GameObject>();

        //enemyPrefabs.Add((GameObject)Resources.Load("Enemies/Barbarian"));

        //intervalTimer.Start(10f);
        enemyObjectPool["MaleZombiePrefab"] = new List<GameObject>();
        //enemyObjectPool["Archer"] = new List<GameObject>();
        //waveCountPanel = GameObject.Find("WaveCountPanel");

        spawnPointsToUncover = GameObject.FindGameObjectsWithTag("ZombieSpawner").ToList();
        foreach(GameObject obj in spawnPoints)
        {
            if(spawnPointsToUncover.Contains(obj))
            {
                Debug.Log("Removed spawn point");
                spawnPointsToUncover.Remove(obj);
            }
        }

        //spawnPoints = GameObject.FindGameObjectsWithTag("ZombieSpawner");

        SpawnWave();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Wave: " + (waveNumber-1));
        if (intervalTimer.Expired() && !waveRunning)
        {
            SpawnWave();
        }

    }

    void PlayerLose()
    {
        //throw new KeyNotFoundException("You lost!");
    }

    public void uncover(EnemeyFloorHole newHole)
    {
        spawnPoints.RemoveAt(0);
        spawnPoints.Add(newHole.gameObject);
        spawnPointsToUncover.Remove(newHole.gameObject);
    }

    void SpawnWave()
    {
        //List<GameObject> currentSpawnPoints = new List<GameObject>();
        //foreach(var spawnP in spawnPoints)
        //{
        //    currentSpawnPoints.Add(spawnP);
        //}
        waveRunning = true;

        List<int> rangeValues = new List<int>();
        for (int i = 0; i < spawnPoints.Count; ++i)
        {
            rangeValues.Add(i);
        }

         int numberOfSpawnLocations = spawnPoints.Count;
        //int numberOfSpawnLocations = spawnPoints.Length;
        List<int> randomIndexes = new List<int>();
        for (int i = 0; i < numberOfSpawnLocations; i++)
        {
            int index = Random.Range(0, rangeValues.Count);
            randomIndexes.Add(rangeValues[index]);
            rangeValues.RemoveAt(index);
        }

        //int numberOfEnemiesToSpawn = 200* 5 + numPerWave * waveNumber;
        int numberOfEnemiesToSpawn = lastWaveCount + waveNumber * 2;
        numberOfEnemiesToSpawn = waveNumber * 10;
        //if (waveNumber == 1)
        //{
        //    //numberOfSpawnLocations = 2;
        //    numberOfEnemiesToSpawn = 10;
        //}
        //else if (waveNumber == 3)
        //{
        //    numberOfEnemiesToSpawn = 30;
        //}
        //else if (waveNumber == 5)
        //{
        //    numberOfEnemiesToSpawn = 60;
        //}
        //else if (waveNumber == 6)
        //{
        //    numberOfEnemiesToSpawn = 
        //}
        //else if (waveNumber == 7)
        //{
        //    numberOfEnemiesToSpawn = 100;
        //}
        //else if (waveNumber >= 8)
        //{
        //    numberOfEnemiesToSpawn = 0;
        //}
        //else if (waveNumber == 10)
        //{
        //    numberOfEnemiesToSpawn = 500;
        //}
        lastWaveCount = numberOfEnemiesToSpawn;
        int numPerWave = numberOfEnemiesToSpawn / numberOfSpawnLocations;
        //int numberOfEnemiesToSpawn = 20000 + 5 * waveNumber;
        numPerWave += waveNumber; //Add some addtiona scaling
        numPerWave += waveNumber / 10 * 10;
        int enemiesPerWave = numberOfEnemiesToSpawn / numberOfSpawnLocations;
        //foreach(var spawnP in currentSpawnPoints)
        List<GameObject> selectedSpawnPoints = new List<GameObject>();
        for (int i = 0; i < randomIndexes.Count; ++i)
        {
            var spawnP = spawnPoints[randomIndexes[i]];
            selectedSpawnPoints.Add(spawnP);
            StartCoroutine(SpawnEnemiesAtPosition(i, enemiesPerWave));

        }
        //StartCoroutine(DoSpawning(selectedSpawnPoints, enemiesPerWave));

        intervalTimer.Start(interval);
        //waveCountPanel.GetComponentInChildren<Text>().text = "Wave: " + waveNumber.ToString();
        waveNumber += 1;
    }

    //IEnumerator DoSpawning(List<GameObject> spawnPoints, int enemiesPerWave)
    //{
    //    Debug.Log("Enemies per wave" + enemiesPerWave.ToString());
    //    foreach (GameObject spawnPoint in spawnPoints)
    //    {
    //        Vector2 localSpawnPoint = new Vector2(spawnPoint.transform.position.x, spawnPoint.transform.position.z);

    //        for (int i = 0; i < enemiesPerWave; ++i)
    //        {
    //            //if (i % 2 == 0)
    //            {
    //                yield return new WaitForSeconds(5f);
    //            }

    //            GameObject newEnemyPrefab;
    //            if (Random.Range(0, 2) == 1)
    //            {
    //                newEnemyPrefab = enemyPrefabs[1];
    //            }
    //            else
    //            {
    //                newEnemyPrefab = enemyPrefabs[0];
    //            }

    //            Vector2 enemySpawnPointV2 = (Random.insideUnitCircle.normalized * localSpawnRadius) + localSpawnPoint;
    //            Vector3 enemySpawnPoint = new Vector3(enemySpawnPointV2.x, spawnPoint.transform.position.y, enemySpawnPointV2.y);

    //            GameObject.Instantiate(newEnemyPrefab, enemySpawnPoint, Quaternion.identity);

    //        }
    //    }

    //}



    IEnumerator SpawnEnemiesAtPosition(int index, int numberOfEnemiesToSpawn)
    {
        //Vector2 localSpawnPoint = Random.insideUnitCircle.normalized * waveSpawnRadius;
        GameObject spawnObj = spawnPoints[index];
        Vector3 spawnPoint = spawnObj.transform.position;
        Vector2 localSpawnPoint = new Vector2(spawnPoint.x, spawnPoint.z);

        float enemiesToSpawnPerSecond = Mathf.Max(interval/ numberOfEnemiesToSpawn, 0.5f);

        for (int i = 0; i < numberOfEnemiesToSpawn; ++i)
        {
            //if (i % 2 == 0)
            //{
            Debug.Log("Waiting " + enemiesToSpawnPerSecond + " " + numberOfEnemiesToSpawn + " " + interval);
            yield return new WaitForSeconds(enemiesToSpawnPerSecond);
            //}

            if(spawnPoints[index] != spawnObj)
            {
                 spawnObj = spawnPoints[index];
                 spawnPoint = spawnObj.transform.position;
                 localSpawnPoint = new Vector2(spawnPoint.x, spawnPoint.z);
            }

            GameObject newEnemyPrefab = enemyPrefabs[0];
            //if (Random.Range(0, 3) == 1 && waveNumber > 2)
            //{
            //    newEnemyPrefab = enemyPrefabs[1];
            //}
            //else
            //{
            //    newEnemyPrefab = enemyPrefabs[0];
            //}

            Vector2 enemySpawnPointV2 = (Random.insideUnitCircle.normalized * localSpawnRadius) + localSpawnPoint;
            Vector3 enemySpawnPoint = new Vector3(enemySpawnPointV2.x, spawnPoint.y, enemySpawnPointV2.y);

            //GameObject.Instantiate(newEnemyPrefab, enemySpawnPoint, Quaternion.identity);
            this.CreateEnemy(newEnemyPrefab, enemySpawnPoint);

        }

        waveRunning = false;

    }

    //private void OnDrawGizmos()
    //{
    //    //UnityEditor.Handles.color = Color.green;
    //    //UnityEditor.Handles.DrawWireDisc(center, Vector3.up, waveSpawnRadius);
    //    //UnityEditor.Handles.DrawWireDisc(collider.transform.position, Vector3.back, collider.radius);
    //}

    //public static EnemyManager Instance()
    //{
    //    return GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
    //}

    //public List<Enemy> GetAllEnemies()
    //{
    //    List<Enemy> enemies = new List<Enemy>();
    //    foreach (var enemyObj in GameObject.FindGameObjectsWithTag("Enemy"))
    //    {
    //        if (enemyObj == null || enemyObj.GetComponent<Enemy>().alive == false)
    //            continue;
    //        enemies.Add(enemyObj.GetComponent<Enemy>());
    //    }

    //    return enemies;
    //}
}
