using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : SingletonMonobehaviour<GameMenu>
{
    [SerializeField] CanvasGroup gameMenuCanvasGroup;
    [SerializeField] CanvasGroup settingsCanvasGroup;

    protected override void Awake()
    {
        base.Awake();

        OnResumeButton();//ChL
        OnCloseSettingsButton();
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

    }

}
