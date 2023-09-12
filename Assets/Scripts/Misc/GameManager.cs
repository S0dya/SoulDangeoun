using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{

    protected override void Awake()
    {
        base.Awake();

    }

    

    //UI
    public void Open(CanvasGroup CG, float duration)
    {
        CG.blocksRaycasts = true;
        LTDescr tween = LeanTween.alphaCanvas(CG, 1, duration).setEase(LeanTweenType.easeInOutQuad);
        tween.setUseEstimatedTime(true);
    }
    public void Close(CanvasGroup CG, float duration)
    {
        LTDescr tween = LeanTween.alphaCanvas(CG, 0, duration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => CloseComletely(CG));
        tween.setUseEstimatedTime(true);
    }
    void CloseComletely(CanvasGroup CG)
    {
        CG.blocksRaycasts = false;
    }

    public void Fade(GameObject obj, CanvasGroup CG, float durationStart, float durationEnd)
    {
        LeanTween.alphaCanvas(CG, 1f, durationStart).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => FadeOut(obj, CG, durationEnd));
    }
    void FadeOut(GameObject obj, CanvasGroup CG, float durationEnd)
    {
        LeanTween.alphaCanvas(CG, 0f, durationEnd).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => DestroyObject(obj));
    }
    void DestroyObject(GameObject obj)
    {
        Destroy(obj);
    }

}
