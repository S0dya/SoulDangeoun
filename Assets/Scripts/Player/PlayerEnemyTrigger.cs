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
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyLayer == (enemyLayer | (1 << collision.gameObject.layer)))
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
        if (enemyLayer == (enemyLayer | (1 << collision.gameObject.layer)))
        {
            inEnemiesTransforms.Remove(collision.transform);
            if (inEnemiesTransforms.Count == 0 && peakNearestCor != null)
            {
                player.seesEnemy = false;
                if (peakNearestCor != null) StopCoroutine(peakNearestCor);
                if (lookAtNearestEnemyCor != null) StopCoroutine(lookAtNearestEnemyCor);

                CinemachineCamera.I.ChangeCameraFollow(playerTransform);
            }
        }
    }

    IEnumerator PeakNearestEnemyCor()
    {
        while (true)
        {
            CheckForNearestEnemy();

            yield return null;
        }
    }

    public void CheckForNearestEnemy()
    {
        float shortestDistance = Mathf.Infinity;

        foreach (Transform enemyTransform in inEnemiesTransforms)
        {
            Vector3 directionToEnemy = enemyTransform.position - playerTransform.position;

            if (!Physics.Raycast(playerTransform.position, directionToEnemy, directionToEnemy.magnitude, obstacleLayer))
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemyTransform.position);

                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearest = enemyTransform;
                }
                Debug.DrawLine(playerTransform.position, enemyTransform.position, Color.green); // For visualization purposes
            }
            else
            {
                Debug.DrawLine(playerTransform.position, enemyTransform.position, Color.red); // For visualization purposes
            }
        }
        
        CinemachineCamera.I.ChangeCameraFollow(nearest);
    }

    IEnumerator LookAtNearestEnemyCor()
    {
        while (true)
        {
            player.shootingDirection = (nearest.position - playerTransform.position).normalized;
            player.isEnemyOnTheRight = nearest.position.x < playerTransform.position.x;
            yield return null;
        }

    }
}
