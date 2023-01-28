using BaseTemplate.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoSingleton<PlayerController>
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] float _speed = 2f;
    [SerializeField] GameObject _bullet;
    [SerializeField] Transform _bulletSpawn;
    [SerializeField] float fireRate = 0.5f;

    PlayerInput _playerInput;

    Vector2 moveInput;
    Vector3 moveDirection;

    float nextFire = 0.0f;

    public void Init()
    {
        _playerInput = new PlayerInput();
        _playerInput.Enable();
    }
    private void Update()
    {
        //Rotate Player
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, cursorPos - playerPos);
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);

        //Movement
        moveDirection = new Vector3(moveInput.x, moveInput.y, 0);
        moveDirection = Vector3.Normalize(moveDirection);

        //Fire
        if (_playerInput.Player.Fire.IsPressed()) Fire();
    }


    private void FixedUpdate()
    {
        _rb.MovePosition(transform.position + moveDirection * _speed * Time.fixedDeltaTime);
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void Fire()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            GameObject bullet = Instantiate(_bullet, _bulletSpawn.position, transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = transform.right * 10f;
        }
    }
}

