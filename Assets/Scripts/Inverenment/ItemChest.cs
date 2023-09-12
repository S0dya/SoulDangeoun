using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChest : MonoBehaviour
{
    public int type;//weapon, potion

    Transform pickableParent;

    [SerializeField] SpriteRenderer sprite;

    [SerializeField] Sprite[] closedSprite;
    [SerializeField] Sprite[] openedSprite;

    [SerializeField] GameObject pickableObjectPrefab;
    [SerializeField] SO_Weapon[] weapons;
    [SerializeField] SO_Potion[] potions;
    //[SerializeField] SO_Potion[] potions;

    [HideInInspector] public bool isOpened;

    void Awake()
    {
        pickableParent = GameObject.FindGameObjectWithTag("PickableParent").GetComponent<Transform>();
    }

    void Start()
    {
        type = Random.Range(0, 2);
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

        GameObject pickableObj = Instantiate(pickableObjectPrefab, transform.position, Quaternion.identity, pickableParent);
        PickableObject pickable = pickableObj.GetComponent<PickableObject>();

        switch (type)
        {
            case 0:
                pickable.weapon = weapons[Random.Range(0, weapons.Length)];
                break;
            case 1:
                pickable.potion = potions[Random.Range(0, potions.Length)];
                break;
            //case 1:
            default: break;
        }
    }

}
