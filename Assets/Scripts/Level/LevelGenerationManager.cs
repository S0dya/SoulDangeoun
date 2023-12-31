using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using NavMeshPlus.Components;

public class LevelGenerationManager : SingletonMonobehaviour<LevelGenerationManager>
{
    [SerializeField] Player player;

    [SerializeField] NavMeshSurface surface;
    [SerializeField] SpawnManager spawnManager;

    [SerializeField] GameObject hubRoom;

    [Header("Level : 0.0")]
    //Realm0.0
    [SerializeField] GameObject startRoomForest;
    [SerializeField] GameObject endRoomForest;
    [SerializeField] GameObject[] enemiesRoomsForest;
    [SerializeField] GameObject[] uniqueRoomsForest;
    [SerializeField] TileBase[] floorTilesForest;

    [Header("Level : 0.1")]
    //Realm0.1
    [SerializeField] GameObject startRoomDungeon;
    [SerializeField] GameObject endRoomDungeon;
    [SerializeField] GameObject[] enemiesRoomsDungeon;
    [SerializeField] GameObject[] uniqueRoomsDungeon;
    [SerializeField] TileBase[] floorTilesDungeon;


    //Realm1.0
    [Header("Tiles")]
    [SerializeField] GameObject floorTilemapPrefab;
    [SerializeField] GameObject wallsTilemapPrefab;
    [SerializeField] Transform levelsParentTransform;
    [SerializeField] Transform coridorsParentTransform;
    Tilemap floorTilemap;
    Tilemap wallsTilemap;
    [SerializeField] TileBase[] leftWallTiles;
    [SerializeField] TileBase[] rightWallTiles;
    [SerializeField] TileBase[] downWallTiles;
    [SerializeField] TileBase[] topWallTiles;

    //UI
    [Header("UI")]
    [SerializeField] GameObject levelUIPrefab;
    [SerializeField] Transform mapParent;

    //local
    List<Room> createdRoomsList = new List<Room>();
    public List<LevelUI> createdLevelsUIList = new List<LevelUI>();

    GameObject startRoom;
    GameObject endRoom;
    GameObject[] enemiesRooms;
    GameObject[] uniqueRooms;
    TileBase[] floorTiles;

    protected override void Awake()
    {
        base.Awake();

    }

    void Start()
    {
        Settings.curentEarntCrystals = 0;
        SpawnHubRoom();
        LoadingSceneManager.I.CloseLoadingScreen();
    }

    void SpawnHubRoom()
    {
        Instantiate(hubRoom, Vector2.zero, Quaternion.identity, levelsParentTransform);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            GenerateLevel();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            surface.BuildNavMeshAsync();
        }
    }

    //methods
    public void GenerateLevel()
    {
        LoadingSceneManager.I.OpenLoadingScreen();
        Clear();
        SetCurRealm();
        int amountOfRooms = Random.Range(5, 5 + Settings.levelAmount);
        var curRoomPos = Vector2Int.zero;
        var createdRoomsPositionsList = new List<Vector2Int>() { curRoomPos };

        GameObject startRoomObj = CreateRoom(startRoom, curRoomPos);
        Room strtRoom = startRoomObj.GetComponent<Room>();
        createdRoomsList.Add(strtRoom);

        bool isHorizontalDirection = (Random.Range(0, 2) == 0);
        LoadingSceneManager.I.SetFillAmount(0.1f);
        for (int i = 0; i < amountOfRooms; i++)
        {
            Vector2Int prevRoomPos = curRoomPos;
            curRoomPos = ChooseDirection(createdRoomsPositionsList, curRoomPos, isHorizontalDirection);
            createdRoomsPositionsList.Add(curRoomPos);
            
            isHorizontalDirection = !isHorizontalDirection;

            GameObject roomObj = CreateRoom(i+1 == amountOfRooms ? endRoom :
                (Random.Range(0, 7) == 6) ? uniqueRooms[Random.Range(0, uniqueRooms.Length)] : 
                enemiesRooms[Random.Range(0, enemiesRooms.Length)], curRoomPos);
            Room room = roomObj.GetComponent<Room>();
            FindNeighbours(createdRoomsPositionsList, curRoomPos, prevRoomPos, room);
            room.index = i + 1;
            createdRoomsList.Add(room);
        }

        LoadingSceneManager.I.SetFillAmount(0.4f);
        GenerateCorridors(createdRoomsPositionsList);

        LoadingSceneManager.I.SetFillAmount(0.6f);
        Invoke("NavMeshBake", 0.1f);
        Invoke("StopLoadingLevel", 0.3f);
    }


    GameObject CreateRoom(GameObject room, Vector2Int pos)
    {
        var spawnPos = new Vector3Int(pos.x * Settings.width, pos.y * Settings.height, 1);

        GameObject levelUIObj = Instantiate(levelUIPrefab, mapParent);
        levelUIObj.transform.localPosition = (Vector2)pos * 15;
        LevelUI levelUI = levelUIObj.GetComponent<LevelUI>();
        createdLevelsUIList.Add(levelUI);

        return Instantiate(room, spawnPos, Quaternion.identity, levelsParentTransform);
    }

    Vector2Int ChooseDirection(List<Vector2Int> list, Vector2Int curPos, bool isHorizontalDirection)
    {
        int direction = (Random.Range(0, 2) == 0 ? 1 : -1);
        if (isHorizontalDirection)
        {
            curPos.x += direction;
            if (list.Contains(curPos))
            {
                curPos.x += (direction == 1 ? -2 : 2);
            }
        }
        else
        {
            curPos.y += direction;
            if (list.Contains(curPos))
            {
                curPos.y += (direction == 1 ? -2 : 2);
            }
        }

        return curPos;
    }
    
    void FindNeighbours(List<Vector2Int> list, Vector2Int curPos, Vector2Int prevPos, Room room)
    {
        var directionsArr = new Vector2Int[4]
        {
            new Vector2Int(curPos.x - 1, curPos.y),
            new Vector2Int(curPos.x + 1, curPos.y),
            new Vector2Int(curPos.x, curPos.y - 1),
            new Vector2Int(curPos.x, curPos.y + 1),
        };

        for (int i = 0; i < directionsArr.Length; i++)
        {
            if (list.Contains(directionsArr[i]))
            {
                room.hasNeighbours[i] = true;
            }
        }
    }


    void GenerateCorridors(List<Vector2Int> roomsPositionsList)
    {
        GameObject floorTilemapObject = Instantiate(floorTilemapPrefab, Vector3.zero, Quaternion.identity, coridorsParentTransform);
        GameObject wallsTilemapObject = Instantiate(wallsTilemapPrefab, Vector3.zero, Quaternion.identity, coridorsParentTransform);
        floorTilemap = floorTilemapObject.GetComponent<Tilemap>();
        wallsTilemap = wallsTilemapObject.GetComponent<Tilemap>();
        
        for (int i = 1; i < createdRoomsList.Count; i++)
        {
            int index = 0;
            Vector2Int curPos = roomsPositionsList[i];

            if (createdRoomsList[i].hasNeighbours[0])
            {
                index = roomsPositionsList.IndexOf(curPos + new Vector2Int(-1, 0));
                DrawCorridorLeft(createdRoomsList[i], createdRoomsList[index], curPos);
                createdRoomsList[index].hasNeighbours[1] = true;
            }
            if (createdRoomsList[i].hasNeighbours[1])
            {
                index = roomsPositionsList.IndexOf(curPos + new Vector2Int(1, 0));
                DrawCorridorRight(createdRoomsList[i], createdRoomsList[index], curPos);
                createdRoomsList[index].hasNeighbours[0] = true;
            }
            if (createdRoomsList[i].hasNeighbours[2])
            {
                index = roomsPositionsList.IndexOf(curPos + new Vector2Int(0, -1));
                DrawCorridorDown(createdRoomsList[i], createdRoomsList[index], curPos);
                createdRoomsList[index].hasNeighbours[3] = true;
            }
            if (createdRoomsList[i].hasNeighbours[3])
            {
                index = roomsPositionsList.IndexOf(curPos + new Vector2Int(0, 1));
                DrawCorridorTop(createdRoomsList[i], createdRoomsList[index], curPos);
                createdRoomsList[index].hasNeighbours[2] = true;
            }
        }
    }

    void NavMeshBake()
    {
        LoadingSceneManager.I.SetFillAmount(0.8f);
        surface.BuildNavMeshAsync();
        
        DrawWallsForLevels();
    }
    
    void DrawWallsForLevels()
    {
        foreach (Room room in createdRoomsList)
        {
            room.DrawWalls(false);
        }
        LoadingSceneManager.I.SetFillAmount(1);
    }

    void StopLoadingLevel()
    {
        spawnManager.SpawnPickableShopItems();
        LoadingSceneManager.I.CloseLoadingScreen();
    }

    //tilemap
    void DrawCorridorLeft(Room curRoom, Room prevRoom, Vector2Int curPos)
    {
        int startPosX = curPos.x * Settings.width - curRoom.halfSize.x - 1;
        int endPosLeftX = (curPos.x - 1) * Settings.width + prevRoom.halfSize.x;

        int startPosY = curPos.y * Settings.height - 2;
        int endPosLeftY = curPos.y * Settings.height + 1;

        for (int x = startPosX; x >= endPosLeftX; x--)
        {
            wallsTilemap.SetTile(new Vector3Int(x, startPosY, 0), topWallTiles[Random.Range(0, topWallTiles.Length)]);
            for (int y = startPosY; y <= endPosLeftY; y++)
            {
                floorTilemap.SetTile(new Vector3Int(x, y, 0), floorTiles[Random.Range(0, floorTiles.Length)]);
            }
            wallsTilemap.SetTile(new Vector3Int(x, endPosLeftY, 0), downWallTiles[Random.Range(0, downWallTiles.Length)]);
        }
    }
    void DrawCorridorRight(Room curRoom, Room prevRoom, Vector2Int curPos)
    {
        int startPosX = curPos.x * Settings.width + curRoom.halfSize.x;
        int endPosLeftX = (curPos.x + 1) * Settings.width - prevRoom.halfSize.x - 1;

        int startPosY = curPos.y * Settings.height - 2;
        int endPosLeftY = curPos.y * Settings.height + 1;

        for (int x = startPosX; x <= endPosLeftX; x++)
        {
            wallsTilemap.SetTile(new Vector3Int(x, startPosY, 0), topWallTiles[Random.Range(0, topWallTiles.Length)]);
            for (int y = startPosY; y <= endPosLeftY; y++)
            {
                floorTilemap.SetTile(new Vector3Int(x, y, 0), floorTiles[Random.Range(0, floorTiles.Length)]);
            }
            wallsTilemap.SetTile(new Vector3Int(x, endPosLeftY, 0), downWallTiles[Random.Range(0, downWallTiles.Length)]);
        }
    }
    void DrawCorridorDown(Room curRoom, Room prevRoom, Vector2Int curPos)
    {
        int startPosX = curPos.x * Settings.width - 2;
        int endPosLeftX = curPos.x * Settings.width + 1;

        int startPosY = curPos.y * Settings.height - curRoom.halfSize.y - 1;
        int endPosLeftY = (curPos.y - 1) * Settings.height + prevRoom.halfSize.y;

        for (int y = startPosY; y >= endPosLeftY; y--)
        {
            wallsTilemap.SetTile(new Vector3Int(startPosX, y, 0), rightWallTiles[Random.Range(0, rightWallTiles.Length)]);
            for (int x = startPosX; x <= endPosLeftX; x++)
            {
                floorTilemap.SetTile(new Vector3Int(x, y, 0), floorTiles[Random.Range(0, floorTiles.Length)]);
            }
            wallsTilemap.SetTile(new Vector3Int(endPosLeftX, y, 0), leftWallTiles[Random.Range(0, leftWallTiles.Length)]);
        }
    }
    void DrawCorridorTop(Room curRoom, Room prevRoom, Vector2Int curPos)
    {
        int startPosX = curPos.x * Settings.width - 2;
        int endPosLeftX = curPos.x * Settings.width + 1;

        int startPosY = curPos.y * Settings.height + curRoom.halfSize.y;
        int endPosLeftY = (curPos.y + 1) * Settings.height - prevRoom.halfSize.y - 1;

        for (int y = startPosY; y <= endPosLeftY; y++)
        {
            wallsTilemap.SetTile(new Vector3Int(startPosX, y, 0), rightWallTiles[Random.Range(0, rightWallTiles.Length)]);
            for (int x = startPosX; x <= endPosLeftX; x++)
            {
                floorTilemap.SetTile(new Vector3Int(x, y, 0), floorTiles[Random.Range(0, floorTiles.Length)]);
            }
            wallsTilemap.SetTile(new Vector3Int(endPosLeftX, y, 0), leftWallTiles[Random.Range(0, leftWallTiles.Length)]);
        }
    }

    void SetCurRealm()
    {
        Settings.curDungeonRealm = Random.Range(0, 
            (Settings.curDungeonLevel == 0 ? 2 : 1));

        switch (Settings.curDungeonLevel)
        {
            case 0:
                switch (Settings.curDungeonRealm)
                {
                    case 0:

                        startRoom = startRoomForest;
                        endRoom = endRoomForest;
                        enemiesRooms = enemiesRoomsForest;
                        uniqueRooms = uniqueRoomsForest;
                        floorTiles = floorTilesForest;
                        break;
                    case 1:
                        startRoom = startRoomDungeon;
                        endRoom = endRoomDungeon;
                        enemiesRooms = enemiesRoomsDungeon;
                        uniqueRooms = uniqueRoomsDungeon;
                        floorTiles = floorTilesDungeon;
                        break;
                    default:
                        break;
                }
                break;
            case 1:
                switch (Settings.curDungeonRealm)
                {
                    case 0:
                        startRoom = startRoomForest;
                        endRoom = endRoomForest;
                        enemiesRooms = enemiesRoomsForest;
                        uniqueRooms = uniqueRoomsForest;
                        break;
                    case 1:
                        startRoom = startRoomDungeon;
                        endRoom = endRoomDungeon;
                        enemiesRooms = enemiesRoomsDungeon;
                        uniqueRooms = uniqueRoomsDungeon;
                        break;
                    default:
                        break;
                }
                break;
            default: break;
        }
    }


    void Clear()
    {   
        SpawnManager.I.Clear();
        foreach (Transform room in levelsParentTransform) Destroy(room.gameObject);
        createdRoomsList = new List<Room>();
        foreach (LevelUI level in createdLevelsUIList) level.DestroyObject();
        createdLevelsUIList = new List<LevelUI>();

        Destroy(GameObject.FindGameObjectWithTag("CoridorsFloor"));
        Destroy(GameObject.FindGameObjectWithTag("CoridorsWalls"));
        player.Clear();
    }
}
