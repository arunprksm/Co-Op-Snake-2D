using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public static SnakeController instance;
    public static SnakeController Instance
    {
        get
        {
            return instance;
        }
    }

    public ScreenBounds screenBounds;
    private Vector2 snakeDirection = Vector2.up;
    private bool w, a, s, d;
    private List<Transform> snakeBodyExpand = new List<Transform>();

    [SerializeField] private Transform snakeBodyExpandPrefab;
    [SerializeField] private int snakeInitialSize = 5;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
        w = Input.GetKeyDown(KeyCode.W);
        a = Input.GetKeyDown(KeyCode.A);
        s = Input.GetKeyDown(KeyCode.S);
        d = Input.GetKeyDown(KeyCode.D);

    }

    private void HandleSnakeMovements()
    {
        if (w && snakeDirection != Vector2.down)
        {
            snakeDirection = Vector2.up;
        }

        else if (a && snakeDirection != Vector2.right)
        {
            snakeDirection = Vector2.left;
        }
        else if (s && snakeDirection != Vector2.up)
        {
            snakeDirection = Vector2.down;
        }
        else if (d && snakeDirection != Vector2.left)
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
        //transform.position = new Vector2(Mathf.Round(this.transform.position.x) + snakeDirection.x, Mathf.Round(this.transform.position.y) + snakeDirection.y);
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
        this.transform.position = Vector2.one;
        snakeBodyExpand.Clear();
        snakeBodyExpand.Add(this.transform);

        for (int i = 1; i < this.snakeInitialSize; i++)
        {
            snakeBodyExpand.Add(Instantiate(this.snakeBodyExpandPrefab));
        }
    }
}
