using BaseTemplate.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    [SerializeField] int _level = 0, _exp = 0, _coin = 0;
    [SerializeField] string _name = "Unnamed tank";

    public int GetCurrentExpForNextLevel { get { return ExpForNextLevel(); } }

    public string Name { get => _name; }
    public int Level { get => _level; }
    public int Exp { get => _exp; }
    public int Coin { get => _coin; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            UpdateCoins(1000);
        }
    }

    int ExpForNextLevel()
    {
        return 20;
    }

    public void IncreaseExp(int expToAdd)
    {
        if (_exp + expToAdd >= GetCurrentExpForNextLevel)
        {
            _exp += expToAdd;
            IncreaseLevel(_exp - GetCurrentExpForNextLevel);
        }
        else
        {
            _exp += expToAdd;
        }

        UIManager.Instance.GameView.UpdateExpBar();
    }

    public void UpdateCoins(int number)
    {
        _coin += number;
        UIManager.Instance.GameView.UpdateCoins();
    }

    void IncreaseLevel(int expLeft = 0)
    {
        _level++;
        _exp = 0;
        IncreaseExp(expLeft);

        UIManager.Instance.GameView.InitExpBar();
    }
}
