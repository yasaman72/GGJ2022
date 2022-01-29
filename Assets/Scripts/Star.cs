using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private int healAmount = 2;
    [SerializeField] private float moveSpeed = 5;
    private int direction = 1;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("heal");
        if (other.gameObject.layer == 6)
        {
            other.GetComponentInParent<PlayableCharacter>().Heal(healAmount);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(Mathf.Abs(transform.position.x) > 7)
        {
            direction = -direction;
        }
        transform.Translate(Vector3.left * moveSpeed * direction * Time.deltaTime);
    }
}
