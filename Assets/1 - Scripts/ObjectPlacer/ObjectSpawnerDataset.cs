using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPlacerDataset", menuName = "ObjectPlacer/Dataset", order = 5)]
public class ObjectSpawnerDataset : ScriptableObject
{
    public List<GameObject> objectsToPlace;
}
