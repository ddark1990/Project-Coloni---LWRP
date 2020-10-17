using System;
using System.Collections;
using System.Collections.Generic;
using ProjectColoni;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<AiController> playerControlledColonists;
    public SpriteContainer globalSpriteContainer;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddAiToColony(AiController ai)
    {
        AddColonistToList(ai);
        ai.playerOwned = true;
        
        //add colonist button to UI
    }

    private void AddColonistToList(AiController colonist)
    {
        playerControlledColonists.Add(colonist);
    }
}
