using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : SingletonGenerics<GameManager>
{
    [SerializeField] private GameObject GameOver;
    [SerializeField] private Text playerWin;
    public Text player1Score, player2Score;

    internal bool IsGamePaused = false;
    [SerializeField] private GameObject pauseMenuPanel;

    private void Start()
    {
        if (GameOver != null) GameOver.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsGamePaused)
            {
                Resume();
                return;
            }
            Pause();
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    private void Pause()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        IsGamePaused = true;
    }

    public void Resume()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameOver.SetActive(false);
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GameQuit()
    {
        Application.Quit();
    }
    public void PlayerWin(string name)
    {
        GameOver.SetActive(true);
        playerWin.text = name;
        Time.timeScale = 0f;
    }
}