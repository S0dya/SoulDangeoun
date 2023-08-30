using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonMonobehaviour<Player>
{
    [SerializeField] FloatingJoystick fJoystick;
    [SerializeField] Rigidbody2D rb;

    [SerializeField] SO_Weapon weapon;

    Coroutine movementCor;
    Coroutine shootingCor;

    [HideInInspector] public Vector2 pointingDirection = new Vector2(0, 1);
    [HideInInspector] public Vector2 shootingDirection = new Vector2(0, 1);

    bool canShoot = true;
    [HideInInspector] public bool seesEnemy;

    protected override void Awake()
    {
        base.Awake();

    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartShooting();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StopShooting();
        }
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
                StartCoroutine(Reloading());
            }
            yield return null;
        }
    }
    IEnumerator Reloading()
    {
        canShoot = false;
        yield return new WaitForSeconds(weapon.reloadingSpeed);
        canShoot = true;
    }
    void Shoot()
    {
        if (!seesEnemy)
        {
            shootingDirection = pointingDirection;
        }
        weapon.Shoot((Vector2)transform.position, shootingDirection);
    }

}
