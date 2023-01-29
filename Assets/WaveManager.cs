using BaseTemplate.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoSingleton<WaveManager>
{
    public void Init()
    {
        StartCoroutine(StartGeneration());
    }

    IEnumerator StartGeneration()
    {
        while (true)
        {
            GameObject enemyGO = PoolManager.Instance.GetPooledObject(1);
            EnemyController enemyController = enemyGO.GetComponent<EnemyController>();
            enemyController.Init();
            enemyGO.SetActive(true);
            yield return new WaitForSeconds(1f);
        }
    }
}
