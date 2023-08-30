using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSmallEnemyTriggerAttract : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SmallEnemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.StartFollowing();

        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SmallEnemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.StopFollowing();
        }
    }
}
