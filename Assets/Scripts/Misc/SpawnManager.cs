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
    [Header("SO")]
    [SerializeField] SO_Weapon[] weapons;
    [SerializeField] SO_Potion[] potionsLittle;
    [SerializeField] SO_Potion[] potionsBig;

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
        pickableObject.weapon = weapon;
    }


    public void Clear()
    {
        foreach (Transform enemy in enemiesParent) Destroy(enemy.gameObject);//delm
        foreach (Transform deadBody in deadBodyParent) Destroy(deadBody.gameObject);
        foreach (Transform chest in chestParent) Destroy(chest.gameObject);
        foreach (Transform pickable in pickableParent) Destroy(pickable.gameObject);
    }

    //spawn pickable objects
    public void SpawnPickableShopItems()
    {
        GameObject[] pickableShopItems = GameObject.FindGameObjectsWithTag("PickableShopItem");
        foreach (GameObject item in pickableShopItems)
        {
            SpawnPickable(item.transform.position, (Random.Range(0, 6) < 4 ? 0 : 1), true);
        }
    }

    public void SpawnPickable(Vector2 pos, int type, bool setPrice)
    {
        GameObject pickableObj = Instantiate(pickablePrefab, pos, Quaternion.identity, pickableParent);
        PickableObject pickable = pickableObj.GetComponent<PickableObject>();
        int index = 0;

        switch (type)
        {
            case 0:
                index = Random.Range(0, weapons.Length);
                pickable.weapon = weapons[index];
                if (setPrice) pickable.price = (int)(weapons[index].Price * Settings.priceMultiplaer);
                break;
            case 1:
                if (Random.Range(0, 4) != 3)
                {
                    index = Random.Range(0, potionsLittle.Length);
                    pickable.potion = potionsLittle[index];
                    if (setPrice) pickable.price = (int)(potionsLittle[index].Price * Settings.priceMultiplaer);
                }
                else
                {
                    index = Random.Range(0, potionsBig.Length);
                    pickable.potion = potionsBig[index];
                    if (setPrice) pickable.price = (int)(potionsBig[index].Price * Settings.priceMultiplaer);
                }
                break;
            default: break;
        }
    }
}
