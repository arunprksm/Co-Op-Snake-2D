using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : SingletonGenerics<FoodController>
{
    [SerializeField] private BoxCollider2D gridArea;
    private void Start()
    {
        StartCoroutine(SpwanTime(6));
    }

    public IEnumerator SpwanTime(float time)
    {
        FoodSpawnArea();
        yield return new WaitForSecondsRealtime(time);
        FoodSpawnArea();
    }
    public void FoodSpawnArea()
    {
        Bounds bounds = gridArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        transform.position = new Vector2(Mathf.Round(x), Mathf.Round(y));
    }
}