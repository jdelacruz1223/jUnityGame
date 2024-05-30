using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public enum SpawnState
    {
        active,
        destroyed
    }
    public SpawnState currentState;
    [SerializeField] private GameObject[] EnemyToSpawn;
    [SerializeField] private GameObject[] Spawnpoints;
    [SerializeField] private int maxEnemyCount = 4;
    [SerializeField] private int spawnDelay = 2;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private int currentWaypointIndex = 0;
    private bool isSpawning = false;
    public SpriteRenderer sprite;
    public Sprite destroyedSprite;

    void Start()
    {
        currentState = SpawnState.active;
        if(sprite == null)
        {
            sprite = GetComponent<SpriteRenderer>();
        }
        if(sprite == null)
        {
            Debug.LogWarning("SpriteRenderer component not found");
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        if(currentState == SpawnState.destroyed && sprite != null) 
        {
            //Debug.Log("Destroyed");
            sprite.sprite = destroyedSprite;
            return;
        }

        activeEnemies.RemoveAll(enemy => enemy == null);

        if (activeEnemies.Count < maxEnemyCount && !isSpawning)
        {
            StartCoroutine(SpawnEnemyCoroutine());
        }
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        isSpawning = true;

        if (currentWaypointIndex == Spawnpoints.Length)
        {
            currentWaypointIndex = 0;
        }

        GameObject newEnemy = Instantiate
        (
            EnemyToSpawn[currentWaypointIndex], 
            Spawnpoints[currentWaypointIndex].transform.position, 
            Quaternion.identity
        );
        
        activeEnemies.Add(newEnemy);
        currentWaypointIndex++;

        yield return new WaitForSeconds(spawnDelay);
        isSpawning = false; 
        
    }

}
