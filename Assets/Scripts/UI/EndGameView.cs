using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameView : View
{
    [SerializeField] TMP_Text _totalTime, _totalEnemyKilled, _totalWaves;

    public override void Init()
    {
        base.Init();
    }

    public override void OpenView()
    {
        base.OpenView();

        TimeSpan time = TimeSpan.FromSeconds(PlayerManager.Instance.TotalTime);
        _totalTime.text = time.ToString("mm':'ss");

        _totalEnemyKilled.text = PlayerManager.Instance.TotalEnemyKilled.ToString();

        _totalWaves.text = WaveManager.Instance.CurrentIndexOfWaves.ToString();
    }

    public override void CloseView()
    {
        base.CloseView();
    }

    public void HandleReloadScene() => GameManager.Instance.ReloadScene();
}
