using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTriggerController : MonoBehaviour
{
    public Collider2D bombCollider;

    void Start()
    {
        if(bombCollider == null) Debug.LogWarning("Bombable Door Collider not set");
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider == null || collider.gameObject.tag == "Enemy")
        return;

        //if player bomb count >= 1
        // if(DataManager.me.BombsCollected >= 1)
        // {

        // }

        //show interact button display

        //disable enemy spawning

        //update texture to destroyed door

        //reduce bomb count
    }
}
