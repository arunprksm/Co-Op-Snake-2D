using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private BoxCollider2D gridArea;
    [SerializeField] private float time;
    private void Start()
    {
        PowerUpSpawnArea();
    }

    public void PowerUpSpawnArea()
    {
        Bounds bounds = gridArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        x = Mathf.Round(x);
        y = Mathf.Round(y);

        transform.position = new Vector2(x, y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.Instance.StartCoroutine(ActivatePowerUp(time)); //http://answers.unity.com/comments/1847144/view.html <== reference
    }

    IEnumerator ActivatePowerUp(float time)
    {
        if (gameObject.activeInHierarchy)
            gameObject.SetActive(false);
        yield return new WaitForSeconds(time);
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        PowerUpSpawnArea();
    }
}