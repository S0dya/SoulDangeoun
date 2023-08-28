using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonMonobehaviour<Player>
{
    [SerializeField] FloatingJoystick fJoystick;
    [SerializeField] Rigidbody2D rb;

    [SerializeField] GameObject bulletPrefab;

    Coroutine movementCor;
    Coroutine shootingCor;

    Vector2 pointingDirection = new Vector2(0, 1);

    bool canShoot = true;

    protected override void Awake()
    {
        base.Awake();

    }

    void Start()
    {
        
    }

    //Input
    public void StartMoving()
    {
        movementCor = StartCoroutine(MovementCor());
    }
    public void StopMoving()
    {
        if (movementCor != null)
        {
            StopCoroutine(movementCor);
        }
    }
    IEnumerator MovementCor()
    {
        while (true)
        {
            pointingDirection = fJoystick.Direction.normalized;
            rb.MovePosition(rb.position + fJoystick.Direction * 5 * Time.fixedDeltaTime);
            yield return null;
        }
    }

    public void StartShooting()
    {
        shootingCor = StartCoroutine(ShootingCor());
    }
    public void StopShooting()
    {
        if (shootingCor != null)
        {
            StopCoroutine(shootingCor);
        }
    }
    IEnumerator ShootingCor()
    {
        while (true)
        {
            if (canShoot)
            {
                Shoot();
                Debug.Log("Shoot");
                StartCoroutine(Reloading());
            }
            yield return null;
        }
    }
    IEnumerator Reloading()
    {
        canShoot = false;
        yield return new WaitForSeconds(1f);
        canShoot = true;
    }
    void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, (Vector2)transform.position + pointingDirection, Quaternion.identity);
        Rigidbody2D bulletRb = bulletObj.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(pointingDirection * 10f, ForceMode2D.Impulse);
    }

}
