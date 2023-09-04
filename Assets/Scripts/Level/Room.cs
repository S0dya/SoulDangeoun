using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int halfSize;
    public int type;

    //left right down top
    public bool[] hasNeighbours = new bool[4];


}
