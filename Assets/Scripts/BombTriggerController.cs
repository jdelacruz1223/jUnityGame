using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombTriggerController : MonoBehaviour
{
    public Collider2D bombCollider;
    public EnemySpawn spawner;
    private bool spawnDestroyed;

    void Start()
    {
        spawner = GetComponentInParent<EnemySpawn>();
        if(spawner == null)
        {
            Debug.LogWarning("EnemySpawn component not found in parent GameObject.");
        }
        if(bombCollider == null) Debug.LogWarning("Bombable Door Collider not set");
    }

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider == null || collider.gameObject.tag == "Enemy")
        return;

        if(spawnDestroyed) return;

        if(collider.gameObject.CompareTag("Player") && DataManager.me.BombsCollected > 0)
        {
            DataManager.me.BombsCollected--;
            spawner.currentState = EnemySpawn.SpawnState.destroyed;
            spawnDestroyed = true;
            DataManager.me.destroyedDoors++;
            Destroy(bombCollider);
        }

    }
}
