using BaseTemplate.Behaviours;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldManager : MonoSingleton<WorldManager>
{
    [SerializeField] List<Chest> _chests;
    [SerializeField] List<ChestDataPool> _chestsData;

    public void Init()
    {
        foreach( Chest chest in _chests)
        {
            chest.Init(GetRandomChestData());
        }
    }
    ChestData GetRandomChestData()
    {
        int maxWeight = 0;
        int tmpWeight = 0;

        foreach (var chestData in _chestsData)
        {
            maxWeight += chestData.Probabilities;
        }

        int choice = Random.Range(0, maxWeight);

        foreach (var tank in _chestsData)
        {
            if (choice >= tmpWeight && choice < tmpWeight)
            {
                return tank.ChestData;
            }

            tmpWeight += tank.Probabilities;
        }

        return _chestsData.Last().ChestData;
    }
}

[Serializable]
public class ChestDataPool
{
    public ChestData ChestData;
    public int Probabilities;
}
