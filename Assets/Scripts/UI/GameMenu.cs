using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMenu : SingletonMonobehaviour<GameMenu>
{
    [SerializeField] CanvasGroup gameMenuCanvasGroup;
    [SerializeField] CanvasGroup settingsCanvasGroup;
    [SerializeField] CanvasGroup gameoverCanvasGroup;
    [SerializeField] CanvasGroup countCrystalsCanvasGroup;

    [SerializeField] TextMeshProUGUI crystalsAmountText;

    [SerializeField] TextMeshProUGUI killedCloseEnemiesText;
    [SerializeField] TextMeshProUGUI killedMiddleEnemiesText;
    [SerializeField] TextMeshProUGUI killedFarEnemiesText;

    [SerializeField] Image musicImage;

    protected override void Awake()
    {
        base.Awake();

    }

    void Start()
    {
        Time.timeScale = 1f;
        musicImage.color = new Color(255, 255, 255, (Settings.isMusicOn ? 1 : 0.5f));
    }

    //buttons
    //gameMenu
    public void OnPauseButton()
    {
        Time.timeScale = 0f;
        GameManager.I.Open(gameMenuCanvasGroup, 0.2f);
    }
    public void OnResumeButton()
    {
        GameManager.I.Close(gameMenuCanvasGroup, 0.2f);
        Time.timeScale = 1f;
    }

    public void OnHomeButton()
    {
        Settings.crystalsAmount += (int)Settings.curentEarntCrystals;
        LoadingSceneManager.I.LoadMenu(true);
    }

    public void OnSettingsButton()
    {
        GameManager.I.Open(settingsCanvasGroup, 0.2f);
    }
    //settings
    public void OnCloseSettingsButton()
    {
        GameManager.I.Close(settingsCanvasGroup, 0.2f);
    }

    public void OnToggleMusicButton()
    {
        ToggleMusic(!Settings.isMusicOn);
    }

    public void ToggleMusic(bool val)
    {
        Settings.isMusicOn = val;
        musicImage.color = new Color(255, 255, 255, (val ? 1 : 0.5f));
        AudioManager.I.ToggleSound(val);
    }

    //gameover
    public void OnReplayButton()
    {
        LoadingSceneManager.I.RestartGame();
    }

    public void OnShowAdButton()
    {
        if (Player.I.hasResurrected)
        {
            AdsManager.I.ShowRewardedAd();
        }
    }

    public void OnCountCrystalsButton()
    {
        int earntCrystals = (int)Settings.curentEarntCrystals;
        Settings.crystalsAmount += earntCrystals;
        crystalsAmountText.text = earntCrystals.ToString();
        GameManager.I.Open(countCrystalsCanvasGroup, 0.2f);
    }

    //methods
    public void Gameover()
    {
        Time.timeScale = 0f;
        CountKilledEnemies();

        GameManager.I.Open(gameoverCanvasGroup, 0.6f);
    }

    public void CountKilledEnemies()
    {
        killedCloseEnemiesText.text = $" close enemies killed {Settings.amountOfKilledCloseEnemies}";
        killedMiddleEnemiesText.text = $" middle enemies killed {Settings.amountOfKilledMiddleEnemies}";
        killedFarEnemiesText.text = $" far enemies killed {Settings.amountOfKilledFarEnemies}";
    }

    public void RewardPlayer()
    {
        for (int i = 0; i < 3; i++)
        {
            Player.I.ChangeValueForBar(i, -Player.I.maxStats[i]);
        }
        Player.I.hasResurrected = true;

        GameManager.I.Close(gameoverCanvasGroup, 0.1f);
        Time.timeScale = 1f;
    }
}
