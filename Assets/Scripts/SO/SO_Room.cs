using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_Room : ScriptableObject
{
    public Vector2 size;
    public int type;

    //left right down top
    public bool[] hasNeighbours = new bool[4];
    public bool[] hasCoridors = new bool[4];

}
