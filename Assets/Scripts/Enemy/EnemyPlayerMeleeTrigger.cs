using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerMeleeTrigger : MonoBehaviour
{
    [SerializeField] float damage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.TakeDamage(damage);
        }
    }
}
