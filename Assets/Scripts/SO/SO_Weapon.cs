using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SO_Weapon : SO_Item
{
    public GameObject bulletPrefab;
    public int speed;
    public float damage;
    public float reloadingSpeed;



    public void Shoot(Vector2 initialPos, Vector2 shootingDirection)
    {
        GameObject bulletObj = Instantiate(bulletPrefab, initialPos + shootingDirection, Quaternion.identity);

        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.damage = damage;

        Rigidbody2D bulletRb = bulletObj.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(shootingDirection * speed, ForceMode2D.Impulse);
    }


}
