using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIObject : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    
    [SerializeField] Portal portal;

    public void UseGUIObject()
    {
        if (portal != null) portal.UsePortal();
    }

    public void ToggleCanvas(bool val)
    {
        canvas.SetActive(val);
    }

    //ToDo-leanTweenEffect
}
