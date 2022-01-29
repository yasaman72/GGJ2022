using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private MenuPanel mainMenu;
    [SerializeField] private MenuPanel multiplayerMenu;
    [SerializeField] private MenuPanel controlsMenu;
    [SerializeField] private MenuPanel inGame;
    [SerializeField] private MenuPanel gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameoverText;

    MenuPanel currentPanel;

    private void Start()
    {
        currentPanel = mainMenu;
        OpenMainMenu();
        multiplayerMenu.Hide();
        controlsMenu.Hide();
        inGame.Hide();
        gameOverPanel.Hide();

        GameManager.GameOver += OnGameOver;
    }

    private void OnGameOver(bool topPlayerWon)
    {
        if (topPlayerWon)
        {
            gameoverText.text = "Player 2 Won!";

        }
        else
        {
            gameoverText.text = "Player 1 Won!";

        }
        OpenAPanel(gameOverPanel);
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
