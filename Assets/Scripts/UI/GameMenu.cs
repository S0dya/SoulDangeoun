using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : SingletonMonobehaviour<GameMenu>
{
    [SerializeField] CanvasGroup gameMenuCanvasGroup;

    protected override void Awake()
    {
        base.Awake();

        OnResumeButton();//ChL
    }

    //buttons
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

    }

    public void OnSettingsButton()
    {

    }


}
