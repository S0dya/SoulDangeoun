using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : SingletonMonobehaviour<Menu>
{
    [SerializeField] CanvasGroup menuCanvasGroup;
    [SerializeField] CanvasGroup pressAnyButtonCanvasGroup;

    protected override void Awake()
    {
        base.Awake();

    }
    void Start()
    {

    }

    //buttons
    //bg
    public void OnPressAnyButtonButton()
    {
        Debug.Log("DDD");
        GameManager.I.Close(pressAnyButtonCanvasGroup, 0.1f);
        GameManager.I.Open(menuCanvasGroup, 0.3f);
    }
    public void OnPressBackgroundOfMenuButton()
    {
        Debug.Log("D");
        GameManager.I.Close(menuCanvasGroup, 0.1f);
        GameManager.I.Open(pressAnyButtonCanvasGroup, 0.3f);
    }

    public void OnPlayButton()
    {
        Debug.Log("DaDD");
        LoadingSceneManager.I.LoadGame();
    }
    public void OnSettingsButton()
    {

    }
    public void OnExitButton()
    {
        Debug.Log("DDDasdasd");
        Application.Quit();
    }
}
