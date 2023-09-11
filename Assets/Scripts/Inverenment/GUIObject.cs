using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIObject : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    [SerializeField] Portal portal;
    [SerializeField] PickableObject pickableObject;
    public ItemChest itemChest;

    public void UseGUIObject()
    {
        if (portal != null) portal.UsePortal();
        else if (pickableObject != null) pickableObject.PickObject();
        if (itemChest != null) itemChest.OpenChest();
    }

    public void ToggleCanvas(bool val)
    {
        canvas.SetActive(val);
    }

    //ToDo-leanTweenEffect
}
