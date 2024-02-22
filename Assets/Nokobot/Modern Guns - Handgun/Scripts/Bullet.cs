using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 6 && collision.gameObject.layer != 9)
        { 
            if (collision.gameObject.TryGetComponent<HealthEnemy>(out HealthEnemy enemyHealth))
            {
                enemyHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
