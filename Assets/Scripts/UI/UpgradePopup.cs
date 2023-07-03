using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePopup : MonoBehaviour
{
    [SerializeField] TMP_Text _pointText;
    [SerializeField] SliderBar _healthRegenSlider, _maxHealthSlider, _bodyDamageSlider, _bulletSpeedSlider, _bulletPenetrationSlider, _bulletDamageSlider, _bulletReloadSlider, _movementSpeedSlider;
    [SerializeField] Button _healthRegenButton, _maxHealthButton, _bodyDamageButton, _bulletSpeedButton, _bulletPenetrationButton, _bulletDamageButton, _bulletReloadButton, _movementSpeedButton;
    [SerializeField] List<Button> buttonsActive;
    [SerializeField] List<TMP_Text> pointCosts;

    public void SetPoint()
    {
        _pointText.text = PlayerController.Instance.UpgradePoint + " left";
    }

    public void UpdateHealthRegen(int newLevel, int maxLevel)
    {
        _healthRegenSlider.SetBar(newLevel);

        if (newLevel == maxLevel)
        {
            _healthRegenButton.interactable = false;
            buttonsActive.Remove(_healthRegenButton);
        }
        RefreshTextCost();
    }

    public void UpdateMaxHealth(int newLevel, int maxLevel)
    {
        _maxHealthSlider.SetBar(newLevel);

        if (newLevel == maxLevel)
        {
            _maxHealthButton.interactable = false;
            buttonsActive.Remove(_maxHealthButton);
        }
        RefreshTextCost();
    }

    public void UpdateBodyDamage(int newLevel, int maxLevel)
    {
        _bodyDamageSlider.SetBar(newLevel);

        if (newLevel == maxLevel)
        {
            _bodyDamageButton.interactable = false;
            buttonsActive.Remove(_bodyDamageButton);
        }
        RefreshTextCost();
    }

    public void UpdateBulletSpeed(int newLevel, int maxLevel)
    {
        _bulletSpeedSlider.SetBar(newLevel);

        if (newLevel == maxLevel)
        {
            _bulletSpeedButton.interactable = false;
            buttonsActive.Remove(_bulletSpeedButton);
        }
        RefreshTextCost();
    }

    public void UpdateBulletPenetration(int newLevel, int maxLevel)
    {
        _bulletPenetrationSlider.SetBar(newLevel);

        if (newLevel == maxLevel)
        {
            _bulletPenetrationButton.interactable = false;
            buttonsActive.Remove(_bulletPenetrationButton);
        }
        RefreshTextCost();
    }
    public void UpdateBulletDamage(int newLevel, int maxLevel)
    {
        _bulletDamageSlider.SetBar(newLevel);

        if (newLevel == maxLevel)
        {
            _bulletDamageButton.interactable = false;
            buttonsActive.Remove(_bulletDamageButton);
        }
        RefreshTextCost();
    }

    public void UpdateBulletReload(int newLevel, int maxLevel)
    {
        _bulletReloadSlider.SetBar(newLevel);

        if (newLevel == maxLevel)
        {
            _bulletReloadButton.interactable = false;
            buttonsActive.Remove(_bulletReloadButton);
        }
        RefreshTextCost();
    }

    public void UpdateMovementSpeed(int newLevel, int maxLevel)
    {
        _movementSpeedSlider.SetBar(newLevel);

        if (newLevel == maxLevel)
        {
            _movementSpeedButton.interactable = false;
            buttonsActive.Remove(_movementSpeedButton);
        }
        RefreshTextCost();
    }

    public void DesactiveAllButton()
    {
        foreach (Button button in buttonsActive)
        {
            button.interactable = false;
        }
    }

    public void ActiveAllButton()
    {
        foreach (Button button in buttonsActive)
        {
            button.interactable = true;
        }

        RefreshTextCost();
    }

    void RefreshTextCost()
    {
        if (PlayerController.Instance.UpgradePoint 
            < 
            UIManager.Instance.GameView.UpgradePointsNeedsForeachLevel[PlayerController.Instance._healthRegenLevel])
        {
            buttonsActive[0].interactable = false;
        }

        if (PlayerController.Instance.UpgradePoint
            <
            UIManager.Instance.GameView.UpgradePointsNeedsForeachLevel[PlayerController.Instance._healthLevel])
        {
            buttonsActive[1].interactable = false;
        }

        if (PlayerController.Instance.UpgradePoint
            <
            UIManager.Instance.GameView.UpgradePointsNeedsForeachLevel[PlayerController.Instance._bodyDamageLevel])
        {
            buttonsActive[2].interactable = false;
        }

        if (PlayerController.Instance.UpgradePoint
            <
            UIManager.Instance.GameView.UpgradePointsNeedsForeachLevel[PlayerController.Instance._bulletSpeedLevel])
        {
            buttonsActive[3].interactable = false;
        }

        if (PlayerController.Instance.UpgradePoint
            <
            UIManager.Instance.GameView.UpgradePointsNeedsForeachLevel[PlayerController.Instance._bulletPenetrationLevel])
        {
            buttonsActive[4].interactable = false;
        }

        if (PlayerController.Instance.UpgradePoint
            <
            UIManager.Instance.GameView.UpgradePointsNeedsForeachLevel[PlayerController.Instance._bulletDamageLevel])
        {
            buttonsActive[5].interactable = false;
        }

        if (PlayerController.Instance.UpgradePoint
            <
            UIManager.Instance.GameView.UpgradePointsNeedsForeachLevel[PlayerController.Instance._bulletReloadLevel])
        {
            buttonsActive[6].interactable = false;
        }

        if (PlayerController.Instance.UpgradePoint
            <
            UIManager.Instance.GameView.UpgradePointsNeedsForeachLevel[PlayerController.Instance._movementSpeedLevel])
        {
            buttonsActive[7].interactable = false;
        }

        pointCosts[0].text = UIManager.Instance.GameView.UpgradePointsNeedsForeachLevel[PlayerController.Instance._healthRegenLevel].ToString();
        pointCosts[1].text = UIManager.Instance.GameView.UpgradePointsNeedsForeachLevel[PlayerController.Instance._healthLevel].ToString();
        pointCosts[2].text = UIManager.Instance.GameView.UpgradePointsNeedsForeachLevel[PlayerController.Instance._bodyDamageLevel].ToString();
        pointCosts[3].text = UIManager.Instance.GameView.UpgradePointsNeedsForeachLevel[PlayerController.Instance._bulletSpeedLevel].ToString();
        pointCosts[4].text = UIManager.Instance.GameView.UpgradePointsNeedsForeachLevel[PlayerController.Instance._bulletPenetrationLevel].ToString();
        pointCosts[5].text = UIManager.Instance.GameView.UpgradePointsNeedsForeachLevel[PlayerController.Instance._bulletDamageLevel].ToString();
        pointCosts[6].text = UIManager.Instance.GameView.UpgradePointsNeedsForeachLevel[PlayerController.Instance._bulletReloadLevel].ToString();
        pointCosts[7].text = UIManager.Instance.GameView.UpgradePointsNeedsForeachLevel[PlayerController.Instance._movementSpeedLevel].ToString();
    }
}
