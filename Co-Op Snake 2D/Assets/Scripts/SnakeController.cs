using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : SingletonGenerics<SnakeController>
{
    public ScreenBounds screenBounds;
    private Vector2 snakeDirection;
    private bool up, left, down, right;
    private bool upArrow, leftArrow, downArrow, rightArrow;
    private List<Transform> snakeBodyExpand = new List<Transform>();
    string player1, player2;

    [SerializeField] private Transform snakeBodyExpandPrefab;
    [SerializeField] private int snakeInitialSize = 5;
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
    }

    private void Update()
    {
        HandleInputs();
        if (name == player1) HandleSnakeMovements(up,down,left,right);
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
        }
        else
        {
            transform.position = tempPosition;
        }
    }

    public void SnakeExpand()
    {
        Transform newSnakeBodyExpand = Instantiate(snakeBodyExpandPrefab);
        newSnakeBodyExpand.position = snakeBodyExpand[snakeBodyExpand.Count - 1].position;
        snakeBodyExpand.Add(newSnakeBodyExpand);
    }
    public void SnakeShrink()
    {
        if (snakeBodyExpand.Count == 1)
        {
            if (gameObject.name == player1) Debug.Log("player 1");
            else Debug.Log("Player 2");
            ResetGame();
        }
        else
        {
            Destroy(snakeBodyExpand[snakeBodyExpand.Count - 1].gameObject);
            snakeBodyExpand.RemoveAt(this.snakeBodyExpand.Count - 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hit")
        {
            if (gameObject.name == player1)
            {
                Debug.Log("player 1");
            }

            else Debug.Log("Player 2");
            //ResetGame();
        }

        if (collision.GetComponent<FoodController>())
        {
            FoodController.Instance.FoodSpawnArea();
            SnakeExpand();
        }

        if (collision.GetComponent<PoisonController>())
        {
            PoisonController.Instance.PoisonSpawnArea();
            SnakeShrink();
        }
    }

    private void ResetGame()
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
}
