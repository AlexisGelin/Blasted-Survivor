using BaseTemplate.Behaviours;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoSingleton<PlayerController>, IHealth
{
    public bool IsInteract;
    public Vector3 PlayerVelocity;

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

    float _nextFire = 0.0f;
    int _health, _maxHealth;

    Coroutine _playerRegenCoroutine;

    int _healthRegenLevel, _healthLevel, _bodyDamageLevel, _bulletSpeedLevel, _bulletPenetrationLevel, _bulletDamageLevel, _bulletReloadLevel, _movementSpeedLevel, _upgradePoint;
    const int MAX_LEVEL_UPGRADE = 7;

    List<Sequence> _canonSequences = new List<Sequence>();
    List<Vector2> _canonTransforms = new List<Vector2>();

    #region Getter/Setter

    public int GetCurrentHealth { get { return _health; } }
    public int GetCurrentMaxHealth { get { return _maxHealth + _upgradeData.Health; } }
    public int GetCurrentBodyDamage { get { return _data.BodyDamage + _upgradeData.BodyDamage; } }
    public float GetCurrentHealthRegeneration { get { return _data.HealthRegeneration + _upgradeData.HealthRegeneration; } }
    public float GetCurrentSpeed { get { return _data.Speed + _upgradeData.Speed; } }
    public float GetCurrentBulletSpeed { get { return _data.Bullet.Speed + _upgradeData.Bullet.Speed; } }
    public int GetCurrentBulletDamage { get { return _data.Bullet.Damage + _upgradeData.Bullet.Damage; } }
    public int GetCurrentBulletPenetration { get { return _data.Bullet.Penetration + _upgradeData.Bullet.Penetration; } }
    public float GetCurrentFireRate { get { return _data.FireRate + _upgradeData.FireRate; } }

    public int UpgradePoint { get => _upgradePoint; set => _upgradePoint = value; }

    #endregion

    public void Init()
    {
        _playerInput = new PlayerInput();
        _playerInput.Enable();

        _playerInput.Player.Interact.started += PlayerStartInteract;

        _playerInput.Player.Interact.canceled += PlayerStopInteract;

        _upgradeData.ResetData();

        GenerateTank();
    }

    private void PlayerStartInteract(InputAction.CallbackContext obj)
    {
        IsInteract = true;
    }
    private void PlayerStopInteract(InputAction.CallbackContext obj)
    {
        IsInteract = false;
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

        _canonTransforms.Clear();

        foreach (var canon in _tankRenderer._canonTransforms)
        {
            _canonTransforms.Add(canon.localPosition);
        }

        _maxHealth = _data.Health + _upgradeData.Health;
        _health = _data.Health + _upgradeData.Health;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState != GameState.PLAY) return;

        PlayerRotate();
        PlayerMovement();

        if (_playerInput.Player.Fire.IsPressed()) Fire();

        if (Input.GetKeyDown(KeyCode.G)) TakeDamage(10);
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.GameState != GameState.PLAY) return;

        PlayerVelocity = (moveDirection * (_data.Speed + _upgradeData.Speed));

        _rb.MovePosition(transform.position + PlayerVelocity * Time.fixedDeltaTime);
    }

    #region Movement/Rotation/Regen
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

    IEnumerator PlayerRegen()
    {
        yield return new WaitForSeconds(4);

        while (GetCurrentHealth < GetCurrentMaxHealth)
        {
            yield return new WaitForSeconds(.1f);
            int onePercentHP = (int)(GetCurrentMaxHealth * 0.01f);
            int healthToRegen = (int)(GetCurrentMaxHealth * (onePercentHP + GetCurrentHealthRegeneration) / 100);
            healthToRegen = (int)Mathf.Clamp(healthToRegen, 1, Mathf.Infinity);

            TakeHeal(healthToRegen);
        }
    }

    #endregion

    private void Fire()
    {
        if (_tankRenderer._canonTransforms.Count == 0) return;

        if (Time.time > _nextFire)
        {
            _nextFire = Time.time + _data.FireRate - _upgradeData.FireRate;

            AudioManager.Instance.PlaySound("PlayerShoot");


            if (_playerRegenCoroutine != null)
            {
                StopCoroutine(_playerRegenCoroutine);
                _playerRegenCoroutine = null;
            }

            _playerRegenCoroutine = StartCoroutine(PlayerRegen());

            foreach (Sequence seq in _canonSequences)
            {
                seq.Kill();
            }

            _canonSequences.Clear();

            for (int i = 0; i < _tankRenderer._canonTransforms.Count; i++)
            {
                var currentCannon = _tankRenderer._canonTransforms[i];
                GameObject bullet = PoolManager.Instance.GetPooledObject(0);
                _bullet = bullet.GetComponent<Bullet>();

                bullet.transform.position = currentCannon.transform.position;
                bullet.SetActive(true);

                _bullet.Init(_data.Bullet, _upgradeData.Bullet, currentCannon.transform.right);

                currentCannon.transform.localPosition = new Vector3(_canonTransforms[i].x, _canonTransforms[i].y);

                _tankRenderer.ShootFX[i].Play();

                var newSeq = DOTween.Sequence()
                   .Join(currentCannon.transform.DOLocalMoveX(currentCannon.transform.localPosition.x - .05f, .05f).SetEase(Ease.OutSine))
                   .Append(currentCannon.transform.DOLocalMoveX(currentCannon.transform.localPosition.x + .05f, .1f).SetEase(Ease.OutSine))
                   .Append(currentCannon.transform.DOLocalMoveX(currentCannon.transform.localPosition.x, .5f).SetEase(Ease.OutSine));

                _canonSequences.Add(newSeq);
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
            case "HealthRegeneration": // Fait
                if (_healthRegenLevel >= MAX_LEVEL_UPGRADE) return;

                _upgradeData.HealthRegeneration += PlayerManager.Instance.HealthRegenerationUpdate;

                _healthRegenLevel++;

                UIManager.Instance.GameView.UpgradePopup.UpdateHealthRegen(_healthRegenLevel);
                break;
            case "Health": // Fait
                if (_healthLevel >= MAX_LEVEL_UPGRADE) return;

                _upgradeData.Health += PlayerManager.Instance.HealthUpdate;
                _health += PlayerManager.Instance.HealthUpdate;

                _healthLevel++;

                UIManager.Instance.GameView.InitHealthBar();

                UIManager.Instance.GameView.UpgradePopup.UpdateMaxHealth(_healthLevel);
                break;
            case "BodyDamage": // Fait
                if (_bodyDamageLevel >= MAX_LEVEL_UPGRADE) return;

                _upgradeData.BodyDamage += PlayerManager.Instance.BodyDamageUpgrade;

                _bodyDamageLevel++;

                UIManager.Instance.GameView.UpgradePopup.UpdateBodyDamage(_bodyDamageLevel);
                break;
            case "BulletSpeed": // fait
                if (_bulletSpeedLevel >= MAX_LEVEL_UPGRADE) return;

                _upgradeData.Bullet.Speed += PlayerManager.Instance.BulletSpeedUpgrade;

                _bulletSpeedLevel++;

                UIManager.Instance.GameView.UpgradePopup.UpdateBulletSpeed(_bulletSpeedLevel);
                break;
            case "BulletPenetration": // fait
                if (_bulletPenetrationLevel >= MAX_LEVEL_UPGRADE) return;

                _upgradeData.Bullet.Penetration += PlayerManager.Instance.BulletPenetrationUpdate;

                _bulletPenetrationLevel++;

                UIManager.Instance.GameView.UpgradePopup.UpdateBulletPenetration(_bulletPenetrationLevel);
                break;
            case "BulletDamage": // fait
                if (_bulletDamageLevel >= MAX_LEVEL_UPGRADE) return;

                _upgradeData.Bullet.Damage += PlayerManager.Instance.BulletDamageUpdate;

                _bulletDamageLevel++;

                UIManager.Instance.GameView.UpgradePopup.UpdateBulletDamage(_bulletDamageLevel);
                break;
            case "FireRate": // fait
                if (_bulletReloadLevel >= MAX_LEVEL_UPGRADE) return;

                _upgradeData.FireRate += PlayerManager.Instance.BulletFireRateUpdate;

                _bulletReloadLevel++;

                UIManager.Instance.GameView.UpgradePopup.UpdateBulletReload(_bulletReloadLevel);
                break;
            case "Speed": // fait
                if (_movementSpeedLevel >= MAX_LEVEL_UPGRADE) return;

                _upgradeData.Speed += PlayerManager.Instance.SpeedUpdate;

                _movementSpeedLevel++;

                UIManager.Instance.GameView.UpgradePopup.UpdateMovementSpeed(_movementSpeedLevel);
                break;
        }
    }

    public bool TakeDamage(int amount)
    {
        _health -= amount;
        if (_health < 0) _health = 0;

        AudioManager.Instance.PlaySound("PlayerHit");

        UIManager.Instance.GameView.UpdateHealthBar();

        PostProcessManager.Instance.DoVignetteFlash(1f, 1f);

        if (_playerRegenCoroutine != null)
        {
            StopCoroutine(_playerRegenCoroutine);
            _playerRegenCoroutine = null;
        }

        if (_health <= 0)
        {
            StartCoroutine(ScaleTankAndDisable());

            CameraManager.Instance.ZoomHit(.5f, .2f, true);

            GameManager.Instance.UpdateStateToEnd();

            AudioManager.Instance.PlaySound("LooseGame");

            return true;
        }
        else
        {
            CameraManager.Instance.ZoomHit(.5f, .2f);

            _playerRegenCoroutine = StartCoroutine(PlayerRegen());

            if (_damageColorTweeks != null)
            {
                foreach (var tween in _damageColorTweeks)
                {
                    tween.Complete();
                }
            }


            foreach (var sprite in _tankRenderer.Sprite)
            {
                Color tempColor = sprite.color;

                _damageColorTweek = DOTween.Sequence();

                _damageColorTweek.Join(sprite.DOColor(Color.white, .1f))
                    .Join(transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), .1f))
                    .Append(sprite.DOColor(tempColor, .1f))
                    .Join(transform.DOScale(Vector3.one, .1f));

                _damageColorTweeks.Add(_damageColorTweek);
            }
        }

        return false;

    }

    IEnumerator ScaleTankAndDisable()
    {

        if (_damageColorTweeks != null)
        {
            foreach (var tween in _damageColorTweeks)
            {
                tween.Complete();
            }
        }

        transform.DOScale(new Vector3(2, 2, 2), .5f);

        foreach (var sprite in _tankRenderer.Sprite) sprite.DOFade(0, .5f);


        _rb.simulated = false;

        yield return new WaitForSeconds(.3f);

        gameObject.SetActive(false);
    }

    public void TakeHeal(int amount)
    {
        _health += amount;

        if (GetCurrentHealth > GetCurrentMaxHealth) _health = GetCurrentMaxHealth;

        UIManager.Instance.GameView.InitHealthBar();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<IHealth>() != null)
        {
            var enemy = collision.gameObject.GetComponent<IHealth>();
            enemy.TakeDamage(GetCurrentBodyDamage);

        }

        if (collision.gameObject.GetComponent<EnemyController>() != null)
        {
            var enemy = collision.gameObject.GetComponent<EnemyController>();
            TakeDamage(enemy.GetBodyDamage);
        }
    }
}

