using System.Collections;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private BoxCollider2D gridArea;
    [SerializeField] private float time;

    private void Start()
    {
        FoodSpawnArea();
    }

    public void FoodSpawnArea()
    {
        Bounds bounds = gridArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        x = Mathf.Round(x);
        y = Mathf.Round(y);
        x++;
        y--; // to prevent another food spawning on same area... 50/50 chance...

        transform.position = new Vector2(x, y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.Instance.StartCoroutine(ReSpawn(time)); //http://answers.unity.com/comments/1847144/view.html <== reference
    }

    IEnumerator ReSpawn(float time)
    {
        if (gameObject.activeInHierarchy)
            gameObject.SetActive(false);
        yield return new WaitForSeconds(time);
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        FoodSpawnArea();
    }
}