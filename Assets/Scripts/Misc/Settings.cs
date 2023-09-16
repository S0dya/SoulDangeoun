using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    public static float curMulptlier;
    public static int crystalsAmount = 1551;

    //player
    public static float speedMultiplier = 1;

    public static Color colorHealth = new Color(255, 0, 0);
    public static Color colorMana = new Color(0, 0, 255);

    /// <summary>
    /// level generation 
    /// </summary>
    public static int width = 35;
    public static int height = 25;


    //local
    public static int curLevel = 0;
    public static int curDungeonLevel = 0;
    public static int curDungeonRealm = 0;
    public static int levelAmount = 1;
    public static int amountOfEnemiesOnLevel = 5;


    //characters 0-knight 1-assasin 2-wizard
    public static int[] healths = new int[] { 5, 4, 5 };
    public static int[] shields = new int[] { 5, 3, 4 };
    public static int[] mana = new int[] { 200, 140, 240 };

    public static int[] currentUpgrade = new int[] { 0, 0, 0 };

    public static string[] characterName = new string[] { 
        "mighty warrior", "lurking assasin", "wise wizard" };
    public static string[] characterDescription = new string[] {
        "The Knight of Some Kingdom", "Assasin of shadows", "The Wizard of ancient forest" };
    public static string[] upgradesText = new string[] {
        "Increase health by 1", "increase shield by 1", "increase mana by 40", 
        "upgrade power", "Increase health by 1", "increase shield by 1", "increase mana by 40"};

    public static string[] powersName = new string[] {
    "Double weapon", "Dash", "Splash" };
    public static string[] powersDescription = new string[] {
    "Take the weapon in another hand too!", "Makes your movement faster!", "Cast magic splash to throw enemies away!" };

    public static int[] charactersPrice = new int[] { 0, 1500, 3500 };
    public static int[] upgradePrices = new int[] { 1000, 1500, 2000 };

    //powers
    public static float[] reloadOfPowers = new float[] { 3, 3, 5 };
    public static float[] durationOfPowers = new float[] {4, 4, 6 };

    //shop in game
    public static float priceMultiplaer = 1;

    //methods
    public static void Clean()
    {
        curLevel = 0;
        levelAmount = 1;
        amountOfEnemiesOnLevel = 5;
        priceMultiplaer = 1;
    }

    public static void NextLevel()
    {
        curLevel++;
        priceMultiplaer *= 1.2f;

        if (curLevel == 4)
        {
            curLevel = 0;
            curDungeonLevel++;

            //changeLevel
        }
        else
        {
            levelAmount++;
            amountOfEnemiesOnLevel++;
        }
    }
}
