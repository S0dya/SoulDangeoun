using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SO_Power : ScriptableObject
{
    public bool canUsePower = true;
    public float durationOfPower;
    public float reloadOfPower;

    public void ActivatePower()
    {
        GameManager.I.StartCoroutine(PowerDurationCor());
    }

    IEnumerator PowerDurationCor()
    {
        Settings.speedMultiplier += 0.5f;
        yield return new WaitForSeconds(durationOfPower);
        Settings.speedMultiplier -= 0.5f;
    }
    
}
