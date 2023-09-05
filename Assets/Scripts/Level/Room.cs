using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int halfSize;
    public int index;

    //left right down top
    public bool[] hasNeighbours = new bool[4];
    [SerializeField] GameObject[] wallsObj;
    [SerializeField] Collider2D collider;

    bool cleared;

    public void DrawWalls(bool val)
    {
        for (int i = 0; i < 4; i++)
        {
            if (hasNeighbours[i])
            {
                wallsObj[i].SetActive(val);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //SetMap
            if (!cleared)
            {
                cleared = true;
                DrawWalls(true);

                float x = transform.position.x;
                float y = transform.position.y;
                float halfX = (float)halfSize.x;
                float halfY = (float)halfSize.y;

                SpawnManager.I.SpawnEnemies(x - halfX, x + halfX, y - halfY, y + halfY);

            }
        }
    }


}
