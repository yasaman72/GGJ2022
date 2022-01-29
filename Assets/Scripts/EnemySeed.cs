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
        Rigidbody2D rb = mySpawnedEnemy.GetComponent<Rigidbody2D>();
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




        rb.AddForce(new Vector2(0, Mathf.Sign(rb.gravityScale) * 20), ForceMode2D.Impulse);
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
