using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerButton : SingletonMonobehaviour<PowerButton>
{
    [SerializeField] Player player;
    [SerializeField] Image activityOfPowerButton;

    System.Action buttonAction;

    //[Header("double weapon")][SerializeField] int type;

    [Header("Dash")]
    [SerializeField] float powerMult;

    [Header("Splash")]
    public float splashSpeed;
    [SerializeField] CircleCollider2D splashCollider;
    [SerializeField] LineRenderer splashLineRenderer;
    [SerializeField] int numPoints;

    bool canUsePower = true;
    float reloadOfPower;
    float durationOfPower;

    protected override void Awake()
    {
        base.Awake();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnPowerButton();
        }
    }

    //button
    public void OnPowerButton()
    {
        if (canUsePower)
        {
            StartCoroutine(PowerActivityCor());

            buttonAction.Invoke();
        }
    }

    //powers
    void DoubleWeaponPower()
    {
        StartCoroutine(DoubleWeaponDurationCor());
    }
    IEnumerator DoubleWeaponDurationCor()
    {
        if (player.amountOfWeapon != 2)
        {
            player.ShowSecondWeapon(true);
            player.amountOfWeapon = 2;
        
            yield return new WaitForSeconds(durationOfPower);
        
            player.ShowSecondWeapon(false);
            player.amountOfWeapon = 1;
        }
    }


    void DashPower()
    {
        StartCoroutine(DashPowerDurationCor());
    }
    IEnumerator DashPowerDurationCor()
    {
        Settings.speedMultiplier += powerMult;
        yield return new WaitForSeconds(durationOfPower);
        Settings.speedMultiplier -= powerMult;
    }


    void SplashPower()
    {
        StartCoroutine(SplashPowerDurationCor());
    }
    IEnumerator SplashPowerDurationCor()
    {
        splashCollider.enabled = true;
        splashLineRenderer.enabled = true;

        while (splashCollider.radius < 4.5f)
        {
            splashCollider.radius += splashSpeed * Time.deltaTime;
            DrawCircle();

            yield return null;
        }

        splashCollider.enabled = false;
        splashLineRenderer.enabled = false;

        splashCollider.radius = 0.1f;
    }
    private void DrawCircle()
    {
        float radius = splashCollider.radius;

        splashLineRenderer.positionCount = numPoints + 1;

        float deltaTheta = (2f * Mathf.PI) / numPoints;
        float theta = 0f;

        for (int i = 0; i <= numPoints; i++)
        {
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            Vector2 point = new Vector2(x, y);
            splashLineRenderer.SetPosition(i, point);
            theta += deltaTheta;
        }
    }

    //reload
    IEnumerator PowerActivityCor()
    {
        canUsePower = false;
        float reload = reloadOfPower;
        float time = reload;

        while (time > 0.01f)
        {
            activityOfPowerButton.fillAmount = time / reload;
            time -= Time.deltaTime;

            yield return null;
        }
        activityOfPowerButton.fillAmount = 0;
        canUsePower = true;
    }

    


    public void ChangePower(int i)
    {
        reloadOfPower = Settings.reloadOfPowers[i];
        durationOfPower = Settings.durationOfPowers[i];

        switch (i)
        {
            case 0:
                buttonAction = DoubleWeaponPower;
                break;
            case 1:
                buttonAction = DashPower;
                break;
            case 2:
                buttonAction = SplashPower;
                break;

            default: break;
        }

    }
}
