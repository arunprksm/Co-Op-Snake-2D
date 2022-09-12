using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class ScreenBounds : MonoBehaviour
{
    BoxCollider2D boxCollider2D;
    private UnityEvent<Collider2D> ExitTriggerFired;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float teleportOffset = 0.2f;
    [SerializeField] private float cornerOffset = 1f;


    private void Awake()
    {
        mainCamera.transform.localScale = Vector2.one;
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;
    }

    private void Start()
    {
        transform.position = Vector2.zero;
        UpdateBoundSize();
    }

    public void UpdateBoundSize()
    {
        float ySize = mainCamera.orthographicSize * 2;

        Vector2 boxColliderSize = new Vector2(ySize * mainCamera.aspect, ySize);
        boxCollider2D.size = boxColliderSize;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ExitTriggerFired?.Invoke(collision);
    }

    public bool AmIOutOfBounds(Vector3 worldPosition)
    {
        return (
                Mathf.Abs(worldPosition.x) > Mathf.Abs(boxCollider2D.bounds.min.x) || 
                Mathf.Abs(worldPosition.y) > Mathf.Abs(boxCollider2D.bounds.min.y)
               );
    }

    public Vector2 CalculateWrappedPosition(Vector2 worldPosition)
    {
        bool xBoundResult = Mathf.Abs(worldPosition.x) > Mathf.Abs(boxCollider2D.bounds.min.x) - cornerOffset;
        bool yBoundResult = Mathf.Abs(worldPosition.y) > Mathf.Abs(boxCollider2D.bounds.min.y) - cornerOffset;

        Vector2 signWorldPosition = new Vector2(Mathf.Sign(worldPosition.x), Mathf.Sign(worldPosition.y));

        if (xBoundResult && yBoundResult)
        {
            return Vector2.Scale(worldPosition, Vector2.one * -1) + Vector2.Scale(new Vector2(teleportOffset, teleportOffset), signWorldPosition);
        }
        else if (xBoundResult)
        {
            return new Vector2(worldPosition.x * -1, worldPosition.y) + new Vector2(teleportOffset * signWorldPosition.x, teleportOffset);
        }
        else if (yBoundResult)
        {
            return new Vector2(worldPosition.x, worldPosition.y * -1) + new Vector2(teleportOffset, teleportOffset * signWorldPosition.y);
        }
        else
        {
            return worldPosition;
        }
    }
}