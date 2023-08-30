using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : SingletonMonobehaviour<Player>
{
    [SerializeField] FloatingJoystick fJoystick;
    [SerializeField] Rigidbody2D rb;

    [SerializeField] SO_Weapon weapon;
    [SerializeField] Transform weaponTransform;

    [SerializeField] Image HPBar;
    [SerializeField] TextMeshProUGUI HPText;

    Coroutine movementCor;
    Coroutine shootingCor;

    [HideInInspector] public Vector2 pointingDirection = new Vector2(0, 1);
    [HideInInspector] public Vector2 shootingDirection = new Vector2(0, 1);

    bool canShoot = true;
    [HideInInspector] public bool seesEnemy;
    bool isLookingOnRight;
    [HideInInspector] public bool isEnemyOnTheRight;


    //local
    float curHP = 1;
    float healthMultiplier = 1.0f / 5;

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
    
    void FixedUpdate()
    {
        Vector2 direction = (seesEnemy ? shootingDirection : pointingDirection);
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weaponTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    //Input
    public void StartMoving()
    {
        movementCor = StartCoroutine(MovementCor());
    }
    public void StopMoving()
    {
        if (movementCor != null) StopCoroutine(movementCor);
    }
    IEnumerator MovementCor()
    {
        while (true)
        {
            if (fJoystick.Direction.normalized != Vector2.zero)
            {
                pointingDirection = fJoystick.Direction.normalized;
            }
            rb.MovePosition(rb.position + fJoystick.Direction * 5 * Time.fixedDeltaTime);
            if (seesEnemy)
            {
                if ((isEnemyOnTheRight && !isLookingOnRight) || (!isEnemyOnTheRight && isLookingOnRight))
                {
                    ChangeLookingDirection();
                }
            }
            else if ((pointingDirection.x < 0 && !isLookingOnRight) || (pointingDirection.x > 0 && isLookingOnRight))
            {
                ChangeLookingDirection();
            }
            yield return null;
        }
    }

    public void StartShooting()
    {
        shootingCor = StartCoroutine(ShootingCor());
    }
    public void StopShooting()
    {
        if (shootingCor != null) StopCoroutine(shootingCor);
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
        yield return new WaitForSeconds(Random.Range(weapon.reloadingTimeMin, weapon.reloadingTimeMax));
        canShoot = true;
    }

    //other
    void ChangeLookingDirection()
    {
        isLookingOnRight = !isLookingOnRight;
        if (transform.rotation.y == 0)
        {
            transform.rotation = Quaternion.Euler(new Vector2(0, 180));
            weaponTransform.localScale = new Vector2(1, -1);
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector2(0, 0));
            weaponTransform.localScale = new Vector2(1, 1);
        }
    }

    void Shoot()
    {
        if (!seesEnemy)
        {
            shootingDirection = pointingDirection;
        }
        
        weapon.Shoot((Vector2)transform.position + shootingDirection, shootingDirection);
    }


    public void TakeDamage(float value)
    {
        float newHP = Mathf.Max(curHP - (value * healthMultiplier), 0);
        curHP = newHP;
        HPBar.fillAmount = newHP;

        int actualHP = (int)(newHP * 5);
        ChangeText(HPText, actualHP);
        if (actualHP == 0)
        {
            Die();
        }
    }
    void ChangeText(TextMeshProUGUI text, int amount)
    {
        text.text = $"{amount} / {5}";
    }

    void Die()
    {
        Debug.Log("GameOver");
    }
}
