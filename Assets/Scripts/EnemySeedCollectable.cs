using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySeedCollectable : MonoBehaviour
{
    [SerializeField] private int hitToCollect;
    private int currentHit = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            currentHit++;
            if (currentHit >= hitToCollect)
            {
                other.GetComponent<Bullet>().owner.OnCollectedEnemySeed();
            }
            Destroy(other);
        }
    }
}
