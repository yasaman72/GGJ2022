using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayableCharacter : MonoBehaviour
{
    [SerializeField] private bool startFlipped;
    [Space]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Collider2D collider;
    [SerializeField] private float jumpForce = 500;
    [SerializeField] private float moveSpeed = 500;
    [SerializeField] private float slowDownSpeed = 100;
    [SerializeField] private float rotateSpeed = 5;
    [Space]
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bullet;

    Vector2 moveInput;
    bool moving;
    bool isJumping;

    private void OnEnable()
    {
        if (startFlipped)
            rigidBody.gravityScale *= -1;

        StartCoroutine(SetRotation());
    }

    bool IsGrounded()
    {
        return collider.IsTouching(startFlipped ? GameManager.instance.upGround : GameManager.instance.downground);
    }


    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (IsGrounded())
            {
                rigidBody.AddForce( Vector2.up * Mathf.Sign(rigidBody.gravityScale) * jumpForce);
                isJumping = true;
                StartCoroutine(JumpingProcess());
            }
        }
        else if (context.canceled)
        {
            isJumping = false;
        }
    }

    private IEnumerator JumpingProcess()
    {
        yield return new WaitForSeconds(0.2f);
        if (IsGrounded())
        {
            isJumping = false;
        }
        if (isJumping)
        {
            rigidBody.AddForce(Vector2.up * Mathf.Sign(rigidBody.gravityScale) * jumpForce);
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
            rigidBody.velocity = new Vector2((moveInput * moveSpeed * Time.fixedDeltaTime).x, rigidBody.velocity.y);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator StopSlowly()
    {
        while (rigidBody.velocity != Vector2.zero)
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

    public void SwitchGravity()
    {
        rigidBody.gravityScale *= -1;
        StartCoroutine(SetRotation());
    }

    private IEnumerator SetRotation()
    {
        bool rotate = true;
        while (rotate)
        {
            yield return new WaitForFixedUpdate();

            if (rigidBody.gravityScale < 0)
            {
                rigidBody.SetRotation(rigidBody.rotation + (rotateSpeed * Time.fixedDeltaTime));
                if (rigidBody.rotation > 180)
                {
                    rigidBody.rotation = 180;
                    rotate = false;
                }
            }
            else
            {
                rigidBody.SetRotation(rigidBody.rotation - (rotateSpeed * Time.fixedDeltaTime));
                if (rigidBody.rotation < 0)
                {
                    rigidBody.rotation = 0;
                    rotate = false;
                }
            }
        }

    }
}
