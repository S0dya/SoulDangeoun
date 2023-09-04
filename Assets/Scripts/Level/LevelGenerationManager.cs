using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerationManager : SingletonMonobehaviour<LevelGenerationManager>
{
    [SerializeField] GameObject startRoom;
    [SerializeField] GameObject endRoom;
    [SerializeField] GameObject[] enemiesRooms;
    [SerializeField] GameObject[] uniqueRooms;

    [SerializeField] GameObject floorTilemapObject;
    [SerializeField] GameObject wallsTilemapObject;
    Transform coridorsParentTransform;
    Tilemap floorTilemap;
    Tilemap wallsTilemap;
    [SerializeField] TileBase[] floorTiles;
    [SerializeField] TileBase[] leftWallTiles;
    [SerializeField] TileBase[] rightWallTiles;
    [SerializeField] TileBase[] downWallTiles;
    [SerializeField] TileBase[] topWallTiles;


    protected override void Awake()
    {
        base.Awake();

        coridorsParentTransform = GameObject.FindGameObjectWithTag("CoridorsParent").transform;
    }


    public void GenerateLevel()
    {
        int amountOfRooms = Random.Range(5, 5 + Settings.levelAmount);
        
        var roomsList = new List<Room>();
        var curRoomPos = Vector2Int.zero;
        var createdRoomsPositionsList = new List<Vector2Int>() { curRoomPos };

        var createdRoomsList = new List<Room>();

        GameObject startRoomObj = CreateRoom(startRoom, curRoomPos);
        Room strtRoom = startRoomObj.GetComponent<Room>();
        createdRoomsList.Add(strtRoom);

        bool isHorizontalDirection = (Random.Range(0, 2) == 0);
        for (int i = 0; i < amountOfRooms; i++)
        {
            Vector2Int prevRoomPos = curRoomPos;
            curRoomPos = ChooseDirection(createdRoomsPositionsList, curRoomPos, isHorizontalDirection);
            createdRoomsPositionsList.Add(curRoomPos);
            
            isHorizontalDirection = !isHorizontalDirection;

            GameObject roomObj = CreateRoom(i+1 == amountOfRooms ? endRoom : 
                enemiesRooms[Random.Range(0, enemiesRooms.Length)], curRoomPos);
            Room room = roomObj.GetComponent<Room>();
            FindNeighbours(createdRoomsPositionsList, curRoomPos, prevRoomPos, room);
            createdRoomsList.Add(room);
        }
        //BAKE

        GenerateCorridors(createdRoomsList, createdRoomsPositionsList);
    }


    void GenerateCorridors(List<Room> roomsList, List<Vector2Int> roomsPositionsList)
    {
        GameObject floorTilemapObj = Instantiate(floorTilemapObject, Vector3.zero, Quaternion.identity, coridorsParentTransform);
        GameObject wallsTilemapObj = Instantiate(wallsTilemapObject, Vector3.zero, Quaternion.identity, coridorsParentTransform);
        floorTilemap = floorTilemapObj.GetComponent<Tilemap>();
        wallsTilemap = wallsTilemapObj.GetComponent<Tilemap>();
        
        Debug.Log(roomsList.Count + " " + roomsPositionsList.Count);

        for (int i = 1; i < roomsList.Count; i++)
        {
            Vector2Int curPos = roomsPositionsList[i];

            if (roomsList[i].hasNeighbours[0]) DrawCorridorLeft(roomsList[i], 
                roomsList[roomsPositionsList.IndexOf(curPos + new Vector2Int(-1, 0))], curPos);
            if (roomsList[i].hasNeighbours[1]) DrawCorridorRight(roomsList[i], 
                roomsList[roomsPositionsList.IndexOf(curPos + new Vector2Int(1, 0))], curPos);
            if (roomsList[i].hasNeighbours[2]) DrawCorridorDown(roomsList[i], 
                roomsList[roomsPositionsList.IndexOf(curPos + new Vector2Int(0, -1))], curPos);
            if (roomsList[i].hasNeighbours[3]) DrawCorridorTop(roomsList[i], 
                roomsList[roomsPositionsList.IndexOf(curPos + new Vector2Int(0, 1))], curPos);
        }
    }

    

    GameObject CreateRoom(GameObject room, Vector2Int pos)
    {
        return Instantiate(room, new Vector3Int(pos.x * 30, pos.y * 20, 1), Quaternion.identity);
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

    //tilemap
    void DrawCorridorLeft(Room curRoom, Room prevRoom, Vector2Int curPos)
    {
        int startPosX = curPos.x * 30 - curRoom.halfSize.x - 1;
        int endPosLeftX = (curPos.x - 1) * 30 + prevRoom.halfSize.x;

        int startPosY = curPos.y * 20 - 2;
        int endPosLeftY = curPos.y * 20 + 1;

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
        int startPosX = curPos.x * 30 + curRoom.halfSize.x;
        int endPosLeftX = (curPos.x + 1) * 30 - prevRoom.halfSize.x - 1;

        int startPosY = curPos.y * 20 - 2;
        int endPosLeftY = curPos.y * 20 + 1;

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
        int startPosX = curPos.x * 30 - 2;
        int endPosLeftX = curPos.x * 30 + 1;

        int startPosY = curPos.y * 20 - curRoom.halfSize.y - 1;
        int endPosLeftY = (curPos.y - 1) * 20 + prevRoom.halfSize.y;

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
        int startPosX = curPos.x * 30 - 2;
        int endPosLeftX = curPos.x * 30 + 1;

        int startPosY = curPos.y * 20 + curRoom.halfSize.y;
        int endPosLeftY = (curPos.y + 1) * 20 - prevRoom.halfSize.y - 1;

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
}
