using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameView : View
{
    public UpgradePopup UpgradePopup;
    [SerializeField] TMP_Text _playerName, _coins, _waves, _timerBeforeWaves;
    [SerializeField] SliderBar _enemyBar, _healthBar;

    RectTransform upgradePopUpRT;

    public override void Init()
    {
        base.Init();

        upgradePopUpRT = UpgradePopup.GetComponent<RectTransform>();
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
        UpdateCoins();
        UpdateWaves();
        _timerBeforeWaves.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.U))
        {
            AddPoint(2);
        }
    }

    void OpenUpgrades()
    {
        UpgradePopup.gameObject.SetActive(true);
        upgradePopUpRT.DOAnchorMin(new Vector2(0, upgradePopUpRT.anchorMin.y), .2f);
        upgradePopUpRT.DOAnchorMax(new Vector2(1, upgradePopUpRT.anchorMax.y), .2f);
    }

    IEnumerator CloseUpgradePopUp()
    {
        UpgradePopup.DesactiveAllButton();
        yield return new WaitForSeconds(1);
        upgradePopUpRT.DOAnchorMin(new Vector2(-2, upgradePopUpRT.anchorMin.y), .2f);
        upgradePopUpRT.DOAnchorMax(new Vector2(-1, upgradePopUpRT.anchorMax.y), .2f).OnComplete(() => UpgradePopup.gameObject.SetActive(false));
    }

    #region UpdateUI

    public void UpdateEnemyBar() => _enemyBar.SetBar(WaveManager.Instance.NumberOfEnemyRemaining,"ENEMY LEFT : ");
    public void UpdateHealthBar() => _healthBar.SetBar(PlayerController.Instance.GetCurrentHealth, true);
    public void InitEnemyBar() => _enemyBar.SetMaxBarWithText(WaveManager.Instance.NumberOfEnemyRemaining, WaveManager.Instance.NumberOfEnemyRemaining, "Enemy left : " + WaveManager.Instance.NumberOfEnemyRemaining);
    public void InitHealthBar() => _healthBar.SetMaxBar(PlayerController.Instance.GetCurrentMaxHealth, PlayerController.Instance.GetCurrentHealth, true);
    public void UpdateCoins() => _coins.text = PlayerManager.Instance.Coin + " coins";
    public void UpdateWaves() => _waves.text = "Wave : " + WaveManager.Instance.CurrentIndexOfWaves;

    public void HandleUpgradeHealthRegen()
    {
        PlayerController.Instance.Upgrade("HealthRegeneration");
        UsePoint();
    }
    public void HandleUpgradeHealth()
    {
        PlayerController.Instance.Upgrade("Health");
        UsePoint();
    }
    public void HandleUpgradeBodyDamage()
    {
        PlayerController.Instance.Upgrade("BodyDamage");
        UsePoint();
    }
    public void HandleUpgradeBulletSpeed()
    {
        PlayerController.Instance.Upgrade("BulletSpeed");
        UsePoint();
    }
    public void HandleUpgradeBulletPenetration()
    {
        PlayerController.Instance.Upgrade("BulletPenetration");
        UsePoint();
    }
    public void HandleUpgradeBulletDamage()
    {
        PlayerController.Instance.Upgrade("BulletDamage");
        UsePoint();
    }
    public void HandleUpgradeBulletReload()
    {
        PlayerController.Instance.Upgrade("FireRate");
        UsePoint();
    }
    public void HandleUpgradeMovementSpeed()
    {
        PlayerController.Instance.Upgrade("Speed");
        UsePoint();
    }
    public void AddPoint(int amount)
    {
        PlayerController.Instance.UpgradePoint += amount;
        UpgradePopup.ActiveAllButton();
        UpgradePopup.SetPoint();
        OpenUpgrades();
    }

    public void UsePoint()
    {
        PlayerController.Instance.UpgradePoint--;
        UpgradePopup.SetPoint();
        if (PlayerController.Instance.UpgradePoint <= 0) StartCoroutine(CloseUpgradePopUp());
    }

    #endregion
}
