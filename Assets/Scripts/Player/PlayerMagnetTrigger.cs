using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagnetTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FollowingObject"))
        {
            FollowingObject followingObject = collision.gameObject.GetComponent<FollowingObject>();
            if (!followingObject.isFollowing)
            {
                followingObject.FollowPlayer();
            }
        }
    }
}
