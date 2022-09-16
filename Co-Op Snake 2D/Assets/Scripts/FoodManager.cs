using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : SingletonGenerics<FoodManager>
{
    public FoodController massGainer;
    public FoodController massBurner;
    private void Start()
    {
        SpawnFood(massGainer);
        SpawnFood(massBurner);
    }

    public void SpawnFood(FoodController Food)
    {
        FoodController foodController = Instantiate(Food);
        //StartCoroutine(foodController.SpwanTime(10));
    }
}
