using BaseTemplate.Behaviours;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoSingleton<PlayerController>, IHealth
{
    [SerializeField] TankData _data, _upgradeData;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _renderer;

    TankRenderer _tankRenderer;
    Bullet _bullet;
    PlayerInput _playerInput;

    Sequence _damageColorTweek;
    List<Sequence> _damageColorTweeks = new List<Sequence>();

    Vector2 moveInput;
    Vector3 moveDirection;

    float nextFire = 0.0f;
    int _health, _maxHealth;

    #region Getter/Setter

    public int GetCurrentHealth { get { return _health; } }
    public int GetCurrentMaxHealth { get { return _maxHealth; } }
    public int GetCurrentBodyDamage { get { return _data.BodyDamage + _upgradeData.BodyDamage; } }
    public int GetCurrentHealthRegeneration { get { return _data.HealthRegeneration + _upgradeData.HealthRegeneration; } }
    public int GetCurrentSpeed { get { return _data.Speed + _upgradeData.Speed; } }
    public int GetCurrentBulletSpeed { get { return _data.Bullet.Speed + _upgradeData.Bullet.Speed; } }
    public int GetCurrentBulletDamage { get { return _data.Bullet.Damage + _upgradeData.Bullet.Damage; } }
    public int GetCurrentBulletPenetration { get { return _data.Bullet.Penetration + _upgradeData.Bullet.Penetration; } }
    public float GetCurrentFireRate { get { return _data.FireRate + _upgradeData.FireRate; } }

    #endregion

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

        Debug.Log(_data.Health);

        _maxHealth = _data.Health + _upgradeData.Health;
        _health = _data.Health + _upgradeData.Health;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState != GameState.PLAY) return;

        PlayerRotate();
        PlayerMovement();

        if (_playerInput.Player.Fire.IsPressed()) Fire();

        if (Input.GetKeyDown(KeyCode.G)) TakeDamage(1);
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.GameState != GameState.PLAY) return;

        _rb.MovePosition(transform.position + moveDirection * (_data.Speed + _upgradeData.Speed) * Time.fixedDeltaTime);
    }

    #region Movement/Rotation
    void PlayerMovement()
    {
        moveDirection = new Vector3(moveInput.x, moveInput.y, 0);
        moveDirection = Vector3.Normalize(moveDirection);
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void PlayerRotate()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, cursorPos - playerPos);
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
    }

    #endregion

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
            case "HealthRegeneration":
                _upgradeData.HealthRegeneration += 1;
                break;
            case "Health":
                _upgradeData.Health += 1;
                break;
            case "BodyDamage":
                _upgradeData.BodyDamage += 1;
                break;
            case "BulletSpeed": // fait
                _upgradeData.Bullet.Speed += 1;
                break;
            case "BulletPenetration": // fait
                _upgradeData.Bullet.Penetration += 1;
                break;
            case "BulletDamage": // fait
                _upgradeData.Bullet.Damage += 1;
                break;
            case "FireRate": // fait
                _upgradeData.FireRate += 0.1f;
                break;
            case "Speed": // fait
                _upgradeData.Speed += 1;
                break;
        }
    }

    public bool TakeDamage(int amount)
    {
        _health -= amount;
        if (_health < 0) _health = 0;

        UIManager.Instance.GameView.UpdateHealthBar();
        /*  
        if (_damageColorTweeks != null)
        {
            foreach (var tween in _damageColorTweeks)
            {
                tween.Complete();
            }
        }


      foreach (var sprite in _sprites)
        {
            Color tempColor = sprite.color;

            _damageColorTweek = DOTween.Sequence();

            _damageColorTweek.Join(sprite.DOColor(Color.white, .05f))
                .Join(transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), .05f))
                .Append(sprite.DOColor(tempColor, .05f))
                .Join(transform.DOScale(Vector3.one, .05f));

            _damageColorTweeks.Add(_damageColorTweek);
        }*/

        if (_health <= 0)
        {
            StartCoroutine(ScaleTankAndDisable());

            GameManager.Instance.UpdateGameState(GameState.END);

            return true;
        }

        return false;

    }

    IEnumerator ScaleTankAndDisable()
    {
        transform.DOScale(new Vector3(2, 2, 2), .3f);

        _rb.simulated = false;

        yield return new WaitForSeconds(.3f);

        gameObject.SetActive(false);
    }

    public bool TakeHeal(int amount)
    {
        throw new System.NotImplementedException();
    }
}

