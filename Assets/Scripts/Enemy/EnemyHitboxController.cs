using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitboxController : MonoBehaviour
{
    public Collider2D attackCollider;
    [SerializeField] public float attackDamage = 1f;
    [SerializeField] public float knockbackForce = 1f;
    [SerializeField] public float knockbackDuration = 1f;
    [SerializeField] public float maxKnockbackDistance = 1f;

    
    void Start()
    {
        if(attackCollider == null) Debug.LogWarning("Enemy Attack Collider not set");
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider == null || collider.gameObject.tag != "Player") return;

        Debug.Log("Knockback Player");

        Rigidbody2D playerRb = collider.GetComponent<Rigidbody2D>();

        if (playerRb != null)
        {
            Vector2 direction = (collider.transform.position - transform.position).normalized;
            Vector2 knockbackForceVector = direction * knockbackForce;
            playerRb.velocity = Vector2.zero;
            playerRb.AddForce(knockbackForceVector, ForceMode2D.Impulse);
        }

        StartCoroutine
        (
            ApplyKnockback
            (
                collider.gameObject, 
                collider.transform.position, 
                knockbackDuration
            )
        );

        //collider.SendMessage("OnHit", attackDamage);
        DataManager.me.lifeCount--;
    }

    IEnumerator ApplyKnockback(GameObject target, Vector2 targetPosition, float duration)
    {
        yield return new WaitForSeconds(duration);
        Rigidbody2D enemyRigidbody = target.GetComponent<Rigidbody2D>();
        if (enemyRigidbody != null)
        {
            enemyRigidbody.velocity = Vector2.zero; // Stop the enemy after knockback
        }
    }

    
}
