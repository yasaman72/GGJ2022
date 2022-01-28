using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private MenuPanel mainMenu; 
    [SerializeField] private MenuPanel multiplayerMenu; 
    [SerializeField] private MenuPanel controlsMenu;
    [SerializeField] private MenuPanel inGame;

    MenuPanel currentPanel;

    private void Start()
    {
        currentPanel = mainMenu;
        OpenMainMenu();
        multiplayerMenu.Hide();
        controlsMenu.Hide();
        inGame.Hide();
    }
    public void StartGame()
    {
        GameManager.instance.StartGameplay();
        OpenAPanel(inGame);
    }

    public void OpenMainMenu()
    {
        OpenAPanel(mainMenu);
    }

    public void OpenMultiplayerMenu()
    {
        OpenAPanel(multiplayerMenu);
    }

    public void OpenControls()
    {
        OpenAPanel(controlsMenu);
    }

    public void OpenAPanel(MenuPanel panel)
    {
        currentPanel.Hide();
        panel.Show();
        currentPanel = panel;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
