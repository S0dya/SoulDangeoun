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

    [SerializeField] float HPMultiplier;
    [SerializeField] int distanceRandomTargetPick;
    [SerializeField] float minTimeRandomTargetPick;
    [SerializeField] float maxTimeRandomTargetPick;

    float curHP = 1;


    Transform playerTransform;
    Vector2 target;
    Coroutine shootingCor;

    bool isLookingOnRight;
    bool seesPlayer;
    bool isReloading;

    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

    }

    void Start()
    {
        target = transform.position;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        StartCoroutine(PickRandomTargetPosCor());
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

    

    public void StartFollowing()
    {
        seesPlayer = true;

        shootingCor = StartCoroutine(ShootingCor());
    }
    public void StopFollowing()
    {
        seesPlayer = false;

        StopCoroutine(shootingCor);
    }

    IEnumerator PickRandomTargetPosCor()
    {
        while (true)
        {
            float randomX = transform.position.x;
            float randomY = transform.position.y;
            target = new Vector2(Random.Range(randomX + distanceRandomTargetPick, randomX - distanceRandomTargetPick), 
                Random.Range(randomY + distanceRandomTargetPick, randomY - distanceRandomTargetPick));
            yield return new WaitForSeconds(Random.Range(minTimeRandomTargetPick, maxTimeRandomTargetPick));
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

                StartCoroutine(ReloadingCor());
            }

            yield return null;
        }
    }
    IEnumerator ReloadingCor()
    {
        isReloading = true;
        yield return new WaitForSeconds(Random.Range(weapon.reloadingTimeMin, weapon.reloadingTimeMax));
        isReloading = false;
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


        Destroy(gameObject);
    }

}
