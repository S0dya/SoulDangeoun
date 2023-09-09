using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float damage;
    [SerializeField] float bulletImpact;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
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
            GameObject enemyObj = collision.gameObject;
            Enemy enemy = enemyObj.GetComponent<Enemy>();
            enemy.DamageImpact((Vector2)(enemyObj.transform.position - transform.position), bulletImpact);
            enemy.ChangeHP(damage);
            Destroy(gameObject);
        }
    }

}
