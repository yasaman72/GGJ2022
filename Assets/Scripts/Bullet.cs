using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private float speed = 1000;
    [SerializeField] private float life = 5;

    public void Fire(Vector2 direction)
    {
        rb2D.AddForce(direction * speed);
        StartCoroutine(EndLife());
    }

    IEnumerator EndLife()
    {
        yield return new WaitForSeconds(life);
        Destroy(gameObject);
    }
}
