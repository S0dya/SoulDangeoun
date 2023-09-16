using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardPowerTrigger : MonoBehaviour
{
    [SerializeField] LayerMask enemyLayer;

    float splashImpact = 4;
    float damage = 3;


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyLayer == (enemyLayer | (1 << collision.gameObject.layer)))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.DamageImpact((Vector2)(collision.transform.position - transform.position), splashImpact);
            enemy.ChangeHP(damage);
        }
    }
}
