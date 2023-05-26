using BaseTemplate.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    [SerializeField] int _coin = 0, _totalEnemyKilled = 0;
    [SerializeField] string _name = "Unnamed tank";

    float _totalTime;

    public string Name { get => _name; }
    public int Coin { get => _coin; }
    public float TotalTime { get => _totalTime; }
    public int TotalEnemyKilled { get => _totalEnemyKilled; }

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
