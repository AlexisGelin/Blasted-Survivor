using BaseTemplate.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolEnemy
{

    public EnemyController objectToPool;
    public int amountToPool;
    public bool shouldExpand = true;

    public ObjectPoolEnemy(EnemyController obj, int amt, bool exp = true)
    {
        objectToPool = obj;
        amountToPool = Mathf.Max(amt, 2);
        shouldExpand = exp;
    }
}

public class EnemyPoolManager : MonoSingleton<EnemyPoolManager>
{

    public List<ObjectPoolEnemy> enemysToPool;
    public List<List<EnemyController>> pooledEnemysList;

    public List<EnemyController> pooledEnemys;
    private List<int> positions;
    public void Init()
    {
        //enemysToPool  = new List<ObjectPoolEnemy>();
        pooledEnemysList = new List<List<EnemyController>>();
        positions = new List<int>();

        for (int i = 0; i < enemysToPool.Count; i++)
        {
            ObjectPoolItemToPooledEnemy(i);
        }
    }

    public EnemyController GetPooledEnemy(int index)
    {

        int curSize = pooledEnemysList[index].Count;
        for (int i = positions[index] + 1; i < positions[index] + pooledEnemysList[index].Count; i++)
        {

            if (!pooledEnemysList[index][i % curSize].gameObject.activeInHierarchy)
            {
                positions[index] = i % curSize;
                return pooledEnemysList[index][i % curSize];
            }
        }

        if (enemysToPool[index].shouldExpand)
        {

            EnemyController obj = Instantiate(enemysToPool[index].objectToPool);
            obj.gameObject.SetActive(false);
            obj.transform.parent = this.transform;
            pooledEnemysList[index].Add(obj);
            return obj;

        }
        return null;
    }

    void ObjectPoolItemToPooledEnemy(int index)
    {
        ObjectPoolEnemy item = enemysToPool[index];

        pooledEnemys = new List<EnemyController>();
        for (int i = 0; i < item.amountToPool; i++)
        {
            EnemyController obj = Instantiate(item.objectToPool);
            obj.gameObject.SetActive(false);
            obj.transform.parent = this.transform;
            pooledEnemys.Add(obj);
        }
        pooledEnemysList.Add(pooledEnemys);
        positions.Add(0);

    }

}
