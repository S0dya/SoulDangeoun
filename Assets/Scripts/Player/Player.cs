using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : SingletonMonobehaviour<Player>
{
    [SerializeField] FloatingJoystick fJoystick;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    
    public SO_Weapon[] weapons;
    [SerializeField] Transform[] weaponsTransform;
    [SerializeField] Transform[] weaponsShootingTransform;
    [SerializeField] Animator[] weaponsAnimator;
    [Header("Melee")]
    [SerializeField] BoxCollider2D[] meleeWeaponCollider;
    [SerializeField] MeleeWeaponTrigger[] meleeWeaponTrigger;

    [SerializeField] SpriteRenderer[] weaponsSprite;
    [Header("UI")]
    [SerializeField] GameObject inputUI;
    [SerializeField] GameObject playerInterface;
    [SerializeField] GameObject playerTextCanvas;
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
    [HideInInspector] public bool hasResurrected;

    //local
    [HideInInspector] public int[] maxStats;
    float[] curStats = new float[3] { 1, 1, 1};
    float curMana = 1;
    [HideInInspector] public int curGold;
    
    int weaponI = 0;
    [HideInInspector] public int amountOfWeapon = 1;


    //GUI
    [HideInInspector] public bool onGUI;

    protected override void Awake()
    {
        base.Awake();

    }

    void Start()
    {
        inputUI.SetActive(false);//delter
        playerInterface.SetActive(false);
        Clear();

        //tempLogic

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
        if (Input.GetKeyUp(KeyCode.I))
        {
            Settings.crystalsAmount += 1551;
        }

        Vector2 direction = (seesEnemy ? shootingDirection : pointingDirection);
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        for (int i = 0; i < 2; i++)
        {
            if (isLookingOnRight) weaponsTransform[i].localRotation = Quaternion.Euler(180, 180, -angle);
            else weaponsTransform[i].localRotation = Quaternion.Euler(0, 0, angle);
        }
        
    }
    
    //Input
    public void StartMoving()
    {
        animator.Play("Walking");

        movementCor = StartCoroutine(MovementCor());
    }
    public void StopMoving()
    {
        animator.Play("Default");

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
    public void OnChangeWeaponButton()
    {
        int nextI = weaponI + 1;
        int newI = (nextI== weapons.Length ? 0 : nextI);
        if (weapons[newI] != null)
        {
            weapons[weaponI].UnSet();
            weaponI = newI;

            for (int i = 0; i < amountOfWeapon; i++) 
            {
                SetWeapon(i);
            }
        }
    }
    void SetWeapon(int i)
    {
        if (i == 0) weapons[weaponI].Set();
        weaponsSprite[i].sprite = weapons[weaponI].ItemImage;

        if (weapons[weaponI].type == 1)
        {
            meleeWeaponTrigger[i].damage = weapons[weaponI].damage;
            meleeWeaponTrigger[i].meleeImpact = weapons[weaponI].meleeImpact;
            meleeWeaponCollider[i].size = weapons[weaponI].colliderSize;
            meleeWeaponCollider[i].offset = weapons[weaponI].colliderOffset;
        }
    }
    public void ShowSecondWeapon(bool val)
    {
        weaponsSprite[1].sprite = val ? weapons[weaponI].ItemImage : null;
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

            animator.speed = fJoystick.Direction.magnitude;
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
                StartCoroutine(ShootingWithDelayCor());
                StartCoroutine(Reloading());
            }
            yield return null;
        }
    }
    IEnumerator ShootingWithDelayCor()
    {
        for (int i = 0; i < amountOfWeapon; i++)
        {
            Shoot(i);
            yield return new WaitForSeconds(weapons[weaponI].delayOfShootingWithDoubleWeapon);
        }
    }
    IEnumerator Reloading()
    {
        canShoot = false;
        yield return new WaitForSeconds(Random.Range(weapons[weaponI].reloadingTimeMin, weapons[weaponI].reloadingTimeMax));
        canShoot = true;
    }


    

    //UICor
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
        playerTextCanvas.transform.localScale = new Vector2(playerTextCanvas.transform.localScale.x == 1 ? -1 : 1, 1);
    }

    void Shoot(int i)
    {
        if (!seesEnemy)
        {
            shootingDirection = pointingDirection;
        }

        weapons[weaponI].Shoot((Vector2)weaponsShootingTransform[i].position, shootingDirection);
        if (weapons[weaponI].type == 1)
        {
            weaponsAnimator[i].Play("Swing");
        }
        else
        {
            AudioManager.I.PlayOneShot("ShootSound");
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

        weapons[weaponI].UnSet();
        SpawnManager.I.DropWeapon(weapons[weaponI]);
        weapons[weaponI] = newWeapon;
        for (int i = 0; i < amountOfWeapon; i++)
        {
            SetWeapon(i);
        }
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
            ChangeValueForBar(2, -mana);
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


    public void PlayStepSound()
    {
        AudioManager.I.PlayOneShot("PlayerStepSound");
    }

    public void EnablePlayer()
    {
        for (int i = 0; i < amountOfWeapon; i++)
        {
            weaponsSprite[i].sprite = weapons[0].ItemImage;
            SetWeapon(i);
        }

        for (int i = 0; i < 3; i++) ChangeValueForBar(i, 0);

        inputUI.SetActive(true);
        playerInterface.SetActive(true);
    }

    public void DesablePlayer()
    {
        inputUI.SetActive(false);
        playerInterface.SetActive(false);
    }

    public void Clear()
    {
        transform.position = Vector3.zero; 
        pointingDirection = new Vector2(1, 0);
        shootingDirection = new Vector2(1, 0);
    }

    void Die()
    {
        DesablePlayer();
        GameMenu.I.Gameover();
    }
}
