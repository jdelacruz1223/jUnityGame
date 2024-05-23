using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

    [SerializeField] private GameObject EnemyToSpawn;
    [SerializeField] private GameObject[] Spawnpoints;
    [SerializeField] private int maxEnemyCount = 1;
    [SerializeField] private int spawnDelay = 2;
    private int currentWaypointIndex = 0;
    private int currentEnemyCount = 0;
    

    // Update is called once per frame
    void Update()
    {
        if(currentEnemyCount < maxEnemyCount)
        {
            StartCoroutine(SpawnEnemyCoroutine());
        }
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        if(currentWaypointIndex == maxEnemyCount) currentWaypointIndex = 0;
        Instantiate(EnemyToSpawn, Spawnpoints[currentWaypointIndex].transform.position, Quaternion.identity);

        for(int i = 0; i < spawnDelay; i++)
        {
            yield return new WaitForSeconds(1); 
        }
    }

}
