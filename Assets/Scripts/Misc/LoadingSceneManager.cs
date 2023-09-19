using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : SingletonMonobehaviour<LoadingSceneManager>
{
    [SerializeField] CanvasGroup loadingScreenCanvasGroup;
    [SerializeField] Image LoadingBarFill;

    protected override void Awake()
    {
        base.Awake();

    }

    void Start()
    {
        LoadMenu(false);
    }


    public void LoadMenu(bool val)
    {
        AudioManager.I.StopMusic();
        AudioManager.I.EventInstancesDict["MusicMainMenu"].start();
        StartCoroutine(LoadSceneCor(val, 2, 1));
    }
    
    public void RestartGame()
    {
        StartCoroutine(LoadSceneCor(true, 2, 2));
    }

    public void LoadGame()
    {
        OpenLoadingScreen();

        SceneManager.UnloadSceneAsync(1);
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
    }
    
    
    IEnumerator LoadSceneCor(bool closeGame, int sceneToClose, int SceneToOpen)
    {
        if (closeGame) SceneManager.UnloadSceneAsync(sceneToClose);
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneToOpen, LoadSceneMode.Additive);

        OpenLoadingScreen();

        while (!operation.isDone)
        {
            float progression = Mathf.Clamp01(operation.progress / 0.9f);

            SetFillAmount(progression);

            yield return null;
        }

        CloseLoadingScreen();
    }

    
    public void OpenLoadingScreen()
    {
        if (Settings.isMusicOn)
        {
            AudioManager.I.ToggleSound(false);
        }
        SetFillAmount(0);
        GameManager.I.Open(loadingScreenCanvasGroup, 0.1f);
    }
    public void SetFillAmount(float progression)
    {
        LoadingBarFill.fillAmount = progression;
    }
    public void CloseLoadingScreen()
    {
        if (Settings.isMusicOn)
        {
            AudioManager.I.ToggleSound(true);
        }
        GameManager.I.Close(loadingScreenCanvasGroup, 0.2f);
    }
}
