using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySeed : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private SpriteRenderer mySpriteRndr;
    private GameObject mySpawnedEnemy;
    private int myEnemyCurrentHP;
    private bool alreadySpawnedEnemy;

    public void SpawnEnemy(bool isOnTop)
    {
        mySpawnedEnemy = Instantiate(enemy, transform.position, Quaternion.identity, null);
        Rigidbody2D rb = mySpawnedEnemy.GetComponent<Rigidbody2D>();
        if (!alreadySpawnedEnemy)
        {
            mySpawnedEnemy.GetComponent<Enemy>().OnSpawn();
        }
        else
        {
            mySpawnedEnemy.GetComponent<Enemy>().SetCurrentHP(myEnemyCurrentHP);
        }

        if (isOnTop)
        {
            rb.gravityScale = -rb.gravityScale;
        }

        rb.AddForce(new Vector2(0, Mathf.Sign(rb.gravityScale) * 20), ForceMode2D.Impulse);
        StartCoroutine(SetRotation(rb));
        mySpriteRndr.color = Color.clear;
        alreadySpawnedEnemy = true;

    }
    private IEnumerator SetRotation(Rigidbody2D rb)
    {
        bool rotate = true;
        while (rotate)
        {
            yield return new WaitForFixedUpdate();

            if (rb.gravityScale < 0)
            {
                rb.SetRotation(rb.rotation + (500 * Time.fixedDeltaTime));
                if (rb.rotation > 180)
                {
                    rb.rotation = 180;
                    rotate = false;
                }
            }
            else
            {
                rb.SetRotation(rb.rotation - (500 * Time.fixedDeltaTime));
                if (rb.rotation < 0)
                {
                    rb.rotation = 0;
                    rotate = false;
                }
            }
        }

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
        mySpriteRndr.color = Color.white;

    }

    private void OnMyEnemyDied()
    {
        SeedsManager.instance.OnEnemyDied(gameObject);
        Destroy(gameObject);
    }
}
