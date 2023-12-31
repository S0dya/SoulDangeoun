using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float damage;
    [SerializeField] float bulletImpact;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || obstacleLayer == (obstacleLayer | (1 << collision.gameObject.layer)))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Box"))
        {
            Box box = collision.gameObject.GetComponent<Box>();
            box.DestroyObjectWithMana();
            Destroy(gameObject);
        }
        else if (enemyLayer == (enemyLayer | (1 << collision.gameObject.layer)))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.DamageImpact((Vector2)(collision.transform.position - transform.position), bulletImpact);
            enemy.ChangeHP(damage);
            Destroy(gameObject);
        }
    }

}
