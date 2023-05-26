using BaseTemplate.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointEnemyManager : MonoSingleton<SpawnPointEnemyManager>
{
    public List<Transform> enemySpawnPoints;
    [SerializeField] float offsetDistanceToSpawnEnemy;
    [SerializeField] int numberOfCurrentSpawnPointActive;

    List<System.Tuple<float, int>> tempBestSpawnPoint;
    System.Tuple<float, int> currentTuple;
    int currentIndexSpawnPoint;
    public void Init()
    {
        tempBestSpawnPoint = new List<System.Tuple<float, int>>();
    }

    public Transform searchBestSpawnPoint()
    {
        tempBestSpawnPoint.Clear();
        for (int i = 0; i < enemySpawnPoints.Count; i++)
        {
            if (Vector2.Distance(enemySpawnPoints[i].position, 
                PlayerController.Instance.transform.position) >= offsetDistanceToSpawnEnemy)
            {
                currentTuple = System.Tuple.Create(
                    Vector2.Distance(enemySpawnPoints[i].position,PlayerController.Instance.transform.position),i);

                tempBestSpawnPoint.Add(currentTuple);
            }
        }

        tempBestSpawnPoint.Sort((x,y) => x.Item1.CompareTo(y.Item1));
        currentIndexSpawnPoint = Random.Range(0, numberOfCurrentSpawnPointActive);

        while (currentIndexSpawnPoint >= tempBestSpawnPoint.Count)
        {
            currentIndexSpawnPoint--;
        }

        return enemySpawnPoints[tempBestSpawnPoint[currentIndexSpawnPoint].Item2];
    }
}
