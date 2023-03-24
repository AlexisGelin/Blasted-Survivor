using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TankData", menuName = "Data/New Tank Data", order = 1)]
public class TankData : ScriptableObject
{
    public string Name;
    public int Health, BodyDamage, Speed, ExpOnDestroy;
    public float FireRate, HealthRegeneration;
    public BulletData Bullet;
    public TankRenderer Renderer;

    public void ResetData()
    {
        Health = 0;
        HealthRegeneration = 0;
        BodyDamage = 0;
        Speed = 0;
        FireRate = 0;
        Bullet.Damage = 0;
        Bullet.Speed = 0;
        Bullet.Penetration = 0;
    }
}
