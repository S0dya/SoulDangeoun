using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    public static float curMulptlier;

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
