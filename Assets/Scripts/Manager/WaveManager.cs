using BaseTemplate.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoSingleton<WaveManager>
{
    [SerializeField] float delayToInitFirstWave;
    [SerializeField] float delayBetweenWave;
    [SerializeField] float timerForEachSpawnEnemy;
    [SerializeField] List<Wave> waves;
    [SerializeField] Vector2 coinPerKill;

    public int numberOfEnemyRemaining;
    private int numberOfEnemyRemainingBase;

    private int currentIndexOfWaves;
    EnemyController enemyControllerToSpawn;
    int baseOfNumberOfEnemyRemaining;

    public int CurrentIndexOfWaves { get => currentIndexOfWaves; }

    public void Init()
    {
        StartCoroutine(StartFirstWave());
    }

    public void EnnemyDie()
    {
        numberOfEnemyRemaining--;
        Debug.Log(numberOfEnemyRemaining);
        if (numberOfEnemyRemaining <= 0)
        {
            numberOfEnemyRemaining = 0;
            UIManager.Instance.GameView.AddPoint(1);
            StartCoroutine(StartNewWave());

            AudioManager.Instance.PlaySound("EndWave");
        }

        UIManager.Instance.GameView.UpdateEnemyBar();
        PlayerManager.Instance.UpdateCoins((int)Random.Range(coinPerKill.x + (currentIndexOfWaves * 2), coinPerKill.y + (currentIndexOfWaves * 2)));
        PlayerManager.Instance.UpdateEnemyKilled(1);
    }

    IEnumerator StartFirstWave()
    {
        currentIndexOfWaves = 0;

        numberOfEnemyRemainingBase += waves[currentIndexOfWaves].enemyToAdd;
        numberOfEnemyRemaining = numberOfEnemyRemainingBase;

        UIManager.Instance.GameView.InitEnemyBar();

        yield return new WaitForSeconds(delayToInitFirstWave);
        StartCoroutine(SpawnEnemys());

        UIManager.Instance.GameView.InitEnemyBar();
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

        UIManager.Instance.GameView.InitEnemyBar();
        UIManager.Instance.GameView.UpdateWaves();
    }

    IEnumerator SpawnEnemys()
    {
        baseOfNumberOfEnemyRemaining = numberOfEnemyRemaining;
        for (int i = 0; i < baseOfNumberOfEnemyRemaining; i++)
        {
            enemyControllerToSpawn = EnemyPoolManager.Instance.GetPooledEnemy(0);
            enemyControllerToSpawn.gameObject.SetActive(true);
            enemyControllerToSpawn.Init();
            enemyControllerToSpawn.transform.position = SpawnPointEnemyManager.Instance.searchBestSpawnPoint().position;
            yield return new WaitForSeconds(timerForEachSpawnEnemy);
        }
    }
}

[System.Serializable]
public class Wave
{
    public int enemyToAdd;
    public int enemyHealthToAdd;
}
