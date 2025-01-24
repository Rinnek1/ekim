using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARBullet : MonoBehaviour
{
    private float damage;

    public void SetDamage(float amount)
    {
        damage = amount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) return; // Prevent interacting with the player

        EHealth targetHealth = collision.gameObject.GetComponent<EHealth>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}