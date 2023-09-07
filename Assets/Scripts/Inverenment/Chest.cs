using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Sprite openedSprite;

    [SerializeField] GameObject manaPrefab;
    [SerializeField] GameObject goldPrefab;

    bool isOpened;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isOpened)
            {
                isOpened = true;
                PlayOpenAnimation();
            }
        }
    }

    void PlayOpenAnimation()
    {
        Vector2 originalScale = transform.localScale;

        LeanTween.scale(gameObject, originalScale * 1.05f, 0.1f)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
                LeanTween.delayedCall(gameObject, 0.1f, () =>
                {
                    LeanTween.scale(gameObject, originalScale, 0.1f)
                        .setEase(LeanTweenType.easeInQuad)
                        .setOnComplete(() =>
                        {
                            Open();
                        });
                });
            });
    }

    void Open()
    {
        sr.sprite = openedSprite;

        InstantiateGift(manaPrefab, Random.Range(3, 6));
        InstantiateGift(goldPrefab, Random.Range(2, 7));
    }

    void InstantiateGift(GameObject prefab, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            float x = transform.position.x;
            float y = transform.position.y;
            Vector2 randomPos = new Vector2(Random.Range(x - 2, x + 2), Random.Range(y - 2, y + 2));
            Instantiate(prefab, randomPos, Quaternion.identity);
        }
    }
}
