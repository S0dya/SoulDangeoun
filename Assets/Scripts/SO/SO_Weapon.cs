using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SO_Weapon : SO_Item
{
    public int type;//gun, sword
    public float manaOnShoot;
    public float speedOfMovementMultiplier;

    [Header("Gun")]
    public GameObject bulletPrefab;
    public int speed;
    public int amountToShoot;
    public float shootingSpread;
    public int recoil;
    public float delayOfShooting;
    public float delayOfShootingWithDoubleWeapon;
    public float reloadingTimeMin;
    public float reloadingTimeMax;
    public float destroyingDelay;

    [Header("Melee")]
    public float damage;
    public float meleeImpact;
    public Vector2 colliderOffset;
    public Vector2 colliderSize;
    public float timeColliderEnabled;

    
    public void Shoot(Vector2 initialPos, Vector2 shootingDirection)
    {
        switch (type)
        {
            case 0:
                GameManager.I.StartCoroutine(ShooBullets(initialPos, shootingDirection));
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


    IEnumerator ShooBullets(Vector2 initialPos, Vector2 shootingDirection)
    {
        int curAmountShot = 0;
        while (curAmountShot < amountToShoot)
        {
            GameObject bulletObj = Instantiate(bulletPrefab, initialPos, Quaternion.identity);
            float randomY = Random.Range(shootingDirection.y - shootingSpread, shootingDirection.y + shootingSpread);
            float randomX = Random.Range(shootingDirection.x - shootingSpread, shootingDirection.x + shootingSpread);

            float angle = Mathf.Atan2(randomY, randomX) * Mathf.Rad2Deg - 90;
            bulletObj.transform.rotation = Quaternion.Euler(0, 0, angle);

            Rigidbody2D bulletRb = bulletObj.GetComponent<Rigidbody2D>();
            bulletRb.AddForce(new Vector2(randomX, randomY) * speed, ForceMode2D.Impulse);

            GameManager.I.StartCoroutine(DestroyAfterDelay(bulletObj));

            curAmountShot++;
            yield return new WaitForSeconds(delayOfShooting);
        }
    }

    IEnumerator DestroyAfterDelay(GameObject obj)
    {
        yield return new WaitForSeconds(destroyingDelay);
        Destroy(obj);
    }
}
