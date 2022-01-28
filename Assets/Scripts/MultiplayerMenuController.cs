using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Multiplayer;
using System;
using System.Net;

public class MultiplayerMenuController : MonoBehaviour
{
    public static MultiplayerMenuController instance;

    [SerializeField] private TMP_InputField codeInputField;
    [SerializeField] private TextMeshProUGUI hostCode;
    public event Action<GameMsg> OnMessageReceived;

    private void Awake()
    {
        instance = this;
    }

    private void Opponent_OnGameEventReceived(GameMsg obj)
    {
        Debug.Log($"{obj.GetType()} received...");


        if (obj is ReadyMsg)
        {
            // TODO: ready to start...
        }
        else if (obj is MoveMsg move)
        {

        }
        else if (obj is StartMsg)
        {

        }
        else if (obj is SwitchGravityMsg)
        {

        }
        else if (obj is FireMsg)
        {

        }
        else if (obj is PlantMsg)
        {

        }
        else if (obj is JumpMsg)
        {

        }
    }

    public MultiplayerService Multiplayer = null;

    public void OnJoinBtnClicked()
    {
        Debug.Log("clicked on join btn");
        // get the entered code: codeInputField.text

        Multiplayer = new MultiplayerService();
        Multiplayer.OnGameEventReceived += Opponent_OnGameEventReceived;

        Multiplayer.Join(IPAddress.Parse(codeInputField.text));
    }

    public void OnHostBtnClicked()
    {
        Debug.Log("clicked on host btn");

        Multiplayer = new MultiplayerService();
        Multiplayer.OnGameEventReceived += Opponent_OnGameEventReceived;

        Multiplayer.Host();
    }

    // this one is called when finished editing the text for the host code
    public void OnEnteredHostCode()
    {
        Debug.Log("entered host code");
    }

    public void OnShowMultiplayerPanel()
    {
        hostCode.text = "---.---.-.--";
        codeInputField.text = "127.0.0.1";
    }

    public void OnHideMultiplayerPanel()
    {

    }

    public void OnCopyClicked()
    {
        GUIUtility.systemCopyBuffer = hostCode.text;

        //TODO
        Multiplayer.Opponent.StartGame();
    }
}
