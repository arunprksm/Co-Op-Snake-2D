using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : SingletonGenerics<ShieldController>
{
    [SerializeField] private BoxCollider2D gridArea;
    private void Start()
    {
        ShieldSpawn();
    }
    public void ShieldSpawn()
    {
        Bounds bounds = gridArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        transform.position = new Vector2(Mathf.Round(x), Mathf.Round(y));
    }
}