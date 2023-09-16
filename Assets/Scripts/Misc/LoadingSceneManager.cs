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
        //LoadMenu(false);
    }


    public void LoadMenu(bool val)
    {
        StartCoroutine(LoadMenuCor(val));
    }

    IEnumerator LoadMenuCor(bool closeGame)
    {
        if (closeGame) SceneManager.UnloadSceneAsync(2);
        AsyncOperation operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        SetFillAmount(0);
        OpenLoadingScreen();

        while (!operation.isDone)
        {
            float progression = Mathf.Clamp01(operation.progress / 0.9f);

            SetFillAmount(progression);

            yield return null;
        }

        CloseLoadingScreen();
    }

    public void LoadGame()
    {
        SceneManager.UnloadSceneAsync(1);
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);

        SetFillAmount(0);
        OpenLoadingScreen();
    }

    public void OpenLoadingScreen()
    {
        SetFillAmount(0);
        GameManager.I.Open(loadingScreenCanvasGroup, 0.1f);
    }
    public void SetFillAmount(float progression)
    {
        LoadingBarFill.fillAmount = progression;
    }
    public void CloseLoadingScreen()
    {
        GameManager.I.Close(loadingScreenCanvasGroup, 0.3f);

    }
}
