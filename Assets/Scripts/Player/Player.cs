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
    [SerializeField] Transform weaponShootingTransform; 
    [SerializeField] SO_Weapon[] weapons = new SO_Weapon[2];
    [SerializeField] SpriteRenderer weaponSprite;


    [SerializeField] SO_Power power;
    [SerializeField] Image activityOfPowerButton;

    //UI
    [SerializeField] Image[] bars;
    [SerializeField] TextMeshProUGUI[] barsText;
    [SerializeField] TextMeshProUGUI goldText;

    Coroutine movementCor;
    Coroutine shootingCor;
    Coroutine armorRegenerationCor;

    [HideInInspector] public Vector2 pointingDirection = new Vector2(0, 1);
    [HideInInspector] public Vector2 shootingDirection = new Vector2(0, 1);

    bool canShoot = true;
    bool hasManaToShoot = true;
    [HideInInspector] public bool seesEnemy;
    bool isLookingOnRight;
    [HideInInspector] public bool isEnemyOnTheRight;
    bool canUsePower = true;

    //local
    int[] maxStats = new int[3] { 5, 3, 200 };
    float[] curStats = new float[3] { 1, 1, 1};
    float curMana = 1;
    int curGold;
    int curWeaponIndex;

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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnUsePowerButton();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            LevelGenerationManager.I.GenerateLevel();
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
    
    public void StartShooting()
    {
        shootingCor = StartCoroutine(ShootingCor());
    }
    public void StopShooting()
    {
        if (shootingCor != null) StopCoroutine(shootingCor);
    }
    
    //Buttons
    public void OnUsePowerButton()
    {
        if (canUsePower)
        {
            StartCoroutine(PowerActivityCor());
            power.ActivatePower();
        }
    }

    public void OnChangeWeaponButton()
    {
        int i = curWeaponIndex + 1;
        int newI = (i == weapons.Length ? 0 : i);
        if (weapons[newI] != null)
        {
            weapons[curWeaponIndex].UnSet();
            curWeaponIndex = newI;
            weapons[curWeaponIndex].Set();

            weaponSprite.sprite = weapons[newI].ItemImage;
            weapon = weapons[newI];
        }
    }
    

    //inputCor
    IEnumerator MovementCor()
    {
        while (true)
        {
            if (fJoystick.Direction.normalized != Vector2.zero)
            {
                pointingDirection = fJoystick.Direction.normalized;
            }
            rb.MovePosition(rb.position + fJoystick.Direction * 5 * Settings.speedMultiplier * Time.fixedDeltaTime);
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

    IEnumerator ShootingCor()
    {
        while (true)
        {
            if (canShoot && hasManaToShoot)
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

    

    //UICor
    IEnumerator PowerActivityCor()
    {
        canUsePower = false;
        float time = 0;
        float reload = power.reloadOfPower;
        while (time < reload)
        {
            activityOfPowerButton.fillAmount = time / reload;
            time += Time.deltaTime;

            yield return null;
        }
        activityOfPowerButton.fillAmount = 1;
        canUsePower = true;
    }

    IEnumerator ArmorRegenerationCor()
    {
        yield return new WaitForSeconds(5f);

        while (curStats[1] < 1)
        {
            ChangeValueForBar(1, -1);
            yield return new WaitForSeconds(2f);
        }
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
        
        weapon.Shoot((Vector2)weaponShootingTransform.position, shootingDirection);
        ChangeMana(weapon.manaOnShoot);
    }



    //UI
    public void TakeDamage(float value)
    {
        if (armorRegenerationCor != null) StopCoroutine(armorRegenerationCor);
        if (curStats[1] != 0)
        {
            int actualVal = ChangeValueForBar(1, value);
        }
        else
        {
            int actualVal = ChangeValueForBar(0, value);
            if (actualVal == 0)
            {
                Die();
            }
        }
        armorRegenerationCor = StartCoroutine(ArmorRegenerationCor());
    }
    public void ChangeMana(float value)
    {
        int actualVal = ChangeValueForBar(2, value);
        hasManaToShoot = actualVal != 0;
    }
    public int ChangeValueForBar(int i, float value)
    {
        float calc = curStats[i] - (value * (1.0f / maxStats[i]));
        float newVal = (calc > 0.01f ? calc : 0);
        newVal = Mathf.Min(newVal, 1);

        curStats[i] = newVal;
        bars[i].fillAmount = newVal;

        int actualVal = (int)(newVal * maxStats[i]);
        ChangeText(i, actualVal);

        return actualVal;
    }
    void ChangeText(int i, int amount)
    {
        barsText[i].text = $"{amount} / {maxStats[i]}";
    }
    
    public void ChangeGold(int value)
    {
        curGold += value;
        goldText.text = curGold.ToString();
    }





    void Die()
    {
        Debug.Log("GameOver");
    }
}
