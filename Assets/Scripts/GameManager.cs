using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public delegate void gravitySwitch();
    public static gravitySwitch OnGravitySwitched;
    public Collider2D upGround, downground;
    [SerializeField] private PlayableCharacter player1, player2;
    [SerializeField] private float minSwitchInterval = 5, maxSitchInterval = 10;

    public static bool IsGravitySwitched;
    public static bool InGameplay;

    private void Awake()
    {
        instance = this;
    }

    public void StartGameplay()
    {
        InGameplay = true;
        StartCoroutine(SwitchGravityTimer());
        SeedsManager.instance.SpawnSeedSourceOnStart();
    }

    private IEnumerator SwitchGravityTimer()
    {
        while (InGameplay)
        {
            yield return new WaitForSeconds(Random.Range(minSwitchInterval, maxSitchInterval));
            SwitchGravity();
        }
    }

    public void SwitchGravityDebugKey(InputAction.CallbackContext context)
    {
        SwitchGravity();
    }

    private void SwitchGravity()
    {
        if (MultiplayerMenuController.instance.Multiplayer.Opponent.Role != Multiplayer.Roles.Player1) return;
        MultiplayerMenuController.instance.Multiplayer.Opponent.SwitchGravity();

        IsGravitySwitched = !IsGravitySwitched;

        player1.SwitchGravity();
        player2.SwitchGravity();

        if (OnGravitySwitched != null)
        {
            OnGravitySwitched.Invoke();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
