using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public Transform[] points;
    public GameObject enemy;
    public float createTime = 2.0f;
    public int maxEnemy = 10;
    public static int curEnemy;
    public List<GameObject> EnemyPool = new List<GameObject>();

    private void Awake()
    {
        CreatePooling();
    }

   

    void Start()
    {
        
        points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();

        if(points.Length > 0)
        {
            StartCoroutine(this.CreateEnemy());
            curEnemy = maxEnemy;
        }
    }

    public GameObject GetEnemy()
    {
        for(int i = 0; i < EnemyPool.Count; i++)
        {
            if(EnemyPool[i].activeSelf == false)
            {
                return EnemyPool[i];
            }
        }
        return null;
    }

    private void CreatePooling()
    {
        GameObject objectPools = new GameObject("ObjectPools");

        for(int i = 0; i < maxEnemy; i++)
        {
            var obj = Instantiate<GameObject>(enemy, objectPools.transform);
            obj.name = "Enemy_" + i.ToString("00");
            obj.SetActive(false);
            EnemyPool.Add(obj);
        }
    }

    IEnumerator CreateEnemy()
    {
        int enemyCount = (int)GameObject.FindGameObjectsWithTag("Enemy").Length;
        while (enemyCount < maxEnemy)
        {
            
            if(enemyCount < maxEnemy)
            {
                yield return new WaitForSeconds(createTime);

                int idx = Random.Range(1, points.Length);
                var enemy = GetEnemy();
                if(enemy != null)
                {
                    enemy.transform.position = points[idx].position;
                    enemy.transform.rotation = points[idx].rotation;
                    enemy.SetActive(true);

                }
            }
            else
            {
                yield return null;
            }

        }
    }
}
