using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnManager : SingletonMonobehaviour<SpawnManager>
{
    GameObject[][] enemyPrefabs;
    [SerializeField] GameObject[] closeEnemies;
    [SerializeField] GameObject[] middleEnemies;
    [SerializeField] GameObject[] farEnemies;
    
    [SerializeField] GameObject chestPrefab;

    Transform enemiesParent;
    Transform chestParent;

    int startX;
    int endX;
    int startY;
    int endY;

    int spawnEnemiesTimes;
    int curSpawnEnemiesTimes;

    Room curRoom;
    Tilemap curWallTilemap;

    [HideInInspector] public int currentEnemiesAmount;

    protected override void Awake()
    {
        base.Awake();

        enemyPrefabs = new GameObject[3][];
        enemyPrefabs[0] = closeEnemies;
        enemyPrefabs[1] = middleEnemies;
        enemyPrefabs[2] = farEnemies;

        enemiesParent = GameObject.FindGameObjectWithTag("EnemiesParent").transform;
        chestParent = GameObject.FindGameObjectWithTag("ChestParent").transform;
    }

    public void SetPos(Tilemap walls, Room room, int sX, int eX, int sY, int eY)
    {
        curWallTilemap = walls;
        curRoom = room;

        curSpawnEnemiesTimes = 0;
        startX = sX; endX = eX; startY = sY; endY = eY;

        spawnEnemiesTimes = Random.Range(0, 5);
        currentEnemiesAmount = Settings.amountOfEnemiesOnLevel;
        SpawnEnemies(currentEnemiesAmount);
    }

    public void SpawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++) 
        {
            Vector3Int randomPos = GetRandomPositionWithoutWall();
            int type = Random.Range(0, enemyPrefabs.Length);
            int index = Random.Range(0, enemyPrefabs[type].Length);

            Instantiate(enemyPrefabs[type][index], randomPos, Quaternion.identity, enemiesParent);
        }
    }

    public void SpawnMoreChecck()
    {
        if (curSpawnEnemiesTimes == spawnEnemiesTimes)
        {
            Vector3Int randomPos = GetRandomPositionWithoutWall();
            Instantiate(chestPrefab, randomPos, Quaternion.identity, chestParent);
            curRoom.DrawWalls(false);
            Settings.amountOfEnemiesOnLevel++;
        }
        else
        {
            curSpawnEnemiesTimes++;
            currentEnemiesAmount = Settings.amountOfEnemiesOnLevel/curSpawnEnemiesTimes;
            SpawnEnemies(currentEnemiesAmount);
        }
    }

    Vector3Int GetRandomPositionWithoutWall()
    {
        while (true)
        {
            Vector3Int randomPosition = new Vector3Int(Random.Range(startX, endX), Random.Range(startY, endY), 0);

            if (curWallTilemap.GetTile(randomPosition) == null)
            {
                return randomPosition;
            }
        }
    }
}
