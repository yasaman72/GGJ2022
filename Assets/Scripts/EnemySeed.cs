using System.Collections;
using UnityEngine;

public class EnemySeed : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private SpriteRenderer mySpriteRndr;
    private GameObject mySpawnedEnemy;
    private int myEnemyCurrentHP;
    private bool alreadySpawnedEnemy;
    [HideInInspector] public bool isOnTop;

    private void Start()
    {
        GameManager.OnGravitySwitched += OnGravitySwitched;
    }

    public void OnGravitySwitched(bool isOnTheirLand)
    {
        if (isOnTheirLand)
        {
            ReturnEnemyToSeed();
        }
        else
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        mySpawnedEnemy = Instantiate(enemy, transform.position, Quaternion.identity, null);
        Rigidbody2D rb = mySpawnedEnemy.GetComponentInChildren<Rigidbody2D>();
        mySpawnedEnemy.GetComponent<Enemy>().opponentPlayer = isOnTop ?
            GameManager.instance.bottomPlayer :
            GameManager.instance.topPlayer;

        if (isOnTop)
        {
            rb.gravityScale = -rb.gravityScale;
        }

        if (!alreadySpawnedEnemy)
        {
            mySpawnedEnemy.GetComponent<Enemy>().OnSpawn();
        }
        else
        {
            mySpawnedEnemy.GetComponent<Enemy>().SetCurrentHP(myEnemyCurrentHP);
        }




        gameObject.SetActive(false);

        alreadySpawnedEnemy = true;
        mySpawnedEnemy.GetComponent<Enemy>().OnRegenerate();

    }

    public void ReturnEnemyToSeed()
    {
        if (mySpawnedEnemy == null)
        {
            OnMyEnemyDied();
            return;
        }
        myEnemyCurrentHP = mySpawnedEnemy.GetComponent<Enemy>().GetHP();
        Destroy(mySpawnedEnemy.gameObject);
        gameObject.SetActive(true);

    }

    private void OnMyEnemyDied()
    {
        GameManager.OnGravitySwitched -= OnGravitySwitched;
        Destroy(gameObject);
    }
}
