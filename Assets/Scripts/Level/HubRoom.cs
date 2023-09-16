using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HubRoom : MonoBehaviour
{
    [SerializeField] BoxCollider2D collider;
    [SerializeField] Transform zeroZeroCamTransform;
    [SerializeField] Transform[] choosableCharactersTransform;

    [Header("Set")]
    [SerializeField] SO_Weapon[] weapons;
    [SerializeField] Sprite[] sprites;
    [SerializeField] Sprite[] powersImages;

    [Header("UI")]
    [SerializeField] CanvasGroup characterPanelCG;

    [SerializeField] TextMeshProUGUI crystalsAmount;

    [SerializeField] GameObject buyPanel;
    [SerializeField] GameObject boughtPanel;
    [SerializeField] TextMeshProUGUI buyPriceText;
    [SerializeField] TextMeshProUGUI upgradePriceText;

    [SerializeField] TextMeshProUGUI characterName;
    [SerializeField] TextMeshProUGUI characterDescription;

    [SerializeField] TextMeshProUGUI powerName;
    [SerializeField] Image powerImage;
    [SerializeField] TextMeshProUGUI powerDescription;
    
    [SerializeField] TextMeshProUGUI[] statsBarDescription;
    [SerializeField] TextMeshProUGUI upgradeDescription;

    //player
    GameObject playerObj;
    Player player;

    int curIndex;

    void Awake()
    {
        crystalsAmount.text = Settings.crystalsAmount.ToString();
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
        SetCharacterInfo();
        CinemachineCamera.I.ChangeCameraFollow(choosableCharactersTransform[curIndex]);
        CinemachineCamera.I.MoveCameraCloser();
        GameManager.I.Open(characterPanelCG, 0.4f);
    }
    public void OnStopLookingAtCharacterButton()
    {
        if (curIndex == -1) return;
        curIndex = -1;
        CinemachineCamera.I.ChangeCameraFollow(zeroZeroCamTransform);
        CinemachineCamera.I.MoveCameraFurther();
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
        PowerButton.I.ChangePower(curIndex);

        var playerSR = playerObj.GetComponent<SpriteRenderer>();
        playerSR.sprite = sprites[curIndex];

        GameManager.I.Close(characterPanelCG, 0.1f);
        CinemachineCamera.I.ChangeCameraFollow(playerObj.transform);
        CinemachineCamera.I.MoveCameraForPlayer();

        player.EnablePlayer();
    }
    
    public void OnBuyButton()
    {
        if (Settings.crystalsAmount >= Settings.charactersPrice[curIndex])
        {
            ChangeCrystals(-Settings.charactersPrice[curIndex]);
            ToggleBuyPanel(false);
        }
    }
    
    public void OnUpgradeCharacterButton()
    {
        if (Settings.crystalsAmount >= Settings.upgradePrices[curIndex] 
            && Settings.currentUpgrade[curIndex] < 7)
        {
            ChangeCrystals(-Settings.upgradePrices[curIndex]);
            int nextPrice = 500;

            switch (Settings.currentUpgrade[curIndex])
            {
                case 0:
                case 4:
                    Settings.healths[curIndex]++;
                    nextPrice += 250;
                    break;
                case 1:
                case 5:
                    Settings.shields[curIndex]++;
                    nextPrice += 500;
                    break;
                case 2:
                case 6:
                    Settings.mana[curIndex]++;
                    nextPrice += 150;
                    break;
                case 3:

                    nextPrice += 700;
                    break;
                default: break;
            }

            Settings.upgradePrices[curIndex] += nextPrice;
            Settings.currentUpgrade[curIndex]++;

            SetLeftStats();
        }
    }

    //methods
    void SetCharacterInfo()
    {
        ToggleBuyPanel(Settings.charactersPrice[curIndex] > 0);
        buyPriceText.text = Settings.charactersPrice[curIndex].ToString();

        buyPanel.SetActive(Settings.charactersPrice[curIndex] > 0);
        buyPriceText.text = Settings.charactersPrice[curIndex].ToString();

        characterName.text = Settings.characterName[curIndex];
        characterDescription.text = Settings.characterDescription[curIndex];

        powerName.text = Settings.powersName[curIndex];
        powerImage.sprite = powersImages[curIndex];
        powerDescription.text = Settings.powersDescription[curIndex];

        SetLeftStats();
    }

    void SetLeftStats()
    {
        upgradePriceText.text = Settings.upgradePrices[curIndex].ToString();
        upgradeDescription.text = Settings.upgradesText[Settings.currentUpgrade[curIndex]];

        statsBarDescription[0].text = Settings.healths[curIndex].ToString();
        statsBarDescription[1].text = Settings.shields[curIndex].ToString();
        statsBarDescription[2].text = Settings.mana[curIndex].ToString();
    }

    void ChangeCrystals(int val)
    {
        Settings.crystalsAmount = Math.Max(Settings.crystalsAmount + val, 0);
        crystalsAmount.text = Settings.crystalsAmount.ToString();
    }

    void ToggleBuyPanel(bool val)
    {
        buyPanel.SetActive(val);
        boughtPanel.SetActive(!val);
    }
}
