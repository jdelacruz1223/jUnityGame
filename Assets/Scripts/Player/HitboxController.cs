using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HitboxController : MonoBehaviour
{
    public Collider2D swordCollider;
    public float swordDamage = 1f;
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.5f;
    public float maxKnockbackDistance = 2f;
    

    void Start()
    {
        if(swordCollider == null) Debug.LogWarning("Sword Collider not set");
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider == null || collider.gameObject.tag != "Enemy") return;

        Debug.Log("Knockback enemy");

        Rigidbody2D enemyRigidbody = collider.GetComponent<Rigidbody2D>();

        if (enemyRigidbody != null)
        {
            Vector2 direction = (collider.transform.position - transform.position).normalized;
            Vector2 knockbackForceVector = direction * knockbackForce;
            enemyRigidbody.velocity = Vector2.zero;
            enemyRigidbody.AddForce(knockbackForceVector, ForceMode2D.Impulse);
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

        collider.SendMessage("OnHit", swordDamage);
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
