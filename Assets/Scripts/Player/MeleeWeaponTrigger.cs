using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponTrigger : MonoBehaviour
{
    [SerializeField] LayerMask enemyLayer;
    [HideInInspector] public float damage;


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyLayer == (enemyLayer | (1 << collision.gameObject.layer)))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.ChangeHP(damage);
        }
    }
}
