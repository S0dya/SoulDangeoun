using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Player player;

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (player.onGUI)
        {
            PlayerGUITrigger.I.UseGUIObject();
        }
        else
        {
            player.StartShooting();
        }
    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        player.StopShooting();
    }
}
