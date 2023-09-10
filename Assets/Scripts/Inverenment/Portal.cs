using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public void UsePortal()
    {
        GameMenu.I.ToggleLoadingScreen(true);

        Settings.NextLevel();
        GameManager.I.NextLevel();

        Destroy(gameObject);
    }
}
