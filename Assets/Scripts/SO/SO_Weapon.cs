using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SO_Weapon : SO_Item
{
    public GameObject bulletPrefab;
    public int speed;
    public int type;//gun, sword
    //gun
    public float reloadingTimeMin;
    public float reloadingTimeMax;

    //melee
    public float damage;
    public Vector2 colliderOffset;
    public Vector2 colliderSize;
    public float timeColliderEnabled;


    public float manaOnShoot;

    public float speedOfMovementMultiplier;

    
    public void Shoot(Vector2 initialPos, Vector2 shootingDirection)
    {
        switch (type)
        {
            case 0:
                GameObject bulletObj = Instantiate(bulletPrefab, initialPos, Quaternion.identity);

                float angle = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg - 90;
                bulletObj.transform.rotation = Quaternion.Euler(0, 0, angle);

                Rigidbody2D bulletRb = bulletObj.GetComponent<Rigidbody2D>();
                bulletRb.AddForce(shootingDirection * speed, ForceMode2D.Impulse);
                break;
            default:
                break;
        }
        
    }

    public void Set()
    {
        Settings.speedMultiplier -= speedOfMovementMultiplier;
    }

    public void UnSet()
    {
        Settings.speedMultiplier += speedOfMovementMultiplier;
    }


}
