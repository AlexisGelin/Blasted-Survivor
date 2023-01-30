using BaseTemplate.Behaviours;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoSingleton<PlayerController>
{
    [SerializeField] TankData _data, _upgradeData;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _renderer;

    TankRenderer _tankRenderer;
    Bullet _bullet;
    PlayerInput _playerInput;

    Vector2 moveInput;
    Vector3 moveDirection;

    float nextFire = 0.0f;
    int _health, _maxHealth;

    public TankData Data { get => _data; }

    public void Init()
    {
        _playerInput = new PlayerInput();
        _playerInput.Enable();

        _upgradeData.ResetData();

        GenerateTank();
    }

    private void GenerateTank()
    {
        if (_tankRenderer != null)
        {
            foreach (Transform TankRenderer in _tankRenderer._canonTransforms)
            {
                DOTween.Kill(TankRenderer);
            }
            Destroy(_tankRenderer.gameObject);
        }

        var tempRenderer = Instantiate(_data.Renderer.Renderer, _renderer);
        _tankRenderer = tempRenderer.GetComponent<TankRenderer>();

        _maxHealth = _data.Health + _upgradeData.Health;
        _health = _data.Health + _upgradeData.Health;
    }

    private void Update()
    {
        if (GameManager.Instance.gameState != GameState.PLAY) return;

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
        if (GameManager.Instance.gameState != GameState.PLAY) return;

        _rb.MovePosition(transform.position + moveDirection * (_data.Speed + _upgradeData.Speed) * Time.fixedDeltaTime);
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void Fire()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + _data.FireRate - _upgradeData.FireRate;

            foreach (var cannon in _tankRenderer._canonTransforms)
            {
                GameObject bullet = PoolManager.Instance.GetPooledObject(0);
                _bullet = bullet.GetComponent<Bullet>();

                bullet.transform.position = cannon.transform.position;
                bullet.SetActive(true);

                _bullet.Init(_data.Bullet, _upgradeData.Bullet, cannon.transform.right);
            }
        }
    }

    public void Evolve(string evolutionName)
    {
        _data = TankManager.Instance.GetTankData(evolutionName);

        GenerateTank();
    }

    public void Upgrade(string upgradeName)
    {
        switch (upgradeName)
        {
            case "Health":
                _upgradeData.Health += 1;
                break;
            case "HealthRegeneration":
                _upgradeData.HealthRegeneration += 1;
                break;
            case "BodyDamage":
                _upgradeData.BodyDamage += 1;
                break;
            case "Speed": // fait
                _upgradeData.Speed += 1;
                break;
            case "FireRate": // fait
                _upgradeData.FireRate += 0.1f;
                break;
            case "BulletDamage": // fait
                _upgradeData.Bullet.Damage += 1;
                break;
            case "BulletPenetration": // fait
                _upgradeData.Bullet.Penetration += 1;
                break;
            case "BulletSpeed": // fait
                _upgradeData.Bullet.Speed += 1;
                break;
        }
    }
}

