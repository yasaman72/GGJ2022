using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private MenuPanel mainMenu; 
    [SerializeField] private MenuPanel multiplayerMenu; 
    [SerializeField] private MenuPanel controlsMenu;

    MenuPanel currentPanel;

    private void Start()
    {
        currentPanel = mainMenu;
        OpenMainMenu();
        multiplayerMenu.Hide();
        controlsMenu.Hide();
    }
    public void StartGame()
    {
        mainMenu.Hide();
        GameManager.instance.StartGameplay();
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
