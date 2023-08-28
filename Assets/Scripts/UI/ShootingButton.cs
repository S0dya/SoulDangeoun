using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Player player;

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        player.StartShooting();
    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        player.StopShooting();
    }
}
