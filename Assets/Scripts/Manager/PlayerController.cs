using BaseTemplate.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoSingleton<PlayerController>
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] float _speed = 2f;

    Vector2 moveInput;
    Vector3 moveDirection;

    public void Init()
    {

    }


    private void FixedUpdate()
    {
        moveDirection = new Vector3(moveInput.x, moveInput.y, 0);
        moveDirection = Vector3.Normalize(moveDirection);
        _rb.velocity = moveDirection * _speed;
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}

