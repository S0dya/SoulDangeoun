using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickableObject : MonoBehaviour
{
    public int type;//weapon, 

    public SO_Weapon weapon;

    [SerializeField] SpriteRenderer sprite;
    [SerializeField] TextMeshProUGUI text;

    void Start()
    {
        if (weapon != null)
        {
            sprite.sprite = weapon.ItemImage;
            text.text = weapon.Name;
        }
    }

    public void PickObject()
    {
        switch (type)
        {
            case 0:
                Player.I.PickWeapon(weapon);
                break;
            default: break;
        }

        Destroy(gameObject);
    }
}
