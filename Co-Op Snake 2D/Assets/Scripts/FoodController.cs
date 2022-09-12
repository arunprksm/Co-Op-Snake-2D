using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : SingletonGenerics<FoodController>
{
    public BoxCollider2D gridArea;
    private void Start()
    {
        FoodSpawnArea();
    }
    public void FoodSpawnArea()
    {
        Bounds bounds = this.gridArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        this.transform.position = new Vector2(Mathf.Round(x), Mathf.Round(y));
    }
}