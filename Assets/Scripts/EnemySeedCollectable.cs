using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySeedCollectable : MonoBehaviour
{
    [SerializeField] private int hitToCollect;
    private int currentHit = 5;
    [HideInInspector] public bool isOnBottom;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            currentHit++;
            if (currentHit >= hitToCollect)
            {
                PlayableCharacter bulletOwner = other.GetComponent<Bullet>().owner;
                bulletOwner.OnCollectedEnemySeed();
                Destroy(gameObject);
                SeedsManager.instance.SpawnSeed(isOnBottom); ;
            }
            Destroy(other.gameObject);
        }
    }
}
