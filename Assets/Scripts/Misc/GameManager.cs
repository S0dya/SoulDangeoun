using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{

    protected override void Awake()
    {
        base.Awake();

    }

    void Start()
    {
        LoadData();
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

    //save/load
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveData();
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveData();
        }
    }

    void OnApplicationQuit()
    {
        SaveData();
    }

    public void SaveData()
    {
        //int/float
        PlayerPrefs.SetInt("crystalsAmount", Settings.crystalsAmount);
        for (int i = 0; i < Settings.healths.Length; i++)
        {
            PlayerPrefs.SetInt($"healths {i}", Settings.healths[i]);
            PlayerPrefs.SetInt($"shields {i}", Settings.shields[i]);
            PlayerPrefs.SetInt($"mana {i}", Settings.mana[i]);
            PlayerPrefs.SetInt($"currentUpgrade {i}", Settings.currentUpgrade[i]);
            PlayerPrefs.SetInt($"charactersPrice {i}", Settings.charactersPrice[i]);
            PlayerPrefs.SetInt($"upgradePrices {i}", Settings.upgradePrices[i]);
            PlayerPrefs.SetFloat($"reloadOfPowers {i}", Settings.reloadOfPowers[i]);
            PlayerPrefs.SetFloat($"durationOfPowers {i}", Settings.durationOfPowers[i]);
        }

        //bool
        PlayerPrefs.SetInt("isMusicOn", Settings.isMusicOn ? 0 : 1);
    }

    public void LoadData() 
    {
        //int/float
        Settings.crystalsAmount = PlayerPrefs.GetInt("crystalsAmount");

        for (int i = 0; i < Settings.healths.Length; i++)
        {
            Settings.healths[i] = PlayerPrefs.GetInt($"healths {i}");
            Settings.shields[i] = PlayerPrefs.GetInt($"shields {i}");
            Settings.mana[i] = PlayerPrefs.GetInt($"mana {i}");
            Settings.currentUpgrade[i] = PlayerPrefs.GetInt($"currentUpgrade {i}");
            Settings.charactersPrice[i] = PlayerPrefs.GetInt($"charactersPrice {i}");
            Settings.upgradePrices[i] = PlayerPrefs.GetInt($"upgradePrices {i}");
            Settings.reloadOfPowers[i] = PlayerPrefs.GetFloat($"reloadOfPowers {i}");
            Settings.durationOfPowers[i] = PlayerPrefs.GetFloat($"durationOfPowers {i}");
        }

        //bool
        Settings.isMusicOn = (PlayerPrefs.GetInt("isMusicOn") == 0);
    }

}
