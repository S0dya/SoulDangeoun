using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChest : MonoBehaviour
{
    public int type;//weapon, potion

    [SerializeField] SpriteRenderer sprite;

    [SerializeField] Sprite[] closedSprite;
    [SerializeField] Sprite[] openedSprite;

    [HideInInspector] public bool isOpened;

    void Start()
    {
        type = (Random.Range(0, 6) < 4 ? 0 : 1);
        sprite.sprite = closedSprite[type];
    }

    public void OpenChest()
    {
        isOpened = true;
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
        sprite.sprite = openedSprite[type];
        SpawnManager.I.SpawnPickable(transform.position, type, false);
    }
}
