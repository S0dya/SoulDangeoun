using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : SingletonMonobehaviour<Menu>
{
    [SerializeField] CanvasGroup menuCanvasGroup;
    [SerializeField] CanvasGroup pressAnyButtonCanvasGroup;
    [SerializeField] CanvasGroup settingsCanvasGroup;

    [SerializeField] Image musicImage;


    protected override void Awake()
    {
        base.Awake();

    }
    void Start()
    {
        musicImage.color = new Color(255, 255, 255, (Settings.isMusicOn ? 0.5f : 1));
    }

    //buttons
    //bg
    public void OnPressAnyButtonButton()
    {
        GameManager.I.Close(pressAnyButtonCanvasGroup, 0.1f);
        GameManager.I.Open(menuCanvasGroup, 0.3f);
    }
    public void OnPressBackgroundOfMenuButton()
    {
        GameManager.I.Close(menuCanvasGroup, 0.1f);
        GameManager.I.Open(pressAnyButtonCanvasGroup, 0.3f);
    }

    public void OnPlayButton()
    {
        LoadingSceneManager.I.LoadGame();
    }
    
    public void OnExitButton()
    {
        Application.Quit();
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
        musicImage.color = new Color(255, 255, 255, (val ? 0.5f : 1));
        AudioManager.I.ToggleSound(val);
    }
}
