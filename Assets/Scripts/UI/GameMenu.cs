using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : SingletonMonobehaviour<GameMenu>
{
    [SerializeField] CanvasGroup gameMenuCanvasGroup;

    protected override void Awake()
    {
        base.Awake();

        ToggleGameMenu(false);//ChL
    }

    //buttons
    public void OnPauseButton()
    {
        Time.timeScale = 0f;
        ToggleGameMenu(true);
    }
    public void OnResumeButton()
    {
        Time.timeScale = 1f;
        ToggleGameMenu(false);
    }

    public void OnHomeButton()
    {

    }

    public void OnSettingsButton()
    {

    }

    //methods
    public void ToggleGameMenu(bool val)
    {
        LTDescr tween = LeanTween.alphaCanvas(gameMenuCanvasGroup, (val ? 1 : 0), 0.2f).setEase(LeanTweenType.easeInOutQuad);
        tween.setUseEstimatedTime(true);
        gameMenuCanvasGroup.blocksRaycasts = val;
    }
}
