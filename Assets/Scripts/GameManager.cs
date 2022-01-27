using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Collider2D upGround, downground;
    [SerializeField] private PlayableCharacter player1, player2;

    public static bool isGravitySwitched;

    private void Awake()
    {
        instance = this;
    }

    public void SwitchGravityDebugKey(InputAction.CallbackContext context)
    {
        SwitchGravity();
    }

    private void SwitchGravity()
    {
        isGravitySwitched = !isGravitySwitched;

        player1.SwitchGravity();
        player2.SwitchGravity();
    }
}
