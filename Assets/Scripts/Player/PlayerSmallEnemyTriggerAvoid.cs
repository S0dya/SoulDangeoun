using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerSmallEnemyTriggerAvoid : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SmallEnemy"))
        {
            NavMeshAgent agent = collision.gameObject.GetComponent<NavMeshAgent>();
            agent.speed = 0;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SmallEnemy"))
        {
            NavMeshAgent agent = collision.gameObject.GetComponent<NavMeshAgent>();
            agent.speed = 3.5f;
        }
    }
}
