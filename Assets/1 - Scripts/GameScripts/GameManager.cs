using System;
using System.Collections;
using System.Collections.Generic;
using ProjectColoni;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector] public ResourceManager resourceManager;

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

        resourceManager = GetComponent<ResourceManager>();
    }

    public void AddAiToColony(AiController ai)
    {
        AddColonistToList(ai);
        ai.playerOwned = true;
        
        UI_Controller.Instance.colonistPanel.UpdateColonistPanel(ai);
        //add colonist button to UI
    }

    private void AddColonistToList(AiController colonist)
    {
        playerControlledColonists.Add(colonist);
    }
}
