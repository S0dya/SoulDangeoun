using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Image HPBar;
    [SerializeField] SO_Weapon weapon;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject deadBodyPrefab;

    [Header("Enemy")]
    [SerializeField] int behaviourType; //melee, middle, far
    [SerializeField] float HPMultiplier;
    [SerializeField] int distanceRandomTargetPick;
    [SerializeField] float minTimeRandomTargetPick;
    [SerializeField] float maxTimeRandomTargetPick;
    [SerializeField] float inputFromAttackMuiltiplier;
    [Header("Melee")]
    [SerializeField] float meleeForce;
    [SerializeField] CircleCollider2D meleeCollider;//ChL
    [SerializeField] float meleeReloadingTimeMin;
    [SerializeField] float meleeReloadingTimeMax;
    [SerializeField] float attackDistance;
    [SerializeField] float attackTime;

    [Header("Middle")]
    [SerializeField] int distanceRandomTargetPickOnSeesPlayer;

    Transform playerTransform;
    Vector2 target;

    Transform deadBodyParent;

    Coroutine pickRandomTargetPosCor;
    Coroutine followPlayerCor;
    Coroutine shootingCor;
    Coroutine meleeAtackingCor;

    //local
    bool isLookingOnRight;
    [HideInInspector] public bool seesPlayer;
    bool isReloading;
    bool isDying;

    float curHP = 1;
    float defaultSpeed;
    
    //onDeath
    Vector2 lastImpactPos;
    float lastImpactDistance;


    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        deadBodyParent = GameObject.FindGameObjectWithTag("DeadBodyParent").transform;
    }

    void Start()
    {
        defaultSpeed = agent.speed;
        target = transform.position;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        pickRandomTargetPosCor = StartCoroutine(PickRandomTargetPosCor());
    }

    void FixedUpdate()
    {
        agent.SetDestination(target);

        if (seesPlayer)
        {
            if ((isLookingOnRight && transform.position.x < playerTransform.position.x) || (!isLookingOnRight && transform.position.x > playerTransform.position.x))
            {
                ChangeLookingDirection();
            }
        }
        else if ((isLookingOnRight && agent.velocity.x > 0) || (!isLookingOnRight && agent.velocity.x < 0))
        {
            ChangeLookingDirection(); 
        }
    }

    
    //main
    public void StartFollowing()
    {
        seesPlayer = true;

        switch (behaviourType)
        {
            case 0:
                followPlayerCor = StartCoroutine(FollowPlayerCor());
                meleeAtackingCor = StartCoroutine(MeleeAtackingCor());
                break;
            case 1:
                shootingCor = StartCoroutine(ShootingCor());
                distanceRandomTargetPick = distanceRandomTargetPickOnSeesPlayer;
                break;
            case 2:
                shootingCor = StartCoroutine(ShootingCor());
                if (pickRandomTargetPosCor != null) StopCoroutine(pickRandomTargetPosCor);
                break;
            default:
                break;
        }
    }
    public void StopFollowing()
    {
        seesPlayer = false;

        switch (behaviourType)
        {
            case 0:
                if (meleeAtackingCor != null) StopCoroutine(meleeAtackingCor);
                if (followPlayerCor != null) StopCoroutine(followPlayerCor);
                break;
            case 1:
                if (shootingCor != null) StopCoroutine(shootingCor);
                distanceRandomTargetPickOnSeesPlayer = distanceRandomTargetPick;
                break;
            case 2:
                if (shootingCor != null) StopCoroutine(shootingCor);
                pickRandomTargetPosCor = StartCoroutine(PickRandomTargetPosCor());
                break;
            default:
                break;
        }
    }

    //cors
    IEnumerator PickRandomTargetPosCor()
    {
        while (true)
        {
            float randomX = transform.position.x;
            float randomY = transform.position.y;
            target = new Vector2(Random.Range(randomX - distanceRandomTargetPick, randomX + distanceRandomTargetPick), 
                Random.Range(randomY - distanceRandomTargetPick, randomY + distanceRandomTargetPick));
            yield return new WaitForSeconds(Random.Range(minTimeRandomTargetPick, maxTimeRandomTargetPick));
        }
    }
    IEnumerator FollowPlayerCor()
    {
        while (true)
        {
            target = playerTransform.position;

            yield return null;
        }
    }
    IEnumerator ShootingCor()
    {
        while (true)
        {
            if (!isReloading)
            {
                Vector2 shootingDirection = (playerTransform.position - transform.position).normalized;
                weapon.Shoot((Vector2)transform.position + shootingDirection, shootingDirection);

                StartCoroutine(ReloadingCor(weapon.reloadingTimeMin, weapon.reloadingTimeMax));
            }

            yield return null;
        }
    }
    IEnumerator MeleeAtackingCor()
    {
        while (true)
        {
            if (!isReloading && Vector2.Distance(transform.position, playerTransform.position) < attackDistance)
            {
                MeleeAttack();

                StartCoroutine(ReloadingCor(meleeReloadingTimeMin, meleeReloadingTimeMax));
            }

            yield return null;
        }
    }
    IEnumerator ReloadingCor(float min, float max)
    {
        isReloading = true;
        yield return new WaitForSeconds(Random.Range(min, max));
        isReloading = false;
    }

    //methods
    public void MeleeAttack()
    {
        Vector2 direction = playerTransform.position - transform.position;
        rb.AddForce(direction * meleeForce, ForceMode2D.Impulse);
        //playAnimation

        meleeCollider.enabled = true;
        Invoke("DisableMeleeCollider", attackTime);
    }
    void DisableMeleeCollider()//MOVETOANIM
    {
        meleeCollider.enabled = false;
    }

    public void DamageImpact(Vector2 pos, float distance)
    {
        lastImpactPos = pos;
        lastImpactDistance = distance;
        rb.AddForce(pos * distance, ForceMode2D.Impulse);
    }

    public void StopAgent(bool isStopped)
    {
        agent.speed = isStopped ? 0 : defaultSpeed;
    }

    void ChangeLookingDirection()
    {
        isLookingOnRight = !isLookingOnRight;

        transform.rotation = Quaternion.Euler(new Vector2(0, (transform.rotation.y == 0 ? 180 : 0)));
    }

    public void ChangeHP(float value)
    {
        float newHP = Mathf.Max(curHP - value / 10 * HPMultiplier, 0);
        curHP = newHP;
        HPBar.fillAmount = newHP;
        if (newHP == 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDying) return;
        isDying = true;
        StopAllCoroutines();

        if (PlayerEnemyTrigger.I.inEnemiesTransforms.Contains(this.transform))
        {
            PlayerEnemyTrigger.I.inEnemiesTransforms.Remove(this.transform);
            PlayerEnemyTrigger.I.CheckForNearestEnemy();
        }

        SpawnManager.I.currentEnemiesAmount--;
        if (SpawnManager.I.currentEnemiesAmount <= 0)
        {
            SpawnManager.I.SpawnMoreChecck();
        }

        GameObject deadBodyObj = Instantiate(deadBodyPrefab, transform.position, Quaternion.identity, deadBodyParent);
        Rigidbody2D deadBodyRb = deadBodyObj.GetComponent<Rigidbody2D>();
        deadBodyRb.AddForce(lastImpactPos * lastImpactDistance, ForceMode2D.Impulse);

        Destroy(gameObject);
    }

}
