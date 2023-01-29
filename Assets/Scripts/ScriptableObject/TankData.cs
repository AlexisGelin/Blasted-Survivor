using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TankData", menuName = "Data/New Tank Data", order = 1)]
public class TankData : ScriptableObject
{
    public string Name;
    public int Health, HealthRegeneration, BodyDamage, Speed;
    public float FireRate;
    public BulletData Bullet;
    public TankRenderer Renderer;
}
