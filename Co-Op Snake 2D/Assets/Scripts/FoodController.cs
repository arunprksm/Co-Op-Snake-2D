using UnityEngine;

public class FoodController : MonoBehaviour
{
    public BoxCollider2D gridArea;


    private void Start()
    {
        FoodSpawnArea();
    }
    private void FoodSpawnArea()
    {
        Bounds bounds = this.gridArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        this.transform.position = new Vector2(Mathf.Round(x), Mathf.Round(y));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SnakeController.instance = collision.gameObject.GetComponent<SnakeController>();
        if (SnakeController.instance != null)
        {
            FoodSpawnArea();
            SnakeController.Instance.SnakeExpand();
        }
    }
}
