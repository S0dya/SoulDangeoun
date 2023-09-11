using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnManager : SingletonMonobehaviour<SpawnManager>
{
    GameObject[][] enemyPrefabs;
    [Header("Enemy")]
    [SerializeField] GameObject[] closeEnemies;
    [SerializeField] GameObject[] middleEnemies;
    [SerializeField] GameObject[] farEnemies;

    [SerializeField] Transform enemiesParent;
    [Header("Objects")]
    [SerializeField] GameObject chestPrefab;
    [SerializeField] GameObject itemChestPrefab;
    [SerializeField] Transform deadBodyParent;
    [SerializeField] Transform chestParent;
    [Header("PickableObjects")]
    [SerializeField] Transform pickableParent;
    [SerializeField] GameObject pickablePrefab;

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
    }

    public void SetPos(Tilemap walls, Room room, int sX, int eX, int sY, int eY)
    {
        curWallTilemap = walls;
        curRoom = room;

        curSpawnEnemiesTimes = 1;
        startX = sX; endX = eX; startY = sY; endY = eY;

        spawnEnemiesTimes = Random.Range(0, 3);
        currentEnemiesAmount = Random.Range(Settings.amountOfEnemiesOnLevel/2, Settings.amountOfEnemiesOnLevel);
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
        if (curSpawnEnemiesTimes >= spawnEnemiesTimes)
        {
            Vector3Int randomPos = GetRandomPositionWithoutWall();
            if (Random.Range(0, 2) == 1)
            {
                Instantiate(chestPrefab, randomPos, Quaternion.identity, chestParent);
            }
            else
            {
                GameObject itemChestObj = Instantiate(itemChestPrefab, randomPos, Quaternion.identity, chestParent);
                ItemChest itemChest = itemChestObj.GetComponent<ItemChest>();
                itemChest.type = Random.Range(0, 1);
            }
            curRoom.DrawWalls(false);
            Settings.amountOfEnemiesOnLevel++;
        }
        else
        {
            curSpawnEnemiesTimes++;
            currentEnemiesAmount = Random.Range(1, Settings.amountOfEnemiesOnLevel / curSpawnEnemiesTimes);
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


    public void DropWeapon(SO_Weapon weapon)
    {
        GameObject pickableObj = Instantiate(pickablePrefab, Player.I.transform.position, Quaternion.identity, pickableParent);
        PickableObject pickableObject = pickableObj.GetComponent<PickableObject>();
        pickableObject.type = 0;
        pickableObject.weapon = weapon;
    }


    public void Clear()
    {
        foreach (Transform enemy in enemiesParent) Destroy(enemy.gameObject);//delm
        foreach (Transform deadBody in deadBodyParent) Destroy(deadBody.gameObject);
        foreach (Transform chest in chestParent) Destroy(chest.gameObject);
        foreach (Transform pickable in pickableParent) Destroy(pickable.gameObject);
        
    }
}
