using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePopup : MonoBehaviour
{
    [SerializeField] SliderBar _healthRegenSlider, _maxHealthSlider, _bodyDamageSlider, _bulletSpeedSlider, _bulletPenetrationSlider, _bulletDamageSlider, _bulletReloadSlider, _movementSpeedSlider;

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
}
