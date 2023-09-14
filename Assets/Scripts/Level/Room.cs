using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    public Vector2Int halfSize;
    public int index;

    //left right down top
    public bool[] hasNeighbours = new bool[4];
    [SerializeField] GameObject[] wallsObj;
    [SerializeField] Collider2D collider;

    public bool cleared;

    [SerializeField] Tilemap wallTilemap;

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
            LevelGenerationManager.I.createdLevelsUIList[index].HighlightLevel(true);

            if (!cleared)
            {
                cleared = true;
                DrawWalls(true);

                int x = (int)transform.position.x;
                int y = (int)transform.position.y;

                SpawnManager.I.SetPos(wallTilemap, this, x - halfSize.x + 1, x + halfSize.x - 1, 
                    y - halfSize.y + 1, y + halfSize.y - 1);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LevelGenerationManager.I.createdLevelsUIList[index].HighlightLevel(false);
        }
    }

    //public void DestroyObject() => Destroy(gameObject);
}
