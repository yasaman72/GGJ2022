using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [HideInInspector] public PlayableCharacter opponentPlayer;
    [SerializeField] private int damage = 2;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float detonateDistane = 2;

    private void Start()
    {
        GameManager.GameOver += OnGameOver;
    }

    private void OnGameOver(bool topPlayer)
    {
        StopAllCoroutines();
    }

    public void OnRegenerate()
    {
        Vector2 directionToOpponent = (opponentPlayer.transform.position - transform.position).normalized;
        transform.localScale = transform.localScale * new Vector2(1, Mathf.Sign(directionToOpponent.x));
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            float step = moveSpeed * Time.fixedDeltaTime;
            float targetPos =  (Vector2.MoveTowards(transform.position, opponentPlayer.transform.position, step)).x;
            rigidBody.MovePosition(new Vector2(targetPos, transform.position.y));
            //Vector2 directionToOpponent = (opponentPlayer.transform.position - transform.position).normalized;
            //rigidBody.velocity = new Vector2((Mathf.Sign(directionToOpponent.x) * moveSpeed * Time.fixedDeltaTime), rigidBody.velocity.y);

            if (Vector2.Distance(opponentPlayer.transform.position, transform.position) <= detonateDistane)
            {
                Debug.Log("BOOM");
                opponentPlayer.gameObject.GetComponent<PlayableCharacter>().GetHit(damage);
                GetHit(maxHp * 10);
            }
        }
    }
}
