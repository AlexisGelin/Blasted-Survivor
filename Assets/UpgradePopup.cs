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

    public void SetPoint()
    {
        _pointText.text = PlayerController.Instance.UpgradePoint + " left";
    }

    public void UpdateHealthRegen(int newLevel)
    {
        _healthRegenSlider.SetBar(newLevel);
    }

    public void UpdateMaxHealth(int newLevel)
    {
        _maxHealthSlider.SetBar(newLevel);
    }

    public void UpdateBodyDamage(int newLevel)
    {
        _bodyDamageSlider.SetBar(newLevel);
    }

    public void UpdateBulletSpeed(int newLevel)
    {
        _bulletSpeedSlider.SetBar(newLevel);
    }

    public void UpdateBulletPenetration(int newLevel)
    {
        _bulletPenetrationSlider.SetBar(newLevel);
    }
    public void UpdateBulletDamage(int newLevel)
    {
        _bulletDamageSlider.SetBar(newLevel);
    }

    public void UpdateBulletReload(int newLevel)
    {
        _bulletReloadSlider.SetBar(newLevel);
    }

    public void UpdateMovementSpeed(int newLevel)
    {
        _movementSpeedSlider.SetBar(newLevel);
    }

    public void DesactiveAllButton()
    {
        _healthRegenButton.interactable = false;
        _maxHealthButton.interactable = false;
        _bodyDamageButton.interactable = false;
        _bulletSpeedButton.interactable = false;
        _bulletPenetrationButton.interactable = false;
        _bulletDamageButton.interactable = false;
        _bulletReloadButton.interactable = false;
        _movementSpeedButton.interactable = false;
    }

    public void ActiveAllButton()
    {
        _healthRegenButton.interactable = true;
        _maxHealthButton.interactable = true;
        _bodyDamageButton.interactable = true;
        _bulletSpeedButton.interactable = true;
        _bulletPenetrationButton.interactable = true;
        _bulletDamageButton.interactable = true;
        _bulletReloadButton.interactable = true;
        _movementSpeedButton.interactable = true;
    }
}
