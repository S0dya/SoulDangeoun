using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCloseEnemyTriggerAvoid : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CloseDistanceEnemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.StopAgent(true);

        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CloseDistanceEnemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.StopAgent(false);
        }
    }
}
