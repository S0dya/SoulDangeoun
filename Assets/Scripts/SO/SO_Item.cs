using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SO_Item : ScriptableObject
{
    [field: SerializeField] public int itemCode { get; set; }
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public Sprite ItemImage { get; set; }
    [field: SerializeField] public int Price { get; set; }
}
