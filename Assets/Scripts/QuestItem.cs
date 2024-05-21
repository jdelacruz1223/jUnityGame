using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Optional: Check if the coin touches the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collected bomb #" + DataManager.me.BombsCollected);
            DataManager.me.BombsCollected += 1;
            Destroy(gameObject);
        }
        
    }
}
