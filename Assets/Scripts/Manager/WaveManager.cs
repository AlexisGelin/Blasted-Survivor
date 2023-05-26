using BaseTemplate.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoSingleton<WaveManager>
{
    [SerializeField] float delayToInitFirstWave;
    [SerializeField] float delayBetweenWave;
    [SerializeField] List<Wave> waves;

    public int numberOfEnemyRemaining;
    private int numberOfEnemyRemainingBase;

    private int currentIndexOfWaves;
    EnemyController enemyControllerToSpawn;

    public void Init()
    {
        StartCoroutine(StartFirstWave());
    }

    public void EnnemyDie()
    {
        numberOfEnemyRemaining--;
        if (numberOfEnemyRemaining <= 0)
        {
            StartCoroutine(StartNewWave());
        }
    }
    

    IEnumerator StartFirstWave()
    {
        currentIndexOfWaves = 0;

        numberOfEnemyRemainingBase += waves[currentIndexOfWaves].enemyToAdd;
        numberOfEnemyRemaining = numberOfEnemyRemainingBase;
        yield return new WaitForSeconds(delayToInitFirstWave);
        StartCoroutine(SpawnEnemys());

    }

    IEnumerator StartNewWave()
    {
        currentIndexOfWaves++;
        if (currentIndexOfWaves >= waves.Count)
        {
            currentIndexOfWaves = 0;
        }
        yield return new WaitForSeconds(delayBetweenWave);
        numberOfEnemyRemainingBase += waves[currentIndexOfWaves].enemyToAdd;
        numberOfEnemyRemaining = numberOfEnemyRemainingBase;
        StartCoroutine(SpawnEnemys());
    }

    IEnumerator SpawnEnemys()
    {
        for (int i = 0; i < numberOfEnemyRemaining; i++)
        {
            enemyControllerToSpawn = EnemyPoolManager.Instance.GetPooledEnemy(0);
            enemyControllerToSpawn.Init();
            enemyControllerToSpawn.gameObject.SetActive(true);
            yield return new WaitForEndOfFrame();
        }
    }

    
}

[System.Serializable] 
public class Wave
{
    public int enemyToAdd;
    public int enemyHealthToAdd;
}
