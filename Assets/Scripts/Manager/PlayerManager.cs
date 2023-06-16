using BaseTemplate.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    [SerializeField] int _coin = 0, _totalEnemyKilled = 0;
    [SerializeField] string _name = "Unnamed tank";

    [SerializeField] float healthRegenerationUpdate = .5f;
    [SerializeField] int healthUpdate = 20;
    [SerializeField] int bodyDamageUpgrade = 10;
    [SerializeField] float bulletSpeedUpgrade = 1;
    [SerializeField] int bulletPenetrationUpdate = 1;
    [SerializeField] int bulletDamageUpdate = 1;
    [SerializeField] float bulletFireRateUpdate = 0.1f;
    [SerializeField] float speedUpdate = 1;

    float _totalTime;

    public int Coin { get => _coin; }
    public float TotalTime { get => _totalTime; }
    public int TotalEnemyKilled { get => _totalEnemyKilled; }
    public string Name { get => _name; set => _name = value; }

    public float HealthRegenerationUpdate { get => healthRegenerationUpdate; }
    public int HealthUpdate { get => healthUpdate; }
    public int BodyDamageUpgrade { get => bodyDamageUpgrade; }
    public float BulletSpeedUpgrade { get => bulletSpeedUpgrade; }
    public int BulletPenetrationUpdate { get => bulletPenetrationUpdate;}
    public int BulletDamageUpdate { get => bulletDamageUpdate; }
    public float BulletFireRateUpdate { get => bulletFireRateUpdate; }
    public float SpeedUpdate { get => speedUpdate; }


    private void Update()
    {
        if (GameManager.Instance.IsGamePause == false && GameManager.Instance.GameState == GameState.PLAY) _totalTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.C))
        {
            UpdateCoins(1000);
        }
    }



    public void UpdateCoins(int number)
    {
        _coin += number;
        UIManager.Instance.GameView.UpdateCoins();
    }

    public void UpdateEnemyKilled(int number)
    {
        _totalEnemyKilled += number;
    }
}
