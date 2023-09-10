using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : SingletonMonobehaviour<GameMenu>
{
    [SerializeField] CanvasGroup gameMenuCanvasGroup;
    [SerializeField] CanvasGroup loadingScreenGameMenuCanvasGroup;

    protected override void Awake()
    {
        base.Awake();

        OnResumeButton();//ChL
        ToggleLoadingScreen(true);
    }

    //buttons
    public void OnPauseButton()
    {
        Time.timeScale = 0f;
        Open(gameMenuCanvasGroup, 0.2f);
    }
    public void OnResumeButton()
    {
        Close(gameMenuCanvasGroup, 0.2f);
        Time.timeScale = 1f;
    }

    public void OnHomeButton()
    {

    }

    public void OnSettingsButton()
    {

    }

    //methods
    public void ToggleLoadingScreen(bool val)
    {
        if (val)
        {
            Open(loadingScreenGameMenuCanvasGroup, 0.1f);
        }
        else
        {
            Close(loadingScreenGameMenuCanvasGroup, 0.3f);
        }
    }

    //anim, opening
    void Open(CanvasGroup CG, float duration)
    {
        CG.blocksRaycasts = true;
        LTDescr tween = LeanTween.alphaCanvas(CG, 1, duration).setEase(LeanTweenType.easeInOutQuad);
        tween.setUseEstimatedTime(true);
    }
    void Close(CanvasGroup CG, float duration)
    {
        LTDescr tween = LeanTween.alphaCanvas(CG, 0, duration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => CloseComletely(CG));
        tween.setUseEstimatedTime(true);
    }
    void CloseComletely(CanvasGroup CG)
    {
        CG.blocksRaycasts = false;
    }
}
