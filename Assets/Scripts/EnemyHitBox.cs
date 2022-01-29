using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            enemy.GetHit(1);
        }

    }
}
