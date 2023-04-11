using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChestData", menuName = "Data/New Chest Data", order = 1)]
public class ChestData : ScriptableObject
{
    public int Price;
    public Sprite Preview;

    public List<ChestTankPool> TankPool;
}

[Serializable]
public class ChestTankPool
{
    public TankData TankData;
    public int Probabilities;
}
