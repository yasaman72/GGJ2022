using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayableCharacter : MonoBehaviour
{
    [SerializeField] private bool startFlipped;
    [Space]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Collider2D col;
    [SerializeField] private Transform visuals;
    [SerializeField] private float moveSpeed = 500;
    [SerializeField] private float slowDownSpeed = 100;
    [SerializeField] private float rotateSpeed = 5;
    [SerializeField] private float maxAirTime = 2;
    [Space]
    [SerializeField] private float jumpHeight = 5;
    [SerializeField] private float gradualJumpForce = 1;
    [SerializeField] private float maxJumpVelocity = 10;
    [Space]
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bullet;
    [Space]
    [SerializeField] private bool automaticGun = true;
    [SerializeField] private float fireInterval = 0.5f;

    private float jumpForce = 3;
    private Vector2 moveInput;
    private bool moving;
    private bool isJumping;
    private bool isFiring;
    private Vector2 lookDirection;

    private void OnEnable()
    {
        if (startFlipped)
            rigidBody.gravityScale *= -1;

        StartCoroutine(SetRotation());
    }

    bool IsGrounded()
    {
        return col.IsTouching(startFlipped ? GameManager.instance.upGround : GameManager.instance.downground);
    }


    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (IsGrounded())
            {
                jumpForce = CalculateJumpForce(Physics2D.gravity.magnitude, jumpHeight);

                rigidBody.AddForce(new Vector2(0, Mathf.Sign(rigidBody.gravityScale) * jumpForce), ForceMode2D.Impulse);
                StartCoroutine(JumpingProcess());
            }
        }
        else if (context.canceled)
        {
            isJumping = false;
        }
    }

    private float CalculateJumpForce(float gravityStrength, float jumpHeight)
    {
        //h = v^2/2g
        //2gh = v^2
        //sqrt(2gh) = v
        return Mathf.Sqrt(2 * gravityStrength * jumpHeight);
    }

    private IEnumerator JumpingProcess()
    {
        isJumping = true;
        float passedTime = 0;

        while (isJumping)
        {
            yield return new WaitForEndOfFrame();
            passedTime += Time.deltaTime;
            Debug.Log("before added velocity " + rigidBody.velocity);
            rigidBody.AddForce(new Vector2(0, Mathf.Sign(rigidBody.gravityScale) * gradualJumpForce * Time.fixedDeltaTime), ForceMode2D.Force);
            Debug.Log("added velocity " + rigidBody.velocity);
            if (passedTime >= maxAirTime ||
                Mathf.Sign(rigidBody.velocity.y) != Mathf.Sign(rigidBody.gravityScale) ||
                rigidBody.velocity.y >= maxJumpVelocity)
            {
                Debug.Log("end jump");
                rigidBody.AddForce(new Vector2(0, -Mathf.Sign(rigidBody.gravityScale) * jumpForce / 10f), ForceMode2D.Impulse);

                isJumping = false;
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        Debug.LogError("here again");
        if (context.phase == InputActionPhase.Started)
        {
            moving = false;

            moveInput.x = context.ReadValue<Vector2>().x;
            if (moveInput.x > 0)
            {
                visuals.transform.eulerAngles = new Vector2(0, 0);
                lookDirection = Vector2.right;
            }
            else
            {
                visuals.transform.eulerAngles = new Vector2(0, 180);
                lookDirection = -Vector2.right;
            }
            Debug.Log(moveInput);
            StartCoroutine(Move());
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moving = false;
            StartCoroutine(StopSlowly());
        }
    }

    IEnumerator Move()
    {
        moving = true;
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
            StartCoroutine(KeepFiringGun());
        }
        if (context.canceled)
        {
            isFiring = false;
        }
    }

    private IEnumerator KeepFiringGun()
    {
        isFiring = true;

        while (isFiring)
        {
            GameObject newBullet = Instantiate(bullet, bulletSpawnPoint.transform.position, Quaternion.identity, null);
            newBullet.GetComponent<Bullet>().Fire(lookDirection);
            if (!automaticGun) isFiring = false;
            yield return new WaitForSeconds(fireInterval);
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
