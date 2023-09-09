using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMiddleEnemyTrigger : MonoBehaviour
{
    List<Enemy> enemies = new List<Enemy>();
    [SerializeField] Transform playerTransform;
    [SerializeField] LayerMask obstacleLayer;

    Coroutine checkIfEnemyCanSeePlayerCor;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MiddleDistanceEnemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemies.Add(enemy);
            if (enemies.Count == 1)
            {
                checkIfEnemyCanSeePlayerCor = StartCoroutine(CheckIfEnemyCanSeePlayerCor());
            }
        }
    }

    IEnumerator CheckIfEnemyCanSeePlayerCor()
    {
        while (true)
        {
            CheckEachEnemy();

            yield return new WaitForSeconds(0.2f);
        }
    }

    public void CheckEachEnemy()
    {
        foreach (Enemy enemy in enemies)
        {
            Vector2 directionToEnemy = enemy.gameObject.transform.position - playerTransform.position;
            float distanceToEnemy = directionToEnemy.magnitude;
            Vector2 raycastStart = (Vector2)playerTransform.position + directionToEnemy.normalized * 0.5f;

            RaycastHit2D hit = Physics2D.Raycast(raycastStart, directionToEnemy.normalized, distanceToEnemy, obstacleLayer);

            if (hit.collider == null)
            {
                if (!enemy.seesPlayer)
                {
                    enemy.StartFollowing();
                }
            }
            else
            {
                if (enemy.seesPlayer)
                {
                    enemy.StopFollowing();
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("MiddleDistanceEnemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemies.Remove(enemy);
            if (enemies.Count == 0)
            {
                if (checkIfEnemyCanSeePlayerCor != null) StopCoroutine(checkIfEnemyCanSeePlayerCor);
            }
        }
    }
}
