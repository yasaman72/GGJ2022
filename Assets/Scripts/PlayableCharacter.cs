using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayableCharacter : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private float jumpForce = 500;
    [SerializeField] private float moveSpeed = 500;
    [SerializeField] private float slowDownSpeed =100;
    [Space]
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bullet;

    Vector2 moveInput;
    bool moving;

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rigidBody.AddForce(new Vector2(0, rigidBody.gravityScale * jumpForce));
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("move!");
            moveInput.x = context.ReadValue<Vector2>().x;
            moving = true;
            StartCoroutine(Move());
        }
        else if (context.canceled)
        {
            moving = false;
            //rigidBody.velocity = Vector2.zero;
            StartCoroutine(StopSlowly());
        }
    }

    IEnumerator Move()
    {
        while (moving)
        {
            rigidBody.velocity = moveInput * moveSpeed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator StopSlowly()
    {
        while(rigidBody.velocity != Vector2.zero)
        {
            rigidBody.velocity = Vector3.Lerp(rigidBody.velocity, Vector2.zero, slowDownSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();

        }
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Fire!");
            Instantiate(bullet, bulletSpawnPoint.transform.position, Quaternion.identity, null);
        }
    }
}
