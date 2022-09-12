using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private static SnakeController instance;
    public static SnakeController Instance
    {
        get
        {
            return instance;
        }
    }

    public ScreenBounds screenBounds;
    private Vector2 snakeDirection = Vector2.up;
    private bool up, left, down, right;
    private List<Transform> snakeBodyExpand = new List<Transform>();

    [SerializeField] private Transform snakeBodyExpandPrefab;
    [SerializeField] private int snakeInitialSize = 5;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        ResetGame();
    }

    private void Update()
    {
        HandleInputs();
        HandleSnakeMovements();
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
    }

    private void HandleSnakeMovements()
    {
        if (up && !down && snakeDirection != Vector2.down)
        {
            snakeDirection = Vector2.up;
        }

        else if (left && !right && snakeDirection != Vector2.right)
        {
            snakeDirection = Vector2.left;
        }
        else if (down && !up && snakeDirection != Vector2.up)
        {
            snakeDirection = Vector2.down;
        }
        else if (right && !left && snakeDirection != Vector2.left)
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
        Transform newSnakeBodyExpand = Instantiate(this.snakeBodyExpandPrefab);
        newSnakeBodyExpand.position = snakeBodyExpand[snakeBodyExpand.Count - 1].position;
        snakeBodyExpand.Add(newSnakeBodyExpand);
    }
    public void SnakeShrink()
    {
        if (snakeBodyExpand.Count == 1)
        {
            ResetGame();
        }
        else
        {
            Destroy(snakeBodyExpand[snakeBodyExpand.Count - 1].gameObject);
            snakeBodyExpand.RemoveAt(snakeBodyExpand.Count - 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hit")
        {
            ResetGame();
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
