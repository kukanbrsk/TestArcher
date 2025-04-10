using System;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public event Action<Arrow> OnReturnToPool;

    private Rigidbody2D rigidbodyArrow;
    private Vector3 viewportPosition;
    private Camera mainCamera;

    private void Start()
    {
        rigidbodyArrow = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        UpdateViewportPosition();
        if (IsOutOfScreenBounds())
        {
            OnReturnToPool?.Invoke(this);
        }

        var velocity = rigidbodyArrow.velocity;
        var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private bool IsOutOfScreenBounds() => viewportPosition.x < -1.1f || viewportPosition.x > 1.1f || viewportPosition.y < -1.1f;
    private void UpdateViewportPosition() => viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
}