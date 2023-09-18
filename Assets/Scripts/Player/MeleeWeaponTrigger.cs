using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponTrigger : MonoBehaviour
{
    [SerializeField] LayerMask enemyLayer;
    [HideInInspector] public float damage;
    [HideInInspector] public float meleeImpact;


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyLayer == (enemyLayer | (1 << collision.gameObject.layer)))
        {
            Debug.Log("dd");
            GameObject enemyObj = collision.gameObject;
            Enemy enemy = enemyObj.GetComponent<Enemy>();
            enemy.DamageImpact((Vector2)(enemyObj.transform.position - transform.position), meleeImpact);
            enemy.ChangeHP(damage);
        }

        if (collision.gameObject.CompareTag("EnemyBullet"))//add reflect logic
        {
            Destroy(collision.gameObject);

        }
    }
}
