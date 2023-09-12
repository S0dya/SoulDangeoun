using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : SingletonMonobehaviour<Player>
{
    [SerializeField] FloatingJoystick fJoystick;
    [SerializeField] Rigidbody2D rb;

    [SerializeField] SO_Weapon[] weapons = new SO_Weapon[2];
    [SerializeField] Transform weaponTransform;
    [SerializeField] Transform weaponShootingTransform;
    [Header("Melee")]
    [SerializeField] Animator meleeWeaponAnimator;
    [SerializeField] BoxCollider2D meleeWeaponCollider;
    [SerializeField] MeleeWeaponTrigger meleeWeaponTrigger;

    [SerializeField] SpriteRenderer weaponSprite;
    [Header("Power")]
    [SerializeField] SO_Power power;
    [SerializeField] Image activityOfPowerButton;
    [Header("UI")]
    [SerializeField] Image[] bars;
    [SerializeField] TextMeshProUGUI[] barsText;
    [SerializeField] TextMeshProUGUI goldText;

    Coroutine movementCor;
    Coroutine shootingCor;
    Coroutine armorRegenerationCor;

    [HideInInspector] public Vector2 pointingDirection;
    [HideInInspector] public Vector2 shootingDirection;

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
    int weaponI = 0;

    //GUI
    [HideInInspector] public bool onGUI;

    protected override void Awake()
    {
        base.Awake();

    }

    void Start()
    {
        pointingDirection = new Vector2(1, 0);
        shootingDirection = new Vector2(1, 0);

        //tempLogic
        weaponSprite.sprite = weapons[0].ItemImage;

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
        
        Vector2 direction = (seesEnemy ? shootingDirection : pointingDirection);
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        if (isLookingOnRight) weaponTransform.localRotation = Quaternion.Euler(180, 180, -angle);
        else weaponTransform.localRotation = Quaternion.Euler(0, 0, angle);
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
        int i = weaponI + 1;
        int newI = (i == weapons.Length ? 0 : i);
        if (weapons[newI] != null)
        {
            weapons[weaponI].UnSet();
            weaponI = newI;
            weapons[weaponI].Set();

            weaponSprite.sprite = weapons[newI].ItemImage;
            weapons[weaponI] = weapons[newI];

            if (weapons[weaponI].type == 1)
            {
                meleeWeaponTrigger.damage = weapons[weaponI].damage;
                meleeWeaponTrigger.meleeImpact = weapons[weaponI].meleeImpact;
                meleeWeaponCollider.size = weapons[weaponI].colliderSize;
                meleeWeaponCollider.offset = weapons[weaponI].colliderOffset;
            }
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
                CheckIfPlayerLooksAtEnemy();
            }
            else if ((pointingDirection.x < 0 && !isLookingOnRight) || (pointingDirection.x > 0 && isLookingOnRight))
            {
                ChangeLookingDirection();
            }
            yield return null;
        }
    }

    public void CheckIfPlayerLooksAtEnemy()
    {
        if ((isEnemyOnTheRight && !isLookingOnRight) || (!isEnemyOnTheRight && isLookingOnRight))
        {
            ChangeLookingDirection();
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
        yield return new WaitForSeconds(Random.Range(weapons[weaponI].reloadingTimeMin, weapons[weaponI].reloadingTimeMax));
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
        transform.localScale = new Vector2(transform.localScale.x == 1 ? -1 : 1, 1);
    }

    void Shoot()
    {
        if (!seesEnemy)
        {
            shootingDirection = pointingDirection;
        }
        
        weapons[weaponI].Shoot((Vector2)weaponShootingTransform.position, shootingDirection);
        if (weapons[weaponI].type == 1)
        {
            meleeWeaponAnimator.Play("Swing");

        }
        ChangeMana(weapons[weaponI].manaOnShoot);
    }

    public void PickWeapon(SO_Weapon newWeapon)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null)
            {
                weapons[i] = newWeapon;
                return;
            }
        }

        SpawnManager.I.DropWeapon(weapons[weaponI]);
        weapons[weaponI] = newWeapon;
    }

    public void PickPotion(SO_Potion potion)
    {
        int health = potion.AmountOfHealth;
        int mana = potion.AmountOfMana;

        if (health > 0) 
        {
            ChangeValueForBar(0, -health);
            PlayerGUI.I.AddText($"+ {health}", Settings.colorHealth);
        }
        if (mana > 0)
        {
            ChangeValueForBar(1, -mana);
            PlayerGUI.I.AddText($"+ {mana}", Settings.colorMana);
        }
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



    public void Clear()
    {
        transform.position = Vector3.zero;
    }

    void Die()
    {
        Debug.Log("GameOver");
    }
}
