using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerGUI : SingletonMonobehaviour<PlayerGUI>
{
    [SerializeField] GameObject textPrefab;
    [SerializeField] Transform textParent;

    public List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();


    protected override void Awake()
    {
        base.Awake();

    }

    public void AddText(string textSet, Color color)
    {
        GameObject textObj = Instantiate(textPrefab, Vector2.zero, Quaternion.identity, textParent);
        TextMeshProUGUI text = textObj.GetComponent<TextMeshProUGUI>();
        CanvasGroup textCanvasGroup = textObj.GetComponent<CanvasGroup>();
        text.text = textSet;
        text.color = color;

        texts.Add(text);
        GameManager.I.Fade(textObj, textCanvasGroup, 0.3f, 0.5f);
    }

}
