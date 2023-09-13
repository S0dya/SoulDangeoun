using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickableObject : MonoBehaviour
{
    public SO_Weapon weapon;
    public SO_Potion potion;

    [SerializeField] SpriteRenderer sprite;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI priceText;

    public int price = 0;

    void Start()
    {
        if (weapon != null)
        {
            sprite.sprite = weapon.ItemImage;
            text.text = weapon.Name;
        }
        else if (potion != null)
        {
            sprite.sprite = potion.ItemImage;
            text.text = potion.Name;
        }

        if (price > 0) priceText.text = price.ToString();
    }

    public void PickObject()
    {
        if (price > 0)
        {
            if (price <= Player.I.curGold)
            {
                Player.I.ChangeGold(-price);
            }
            else return;
        }

        if (weapon != null) Player.I.PickWeapon(weapon);
        else if (potion != null) Player.I.PickPotion(potion);

        
        Destroy(gameObject);
    }
}
