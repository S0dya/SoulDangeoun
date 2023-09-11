using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public void UsePortal()
    {
        Settings.NextLevel();
        LevelGenerationManager.I.GenerateLevel();

        Destroy(gameObject);
    }
}
