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
    [SerializeField] int numberOfChestActive;

    List<Chest> chestChoosen;
    List<Chest> chestCanBeActivate;
    int indexOfChestChoosen;
    public void Init()
    {
        chestChoosen = new List<Chest>();
        chestCanBeActivate = _chests;
        

        for (int i =0; i < numberOfChestActive; i++)
        {
            indexOfChestChoosen = Random.Range(1, _chests.Count);

            chestChoosen.Add(chestCanBeActivate[indexOfChestChoosen]);
            chestCanBeActivate.RemoveAt(indexOfChestChoosen);
        }

        foreach ( Chest chest in chestChoosen)
        {
            chest.gameObject.SetActive(true);
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

    public void GetNewChest(Chest chestDisable)
    {
        indexOfChestChoosen = Random.Range(0, chestCanBeActivate.Count);

        chestChoosen.Remove(chestDisable);
        chestChoosen.Add(chestCanBeActivate[indexOfChestChoosen]);

        chestCanBeActivate[indexOfChestChoosen].gameObject.SetActive(true);
        chestCanBeActivate[indexOfChestChoosen].Init(GetRandomChestData());

        chestCanBeActivate.RemoveAt(indexOfChestChoosen);
        
        chestCanBeActivate.Add(chestDisable);
    }
}

[Serializable]
public class ChestDataPool
{
    public ChestData ChestData;
    public int Probabilities;
}
