using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public bool TakeDamage(int amount);
    public bool TakeHeal(int amount);
}
