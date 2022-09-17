using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private ScreenBounds screenBounds;
    string player1, player2;

    private Vector2 snakeDirection;
    private bool up, left, down, right;
    private bool upArrow, leftArrow, downArrow, rightArrow;
    private List<Transform> snakeBodyExpand = new List<Transform>();


    [SerializeField] private Transform snakeBodyExpandPrefab;
    [SerializeField] private int snakeInitialSize = 5;

    [SerializeField] private float snakeSpeed = 20f;
    [SerializeField] private float snakeSpeedMultiplier = 1f;

    private float nextUpdate;
    int currentScore = 0;

    bool isShieldActive;
    bool isScoreBoost;
    bool isSpeedBoost;
    private void Start()
    {
        player1 = "Snake1";
        player2 = "Snake2";
        ResetGame();
        InitialMovement();
    }

    private void InitialMovement()
    {
        if (name == player1) snakeDirection = Vector2.up;
        if (name == player2) snakeDirection = Vector2.left;
        GameManager.Instance.player1Score.text = "Player 1 Score: " + currentScore;
        GameManager.Instance.player2Score.text = "Player 2 Score: " + currentScore;
    }

    private void Update()
    {
        HandleInputs();
        Moves();
    }

    private void Moves()
    {
        if (name == player1) HandleSnakeMovements(up, down, left, right);
        if (name == player2) HandleSnakeMovements(upArrow, downArrow, leftArrow, rightArrow);
    }

    private void FixedUpdate()
    {
        MovementControl();
    }

    private void HandleInputs()
    {
        up = Input.GetKeyDown(KeyCode.W);
        left = Input.GetKeyDown(KeyCode.A);
        down = Input.GetKeyDown(KeyCode.S);
        right = Input.GetKeyDown(KeyCode.D);

        upArrow = Input.GetKeyDown(KeyCode.UpArrow);
        leftArrow = Input.GetKeyDown(KeyCode.LeftArrow);
        downArrow = Input.GetKeyDown(KeyCode.DownArrow);
        rightArrow = Input.GetKeyDown(KeyCode.RightArrow);
    }

    private void HandleSnakeMovements(bool up, bool down, bool left, bool right)
    {
        if (GameManager.Instance.IsGamePaused) return;

        if (up && snakeDirection != Vector2.down)
        {
            snakeDirection = Vector2.up;
        }

        else if (left && snakeDirection != Vector2.right)
        {
            snakeDirection = Vector2.left;
        }
        else if (down && snakeDirection != Vector2.up)
        {
            snakeDirection = Vector2.down;
        }
        else if (right && snakeDirection != Vector2.left)
        {
            snakeDirection = Vector2.right;
        }
    }
    private void MovementControl()
    {
        if (Time.time < nextUpdate) return;
        
        for (int i = snakeBodyExpand.Count - 1; i > 0; i--)
        {
            snakeBodyExpand[i].position = snakeBodyExpand[i - 1].position;
        }

        Vector2 tempPosition;
        tempPosition = new Vector2(Mathf.Round(this.transform.position.x) + snakeDirection.x, Mathf.Round(this.transform.position.y) + snakeDirection.y);

        if (screenBounds.AmIOutOfBounds(tempPosition))
        {
            Vector2 newPosition = screenBounds.CalculateWrappedPosition(tempPosition);
            transform.position = newPosition;
            return;
        }
        transform.position = tempPosition;

        if(isSpeedBoost)
        {
            nextUpdate = Time.time + (1f / (snakeSpeed * snakeSpeedMultiplier * 2));
            return;
        }
        nextUpdate = Time.time + (1f / (snakeSpeed * snakeSpeedMultiplier));
    }

    public void SnakeExpand()
    {
        Transform newSnakeBodyExpand = Instantiate(snakeBodyExpandPrefab);
        newSnakeBodyExpand.position = snakeBodyExpand[snakeBodyExpand.Count - 1].position;
        snakeBodyExpand.Add(newSnakeBodyExpand);
        if (gameObject.name == player1)
        {
            if (isScoreBoost) currentScore += 50;
            currentScore += 100;
            GameManager.Instance.player1Score.text = "Player 1 Score: " + currentScore;
        }
        else if (gameObject.name == player2)
        {
            if (isScoreBoost) currentScore += 50;
            currentScore += 100;
            GameManager.Instance.player2Score.text = "Player 2 Score: " + currentScore;
        }
    }
    public void SnakeShrink()
    {
        if (isShieldActive) return;
        if (snakeBodyExpand.Count == 1)
        {
            if (gameObject.name == player1)
            {
                GameManager.Instance.PlayerWin("Player 2 wins");
            }
            else if (gameObject.name == player2)
            {
                GameManager.Instance.PlayerWin("Player 1 wins");
            }
        }
        else
        {
            Destroy(snakeBodyExpand[snakeBodyExpand.Count - 1].gameObject);
            snakeBodyExpand.RemoveAt(this.snakeBodyExpand.Count - 1);
            if (gameObject.name == player1)
            {
                currentScore -= 100;
                if (currentScore <= 0) currentScore = 0;
                GameManager.Instance.player1Score.text = "Player 1 Score: " + currentScore;

            }
            else if (gameObject.name == player2)
            {
                currentScore -= 100;
                if(currentScore <= 0) currentScore = 0;
                GameManager.Instance.player2Score.text = "Player 2 Score: " + currentScore;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hit")
        {
            if (gameObject.name == player1 && !isShieldActive)
            {
                GameManager.Instance.PlayerWin("Player 2 wins");
            }
            else if (gameObject.name == player2 && !isShieldActive)
            {
                GameManager.Instance.PlayerWin("Player 1 wins");
            }
        }

        if (collision.tag == "FOOD")
        {
            SnakeExpand();
        }

        if (collision.tag == "POISON")
        {
            SnakeShrink();
        }

        if (collision.tag == "SHIELD")
        {
            StartCoroutine(ActivateShield());
        }

        if (collision.tag == "SCORE BOOST")
        {
            StartCoroutine(ScoreBoost());
        }
        if (collision.tag == "SPEED")
        {
            StartCoroutine(SpeedBoost());
        }
    }

    public void ResetGame()
    {
        for (int i = 1; i < snakeBodyExpand.Count; i++)
        {
            Destroy(snakeBodyExpand[i].gameObject);
        }
        transform.position = Vector2.one;
        snakeBodyExpand.Clear();
        snakeBodyExpand.Add(transform);

        for (int i = 1; i < this.snakeInitialSize; i++)
        {
            snakeBodyExpand.Add(Instantiate(snakeBodyExpandPrefab));
        }
    }

    IEnumerator ActivateShield()
    {
        isShieldActive = true;
        yield return new WaitForSeconds(10);
        isShieldActive = false;
    }
    IEnumerator ScoreBoost()
    {
        isScoreBoost = true;
        yield return new WaitForSeconds(15);
        isScoreBoost = false;
    }
    IEnumerator SpeedBoost()
    {
        isSpeedBoost = true;
        yield return new WaitForSeconds(5);
        isSpeedBoost = false;
    }
}