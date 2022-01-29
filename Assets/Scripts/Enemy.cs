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
    [SerializeField] private float spawnForce = 5;

    private float lastDirectionToOpponent;
    private Rigidbody2D opponentRigidBody;
    private bool move;

    private void Start()
    {
        GameManager.GameOver += OnGameOver;
    }

    private void OnGameOver(bool topPlayer)
    {
        move = false;
    }

    public void OnRegenerate()
    {
        rigidBody.AddForce(new Vector2(0, Mathf.Sign(rigidBody.gravityScale) * 5), ForceMode2D.Impulse);

        Vector2 directionToOpponent = (opponentPlayer.transform.position - transform.position).normalized;
        transform.localScale = transform.localScale * new Vector2(1, Mathf.Sign(directionToOpponent.x));
        opponentRigidBody = opponentPlayer.GetComponentInChildren<Rigidbody2D>();
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        move = true;
        while (move)
        {
            float directionToOpponent = opponentRigidBody.transform.position.x - rigidBody.transform.position.x;
            Debug.DrawRay(rigidBody.transform.position, opponentRigidBody.transform.position);
            if(Mathf.Sign(lastDirectionToOpponent) != Mathf.Sign(directionToOpponent))
            {
                rigidBody.velocity = Vector2.zero;
            }
                lastDirectionToOpponent = directionToOpponent;
            rigidBody.velocity = new Vector2((Mathf.Sign(directionToOpponent) * moveSpeed * Time.fixedDeltaTime), rigidBody.velocity.y);

            if (Vector2.Distance(opponentRigidBody.transform.position, rigidBody.transform.position) <= detonateDistane)
            {
                Debug.Log("BOOM");
                opponentPlayer.gameObject.GetComponent<PlayableCharacter>().GetHit(damage);
                GetHit(maxHp * 10);
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
