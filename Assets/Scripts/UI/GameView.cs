using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameView : View
{
    [SerializeField] TMP_Text _playerName, _coins, _waves, _timerBeforeWaves;
    [SerializeField] SliderBar _levelBar, _healthBar;

    public override void Init()
    {
        base.Init();
    }

    public override void OpenView()
    {
        base.OpenView();

        SetPlayerData();
    }

    public override void CloseView()
    {
        base.CloseView();
    }

    void SetPlayerData()
    {
        _playerName.text = PlayerManager.Instance.Name;
        InitHealthBar();
        InitExpBar();
        UpdateCoins();
        UpdateWaves();
        _timerBeforeWaves.text = "";
    }

    #region UpdateUI

    public void UpdateExpBar() => _levelBar.SetBar(PlayerManager.Instance.Exp);
    public void UpdateHealthBar() => _healthBar.SetBar(PlayerController.Instance.GetCurrentHealth, true);
    public void InitExpBar() => _levelBar.SetMaxBarWithText(PlayerManager.Instance.GetCurrentExpForNextLevel, PlayerManager.Instance.Exp, "Level : " + PlayerManager.Instance.Level);
    public void InitHealthBar() => _healthBar.SetMaxBar(PlayerController.Instance.GetCurrentMaxHealth, PlayerController.Instance.GetCurrentHealth, true);
    public void UpdateCoins() => _coins.text = PlayerManager.Instance.Coin + " coins";
    public void UpdateWaves() => _waves.text = "Wave : " + 0; //Wave manager get current Wave





    #endregion
}
