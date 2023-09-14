using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubRoom : MonoBehaviour
{
    [SerializeField] BoxCollider2D collider;
    [SerializeField] Transform[] choosableCharactersTransform;

    //set
    [SerializeField] SO_Weapon[] weapons;
    [SerializeField] SO_Power[] powers;
    [SerializeField] Sprite[] sprites;

    //UI
    [SerializeField] CanvasGroup characterPanelCG;

    //player
    GameObject playerObj;
    Player player;

    int curIndex;

    void Awake()
    {
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LevelGenerationManager.I.GenerateLevel();
        }
    }



    //buttons
    public void OnLookAtCharacterButton(int index)
    {
        curIndex = index;
        GameManager.I.Open(characterPanelCG, 0.4f);
    }
    public void OnStopLookingAtCharacterButton()
    {
        curIndex = -1;
        GameManager.I.Close(characterPanelCG, 0.1f);
    }

    public void OnChooseCharacterButton()
    {
        if (curIndex == -1) return;//dellater
        playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<Player>();

        playerObj.transform.position = choosableCharactersTransform[curIndex].position;
        Destroy(choosableCharactersTransform[curIndex].gameObject);

        player.maxStats = new int[] { Settings.healths[curIndex], Settings.shields[curIndex], Settings.mana[curIndex] };
        player.weapons[0] = weapons[curIndex];
        player.power = powers[curIndex];

        var playerSR = playerObj.GetComponent<SpriteRenderer>();
        playerSR.sprite = sprites[curIndex];

        player.EnablePlayer();
    }

    public void OnUpgradeCharacterButton()
    {

    }
}
