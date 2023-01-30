using BaseTemplate.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankManager : MonoSingleton<TankManager>
{
    [SerializeField] List<TankData> tankDatas = new List<TankData>();

    Dictionary<string, TankData> tankDataDictionary = new Dictionary<string, TankData>();

    public void Init()
    {
        foreach (var tankData in tankDatas)
        {
            tankDataDictionary.Add(tankData.Name, tankData);
        }
    }

    public TankData GetTankData(string tankName)
    {
        return tankDataDictionary[tankName];
    }

}
