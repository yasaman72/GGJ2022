using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayableCharacter : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private float jumpForce = 500;
    [SerializeField] private float moveForce = 100;

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Jump!");
            rigidBody.AddForce(new Vector2(0, rigidBody.gravityScale * jumpForce));
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().x > 0)
        {
            Debug.Log("moving!");
            rigidBody.AddForce(new Vector2(rigidBody.gravityScale * moveForce, 0));
        } else if (context.ReadValue<Vector2>().x < 0)
        {
            Debug.Log("moving!");
            rigidBody.AddForce(new Vector2(rigidBody.gravityScale * -moveForce, 0));
        }
    }
}
