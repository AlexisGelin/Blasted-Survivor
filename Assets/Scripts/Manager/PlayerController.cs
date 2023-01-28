using BaseTemplate.Behaviours;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoSingleton<PlayerController>
{
    [SerializeField] TankData _data;

    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _canonTransform;

    Bullet _bullet;
    PlayerInput _playerInput;

    Vector2 moveInput;
    Vector3 moveDirection;

    float nextFire = 0.0f;

    public TankData Data { get => _data; }

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
        _rb.MovePosition(transform.position + moveDirection * _data.Speed * Time.fixedDeltaTime);
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void Fire()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + _data.FireRate;

            _canonTransform.DOLocalMoveX(.25f, _data.FireRate / 2).OnComplete( () =>
            {
                _canonTransform.DOLocalMoveX(.3f, _data.FireRate / 2); ;
            });

            GameObject bullet = PoolManager.Instance.GetPooledObject(0);
            _bullet = bullet.GetComponent<Bullet>();
            bullet.transform.position = _canonTransform.transform.position;
            bullet.SetActive(true);

            _bullet.Init(_data.Bullet,transform.right);
        }
    }
}

