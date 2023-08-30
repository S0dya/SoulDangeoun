using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] int distanceRandomTargetPick;
    [SerializeField] float minTimeRandomTargetPick;
    [SerializeField] float maxTimeRandomTargetPick;

    Transform playerTransform;

    Vector2 target;

    Coroutine followingCor;
    Coroutine pickRandomTargetPosCor;

    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

    }

    void Start()
    {
        target = transform.position;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        pickRandomTargetPosCor = StartCoroutine(PickRandomTargetPosCor());
    }

    void FixedUpdate()
    {
        agent.SetDestination(target);
    }

    public void StartFollowing()
    {
        if (followingCor != null)
        {
            StopCoroutine(pickRandomTargetPosCor);
        }
        followingCor = StartCoroutine(FollowingCor());
    }
    public void StopFollowing()
    {
        if (followingCor !=  null)
        {
            StopCoroutine(followingCor);
        }
        pickRandomTargetPosCor = StartCoroutine(PickRandomTargetPosCor());
    }

    IEnumerator PickRandomTargetPosCor()
    {
        while (true)
        {
            target = Random.insideUnitCircle * distanceRandomTargetPick;
            yield return new WaitForSeconds(Random.Range(minTimeRandomTargetPick, maxTimeRandomTargetPick));
        }
    }
    IEnumerator FollowingCor()
    {
        while (true)
        {
            target = playerTransform.position;
            yield return null;
        }
    }


}
