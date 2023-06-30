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
    public List<int> UpgradePointsNeedsForeachLevel { get => upgradePointsNeedsForeachLevel; }
    [SerializeField] List<int> upgradePointsNeedsForeachLevel;

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
        if (PlayerController.Instance.UpgradePoint >= upgradePointsNeedsForeachLevel[PlayerController.Instance._healthRegenLevel])
        {
            UsePoint(upgradePointsNeedsForeachLevel[PlayerController.Instance._healthRegenLevel]);
            PlayerController.Instance.Upgrade("HealthRegeneration");
        }
    }
    public void HandleUpgradeHealth()
    {
        if (PlayerController.Instance.UpgradePoint
            >=
            upgradePointsNeedsForeachLevel[PlayerController.Instance._healthLevel])
        {
            UsePoint(upgradePointsNeedsForeachLevel[PlayerController.Instance._healthLevel]);
            PlayerController.Instance.Upgrade("Health");
        }
    }
    public void HandleUpgradeBodyDamage()
    {
        if (PlayerController.Instance.UpgradePoint
            >=
            upgradePointsNeedsForeachLevel[PlayerController.Instance._bodyDamageLevel])
        {
            UsePoint(upgradePointsNeedsForeachLevel[PlayerController.Instance._bodyDamageLevel]);
            PlayerController.Instance.Upgrade("BodyDamage");
        }
    }
    public void HandleUpgradeBulletSpeed()
    {
        if (PlayerController.Instance.UpgradePoint
            >=
            upgradePointsNeedsForeachLevel[PlayerController.Instance._bulletSpeedLevel])
        {
            UsePoint(upgradePointsNeedsForeachLevel[PlayerController.Instance._bulletSpeedLevel]);
            PlayerController.Instance.Upgrade("BulletSpeed");
        }
    }
    public void HandleUpgradeBulletPenetration()
    {
        if (PlayerController.Instance.UpgradePoint
            >=
            upgradePointsNeedsForeachLevel[PlayerController.Instance._bulletPenetrationLevel])
        {
            UsePoint(upgradePointsNeedsForeachLevel[PlayerController.Instance._bulletPenetrationLevel]);
            PlayerController.Instance.Upgrade("BulletPenetration");
        }
    }
    public void HandleUpgradeBulletDamage()
    {
        if (PlayerController.Instance.UpgradePoint
            >=
            upgradePointsNeedsForeachLevel[PlayerController.Instance._bulletDamageLevel])
        {
            UsePoint(upgradePointsNeedsForeachLevel[PlayerController.Instance._bulletDamageLevel]);
            PlayerController.Instance.Upgrade("BulletDamage");
        }
    }
    public void HandleUpgradeBulletReload()
    {
        if (PlayerController.Instance.UpgradePoint
            >=
            upgradePointsNeedsForeachLevel[PlayerController.Instance._bulletReloadLevel])
        {
            UsePoint(upgradePointsNeedsForeachLevel[PlayerController.Instance._bulletReloadLevel]);
            PlayerController.Instance.Upgrade("FireRate");
        }
    }
    public void HandleUpgradeMovementSpeed()
    {
        if (PlayerController.Instance.UpgradePoint
            >=
            upgradePointsNeedsForeachLevel[PlayerController.Instance._movementSpeedLevel])
        {
            UsePoint(upgradePointsNeedsForeachLevel[PlayerController.Instance._movementSpeedLevel]);
            PlayerController.Instance.Upgrade("Speed");
        }
    }
    public void AddPoint(int amount)
    {
        PlayerController.Instance.UpgradePoint += amount;
        UpgradePopup.ActiveAllButton();
        UpgradePopup.SetPoint();
        OpenUpgrades();

    }

    public void UsePoint(int pointUse = 1)
    {
        PlayerController.Instance.UpgradePoint -= pointUse;
        UpgradePopup.SetPoint();
        if (PlayerController.Instance.UpgradePoint <= 0) StartCoroutine(CloseUpgradePopUp());
    }

    #endregion
}
