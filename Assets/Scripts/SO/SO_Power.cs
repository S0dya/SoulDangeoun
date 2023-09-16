using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SO_Power : SO_Item
{
    public bool canUsePower = true;
    public float durationOfPower;
    public float reloadOfPower;
    public string description;

    public float powerMult = 0.5f;

    public void ActivatePower()
    {
        GameManager.I.StartCoroutine(PowerDurationCor());
    }

    IEnumerator PowerDurationCor()
    {
        Settings.speedMultiplier += powerMult;
        yield return new WaitForSeconds(durationOfPower);
        Settings.speedMultiplier -= powerMult;
    }
    
}
