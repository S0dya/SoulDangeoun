using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] GameObject manaPrefab;


    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void DestroyObjectWithMana()
    {
        if (Random.Range(0, 5) == 1)
        {
            float x = transform.position.x;
            float y = transform.position.y;
            Vector2 randomPos = new Vector2(Random.Range(x - 2, x + 2), Random.Range(y - 2, y + 2));
            Instantiate(manaPrefab, randomPos, Quaternion.identity);
        }
        DestroyObject();
    }

}
