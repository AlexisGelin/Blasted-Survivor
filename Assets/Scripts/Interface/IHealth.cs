using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public bool TakeDamage(int amount);
    public void TakeHeal(int amount);
}
