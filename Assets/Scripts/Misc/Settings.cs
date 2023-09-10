using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    public static float curMulptlier;

    //player
    public static float speedMultiplier = 1;

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


    public static void Clean()
    {
        curLevel = 0;
        levelAmount = 1;
        amountOfEnemiesOnLevel = 5;
    }

    public static void NextLevel()
    {
        curLevel++;
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
