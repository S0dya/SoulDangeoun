using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LeanTween;

public class FollowingObject : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] bool isGold;

    Transform playerTransform;
    [HideInInspector] public bool isFollowing;

    void Awake()
    {
        transform.localScale = Vector2.zero;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        LeanTween.scale(gameObject, new Vector2 (1, 1), 1).setEase(LeanTweenType.easeOutBack);
    }

    public void FollowPlayer()
    {
        isFollowing = true;
        StartCoroutine(FollowPlayerCor());
    }
    IEnumerator FollowPlayerCor()
    {
        while (true)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.velocity = direction * 14;

            if (Vector2.Distance(transform.position, playerTransform.position) < 0.5f) break;
            yield return null;
        }

        if (isGold)
            Player.I.ChangeGold(1);
        else
            Player.I.ChangeMana(-8);

        Destroy(gameObject);
    }
}
