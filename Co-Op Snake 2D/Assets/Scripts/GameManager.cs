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

    private void Start()
    {
        if(GameOver !=null)
        GameOver.SetActive(false);
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
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameOver.SetActive(false);
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
