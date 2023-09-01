using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] float damage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Box"))
        {
            Box box = collision.gameObject.GetComponent<Box>();
            box.DestroyObject();
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

}
