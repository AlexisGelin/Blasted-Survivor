using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] TankData _data;
    [SerializeField] int _health;


    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _canonTransform;

    public Rigidbody2D Rb { get => _rb;  }

    public void Init()
    {
        _health = _data.Health;
    }

    public void TakeDamage(int amount)
    {
        _health -= amount;

        if (_health < 0)
        {
            Destroy(gameObject);
        }
    }
}
