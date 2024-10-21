using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 0.0f;
    public float maxMoveSpeed = 3f;
    private float initialSpeed;
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;

    public float MoveSpeed => moveSpeed;

    private void Awake()
    {
        initialSpeed = moveSpeed;
    }
    public void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }

    public void AccelerateBy(float multiplier)
    {
        moveSpeed *= multiplier;
        moveSpeed = Mathf.Clamp(moveSpeed, 1f, maxMoveSpeed);
    }

    public void DecelerateBy(float multiplier)
    {
        moveSpeed /= multiplier;
        moveSpeed = Mathf.Clamp(moveSpeed, 1f, maxMoveSpeed);
    }

    public void TurnToNormal()
    {
        moveSpeed = initialSpeed;
    }
}
