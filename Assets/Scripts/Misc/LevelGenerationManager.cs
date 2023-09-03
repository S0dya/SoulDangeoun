using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerationManager : SingletonMonobehaviour<LevelGenerationManager>
{
    [SerializeField] GameObject startRoom;
    [SerializeField] GameObject endRoom;
    [SerializeField] GameObject[] enemiesRooms;
    [SerializeField] GameObject[] uniqueRooms;

    protected override void Awake()
    {
        base.Awake();

    }


    public void GenerateLevel()
    {
        int amountOfRooms = Random.Range(5, 5 + Settings.levelAmount);
        
        var roomsList = new List<SO_Room>();
        var curRoomPos = Vector2Int.zero;
        var createdRoomsPostionsList = new List<Vector2Int>() { curRoomPos };

        var createdRoomsList = new List<SO_Room>();

        CreateRoom(startRoom, curRoomPos);

        bool isHorizontalDirection = (Random.Range(0, 2) == 0);
        for (int i = 0; i < amountOfRooms; i++)
        {
            Vector2Int prevRoomPos = curRoomPos;
            curRoomPos = ChooseDirection(createdRoomsPostionsList, curRoomPos, isHorizontalDirection);
            createdRoomsPostionsList.Add(curRoomPos);
            
            isHorizontalDirection = !isHorizontalDirection;

            GameObject roomObj = CreateRoom(startRoom, curRoomPos);
            SO_Room room = roomObj.GetComponent<SO_Room>();
            FindNeighbours(createdRoomsPostionsList, curRoomPos, prevRoomPos, room);
            createdRoomsList.Add(room);
        }

        foreach (SO_Room room in createdRoomsList)
        {
            Debug.Log(room.hasNeighbours[0]);
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
    
    void FindNeighbours(List<Vector2Int> list, Vector2Int curPos, Vector2Int prevPos, SO_Room room)
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
//            if (prevPos != directionsArr[i] && list.Contains(directionsArr[i]))
            if (list.Contains(directionsArr[i]))
            {
                room.hasNeighbours[i] = true;
            }
        }
    }
}
