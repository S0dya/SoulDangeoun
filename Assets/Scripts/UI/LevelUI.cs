using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] Image Highlight;

    public void HighlightLevel(bool val)
    {
        Highlight.enabled = val;
    }

    public void DestroyObject() => Destroy(gameObject);
}
