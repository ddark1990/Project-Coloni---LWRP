using System.Collections;
using System.Collections.Generic;
using Lexic;
using ProjectColoni;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ColonistGenerator : MonoBehaviour
{
    public static ColonistGenerator Instance { get; private set; }

    [SerializeField] private GameObject[] colonists;

    private NameGenerator _nameGen;
    
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
        
        _nameGen = FindObjectOfType<NameGenerator>();

    }

    void Update()
    {
        
    }

    public void GenerateRandomColonist(Vector3 placePos)
    {
        var colonist = Instantiate(colonists[0], placePos, Quaternion.identity);

        var skinnedMeshRenderer = colonist.GetComponentInChildren<SkinnedMeshRenderer>();
        
        GenerateRandomColor(skinnedMeshRenderer);
        GenerateColonistData();
        
        Debug.Log(skinnedMeshRenderer.gameObject);
    }
    
    private void GenerateRandomColor(Renderer meshRenderer) 
    {
        var newColor = new Color(Random.Range(0, 1f),Random.Range(0, 1f),Random.Range(0, 1f));

        meshRenderer.material.color = newColor;
    }

    private void GenerateColonistData()
    {
        var stats = new Stats
        {
            AiName = _nameGen.GetNextRandomName()
        };
        
        Debug.Log(stats.AiName);
    }
}
