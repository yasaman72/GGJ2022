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
    [Space]
    [SerializeField] private int maxCollectableSeed = 3;
    [SerializeField] private GameObject[] seedIndicators;

    private float jumpForce = 3;
    private float moveInput;
    private bool moving;
    private bool isJumping;
    private bool isFiring;
    private Vector2 lookDirection = Vector2.right;
    private Vector3 intialScale;
    private bool canMove = true;
    private float moveLeftLastValue;
    private float moveRightLastValue;
    private int currentCollectedSeed;


    private void OnEnable()
    {
        intialScale = rigidBody.transform.localScale;

        if (startFlipped)
        {
            rigidBody.gravityScale *= -1;
            lookDirection = -Vector2.right;
        }

        currentCollectedSeed = 0;
        UpdateSeedsIndicator();

        StartCoroutine(SetRotation());
    }

    bool IsGrounded()
    {
        return col.IsTouching(Mathf.Sign(rigidBody.gravityScale) <= 0 ? GameManager.instance.upGround : GameManager.instance.downground);
    }

    public bool IsFlipped()
    {
        return Mathf.Sign(rigidBody.gravityScale) <= 0 ? true : false;
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

    #region Movement
    public void MoveRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveRightLastValue = 1;
            Move(false, true);
        }
        else if (context.canceled)
        {
            moveRightLastValue = 0;

            if (moveLeftLastValue != 1)
            {
                Move(true);
            }
            else
            {
                Move(false, false);
            }
        }
    }

    public void MoveLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveLeftLastValue = 1;
            Move(false, false);
        }
        else if (context.canceled)
        {
            moveLeftLastValue = 0;

            if (moveRightLastValue != 1)
            {
                Move(true);
            }
            else
            {
                Move(false, true);
            }
        }
    }

    public void Move(bool stopMoving = false, bool movingRight = true)
    {
        moving = false;

        if (stopMoving)
        {
            moving = false;

            StartCoroutine(StopSlowly());
        }
        else
        {

            if (movingRight)
            {
                rigidBody.transform.localScale = new Vector3(intialScale.x * Mathf.Sign(rigidBody.gravityScale),
                                                             intialScale.y,
                                                             intialScale.z);
                lookDirection = Vector2.right;
                moveInput = 1;
            }
            else
            {
                rigidBody.transform.localScale = new Vector3(-intialScale.x * Mathf.Sign(rigidBody.gravityScale),
                                             intialScale.y,
                                             intialScale.z);
                lookDirection = -Vector2.right;

                moveInput = -1;
            }

            StartCoroutine(MoveProcess());
        }
    }

    IEnumerator MoveProcess()
    {
        moving = true;
        while (moving)
        {
            rigidBody.velocity = new Vector2((moveInput * moveSpeed * Time.fixedDeltaTime), rigidBody.velocity.y);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator StopSlowly()
    {
        while (rigidBody.velocity != Vector2.zero && canMove)
        {
            rigidBody.velocity = Vector3.Lerp(rigidBody.velocity, Vector2.zero, slowDownSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        rigidBody.velocity = Vector2.zero;
    }
    #endregion

    #region Fire
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
            newBullet.GetComponent<Bullet>().Fire(lookDirection, this);
            if (!automaticGun) isFiring = false;
            yield return new WaitForSeconds(fireInterval);
        }
    }
    #endregion

    #region Gravity
    public void SwitchGravity()
    {
        canMove = false;
        rigidBody.velocity = Vector2.zero;
        rigidBody.gravityScale *= -1;
        lookDirection = -lookDirection;
        StartCoroutine(SetRotation());
        StartCoroutine(WaitUntilGrounded());
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

    private IEnumerator WaitUntilGrounded()
    {
        while (!IsGrounded())
        {
            yield return new WaitForEndOfFrame();
        }
        canMove = true;
    }
    #endregion

    public void OnCollectedEnemySeed()
    {
        Debug.Log("collected");
        currentCollectedSeed = Mathf.Min(currentCollectedSeed + 1, maxCollectableSeed);
        UpdateSeedsIndicator();
    }

    private void UpdateSeedsIndicator()
    {
        for (int i = 0; i < seedIndicators.Length; i++)
        {
            seedIndicators[i].gameObject.SetActive(i < currentCollectedSeed);
        }
    }
}
