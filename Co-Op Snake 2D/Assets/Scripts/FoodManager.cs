using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : SingletonGenerics<FoodManager>
{
    //public static event Action FoodSpawn;
    //public static event Action PoisonSpawn;

    [SerializeField] private BoxCollider2D gridArea;

    public BoxCollider2D MassGainer, MassBurner;

    //private void Update()
    //{
    //    FoodSpawn?.Invoke();
    //    PoisonSpawn?.Invoke();
    //}
    public IEnumerator SpwanTime(BoxCollider2D food)
    {
        FoodSpawnArea(food);
        yield return new WaitForSecondsRealtime(6);
        FoodSpawnArea(food);
    }

    public void FoodSpawnArea(BoxCollider2D food)
    {
        Destroy(food);
        Bounds bounds = gridArea.bounds;
        float x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float y = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
        Instantiate(food);
        food.gameObject.transform.position = new Vector2(Mathf.Round(x), Mathf.Round(y));
    }
    //public void PoisonSpawnArea()
    //{
    //    Bounds bounds = gridArea.bounds;
    //    float x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
    //    float y = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
    //    transform.position = new Vector2(Mathf.Round(x), Mathf.Round(y));
    //}
}
