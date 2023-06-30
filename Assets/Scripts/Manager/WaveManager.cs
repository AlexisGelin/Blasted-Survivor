using BaseTemplate.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoSingleton<WaveManager>
{
    [SerializeField] float _delayToInitFirstWave;
    [SerializeField] float _delayBetweenWave;
    [SerializeField] float _timerForEachSpawnEnemy;
    [SerializeField] List<Wave> _waves;
    [SerializeField] Vector2 _coinPerKill;
    [SerializeField] float _probabilityShootEnemySpawn;
    [SerializeField] float _maxProbabilityShootEnemySpawn;

    public int NumberOfEnemyRemaining;
    private int _numberOfEnemyRemainingBase;

    private int _currentIndexOfWaves;
    Enemy _enemyControllerToSpawn;
    int _baseOfNumberOfEnemyRemaining;

    public int CurrentIndexOfWaves;

    int totalHealthAdd;


    public void Init()
    {
        StartCoroutine(StartFirstWave());
    }

    public void EnnemyDie()
    {
        NumberOfEnemyRemaining--;
        if (NumberOfEnemyRemaining <= 0)
        {
            NumberOfEnemyRemaining = 0;
            UIManager.Instance.GameView.AddPoint(1);
            StartCoroutine(StartNewWave());

            AudioManager.Instance.PlaySound("EndWave");
        }

        UIManager.Instance.GameView.UpdateEnemyBar();
        PlayerManager.Instance.UpdateCoins((int)Random.Range(_coinPerKill.x + (_currentIndexOfWaves * 2), _coinPerKill.y + (_currentIndexOfWaves * 2)));
        PlayerManager.Instance.UpdateEnemyKilled(1);
    }

    IEnumerator StartFirstWave()
    {
        _currentIndexOfWaves = 0;

        _numberOfEnemyRemainingBase += _waves[_currentIndexOfWaves].enemyToAdd;
        NumberOfEnemyRemaining = _numberOfEnemyRemainingBase;

        UIManager.Instance.GameView.InitEnemyBar();

        yield return new WaitForSeconds(_delayToInitFirstWave);
        StartCoroutine(SpawnEnemys());

        UIManager.Instance.GameView.InitEnemyBar();
    }

    IEnumerator StartNewWave()
    {
        _currentIndexOfWaves++;
        CurrentIndexOfWaves++;
        if (_currentIndexOfWaves >= _waves.Count)
        {
            _currentIndexOfWaves = 0;
        }
        totalHealthAdd += _waves[_currentIndexOfWaves].enemyHealthToAdd;
        yield return new WaitForSeconds(_delayBetweenWave);
        _numberOfEnemyRemainingBase += _waves[_currentIndexOfWaves].enemyToAdd;
        NumberOfEnemyRemaining = _numberOfEnemyRemainingBase;
        StartCoroutine(SpawnEnemys());

        UIManager.Instance.GameView.InitEnemyBar();
        UIManager.Instance.GameView.UpdateWaves();
    }

    IEnumerator SpawnEnemys()
    {
        _baseOfNumberOfEnemyRemaining = NumberOfEnemyRemaining;
        for (int i = 0; i < _baseOfNumberOfEnemyRemaining; i++)
        {
            if (Random.Range(0, 100) <= _probabilityShootEnemySpawn*100)
                _enemyControllerToSpawn = EnemyPoolManager.Instance.GetPooledEnemy(1);
            else
                _enemyControllerToSpawn = EnemyPoolManager.Instance.GetPooledEnemy(0);

            _enemyControllerToSpawn.gameObject.SetActive(true);
            _enemyControllerToSpawn.Init(totalHealthAdd);
            _enemyControllerToSpawn.transform.position = SpawnPointEnemyManager.Instance.searchBestSpawnPoint().position;
            yield return new WaitForSeconds(_timerForEachSpawnEnemy);
        }
    }
}

[System.Serializable]
public class Wave
{
    public int enemyToAdd;
    public int enemyHealthToAdd;
}
