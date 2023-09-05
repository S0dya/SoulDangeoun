using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : SingletonMonobehaviour<SpawnManager>
{
    [SerializeField] GameObject EnemyPrefab;

    Transform enemiesParent;

    protected override void Awake()
    {
        base.Awake();

        enemiesParent = GameObject.FindGameObjectWithTag("EnemiesParent").transform;
    }

    public void SpawnEnemies(float startX, float endX, float startY, float endY)
    {
        Vector3 randomPos = new Vector3(Random.Range(startX, endX), Random.Range(startY, endY), 0);
        Instantiate(EnemyPrefab, randomPos, Quaternion.identity, enemiesParent);
    }
}
