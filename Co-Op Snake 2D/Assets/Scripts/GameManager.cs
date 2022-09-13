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
        GameOver.SetActive(false);
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameOver.SetActive(false);
        Time.timeScale = 1f;
    }

    public void PlayerWin(string name)
    {
        GameOver.SetActive(true);
        playerWin.text = name;
        Time.timeScale = 0f;
    }
}
