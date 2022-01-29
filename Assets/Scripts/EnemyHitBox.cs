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
            if (other.GetComponent<Bullet>().owner != enemy.opponentPlayer) return;

            Destroy(other.gameObject);
            enemy.GetHit(1);
        }

    }
}
