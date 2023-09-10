using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    Player player; 
    Transform deadBodyParent;

    protected override void Awake()
    {
        base.Awake();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        deadBodyParent = GameObject.FindGameObjectWithTag("DeadBodyParent").transform;

    }

    public void ClearGame()
    {
        foreach (Transform deadBody in deadBodyParent)
        {
            Destroy(deadBody.gameObject);
        }

        player.Clear();

    }

    public void NextLevel()
    {
        ClearGame();

        LevelGenerationManager.I.GenerateLevel();
    }
}
