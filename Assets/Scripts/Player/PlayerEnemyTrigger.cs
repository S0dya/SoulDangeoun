using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyTrigger : SingletonMonobehaviour<PlayerEnemyTrigger>
{
    [SerializeField] Player player;
    Transform playerTransform;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] LayerMask obstacleLayer;
    
    public List<Transform> inEnemiesTransforms = new List<Transform>();
    public Transform nearest;

    Coroutine peakNearestCor;
    Coroutine lookAtNearestEnemyCor;

    protected override void Awake()
    {
        base.Awake();

        playerTransform = player.transform;
        nearest = playerTransform;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyLayer == (enemyLayer | (1 << collision.gameObject.layer)) && !inEnemiesTransforms.Contains(collision.transform))
        {
            inEnemiesTransforms.Add(collision.transform);
            if (inEnemiesTransforms.Count == 1)
            {
                peakNearestCor = StartCoroutine(PeakNearestEnemyCor());
                lookAtNearestEnemyCor = StartCoroutine(LookAtNearestEnemyCor());
                player.seesEnemy = true;
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (enemyLayer == (enemyLayer | (1 << collision.gameObject.layer)) && inEnemiesTransforms.Contains(collision.transform))
        {
            inEnemiesTransforms.Remove(collision.transform);
            if (inEnemiesTransforms.Count == 0 && peakNearestCor != null)
            {
                player.seesEnemy = false;
                if (peakNearestCor != null) StopCoroutine(peakNearestCor);
                if (lookAtNearestEnemyCor != null) StopCoroutine(lookAtNearestEnemyCor);

                player.pointingDirection = player.shootingDirection;
                CinemachineCamera.I.ChangeCameraFollow(playerTransform);
            }
        }
    }

    IEnumerator PeakNearestEnemyCor()
    {
        while (true)
        {
            CheckForNearestEnemy();

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void CheckForNearestEnemy()
    {
        float shortestDistance = Mathf.Infinity;
        int seenEnemies = 0;

        foreach (Transform enemyTransform in inEnemiesTransforms)
        {
            if (enemyTransform == null) continue;
            Vector2 directionToEnemy = enemyTransform.position - playerTransform.position;
            float distanceToEnemy = directionToEnemy.magnitude;
            Vector2 raycastStart = (Vector2)playerTransform.position + directionToEnemy.normalized * 0.5f;

            RaycastHit2D hit = Physics2D.Raycast(raycastStart, directionToEnemy.normalized, distanceToEnemy, obstacleLayer);

            if (hit.collider == null)
            {
                seenEnemies++;
                
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearest = enemyTransform;
                    player.CheckIfPlayerLooksAtEnemy();
                }
            }
        }

        if (seenEnemies == 0)
        {
            player.seesEnemy = false;
            CinemachineCamera.I.ChangeCameraFollow(playerTransform);
        }
        else
        {
            player.seesEnemy = true;
            CinemachineCamera.I.ChangeCameraFollow(nearest);
        }
    }

    IEnumerator LookAtNearestEnemyCor()
    {
        while (true)
        {
            if (nearest != null)
            {
                player.shootingDirection = (nearest.position - playerTransform.position).normalized;
                player.isEnemyOnTheRight = nearest.position.x < playerTransform.position.x;
            }
            
            yield return null;
        }

    }
}
