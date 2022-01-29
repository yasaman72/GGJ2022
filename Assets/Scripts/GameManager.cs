using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public delegate void gravitySwitch(bool isOnOwnLand);
    public static gravitySwitch OnGravitySwitched;
    public delegate void gameOver(bool topPlayerWon);
    public static gameOver GameOver;
    public delegate void StarGame();
    public static StarGame startGame;
    public Collider2D upGround, downground;
    [SerializeField] private PlayableCharacter player1, player2;
    [SerializeField] private float minSwitchInterval = 5, maxSitchInterval = 10;

    public PlayableCharacter topPlayer, bottomPlayer;

    public static bool IsGravitySwitched;
    public static bool InGameplay;
    public static bool IsOnTheirLand;
    public static bool IsPlayingOnline;

    private void Awake()
    {
        instance = this;
    }

    public void StartGameplay(bool Online = false)
    {
        IsPlayingOnline = Online;
        InGameplay = true;
        IsOnTheirLand = true;
        StartCoroutine(SwitchGravityTimer());
        SeedsManager.instance.SpawnSeedSourceOnStart();
        if (startGame != null)
            startGame.Invoke();
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
        if (GameManager.IsPlayingOnline)
        {
            if (MultiplayerMenuController.instance.Multiplayer.Opponent.Role != Multiplayer.Roles.Player1) return;
            MultiplayerMenuController.instance.Multiplayer.Opponent.SwitchGravity();
        }

        IsOnTheirLand = !IsOnTheirLand;
        IsGravitySwitched = !IsGravitySwitched;

        player1.SwitchGravity();
        player2.SwitchGravity();

        if (OnGravitySwitched != null)
        {
            OnGravitySwitched.Invoke(IsOnTheirLand);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnAPlayerdied(PlayableCharacter diedPlayer)
    {
        OnGravitySwitched = null;
        InGameplay = false;
        if (GameOver != null)
        {
            GameOver.Invoke(diedPlayer.startFlipped);
        }
    }
}
