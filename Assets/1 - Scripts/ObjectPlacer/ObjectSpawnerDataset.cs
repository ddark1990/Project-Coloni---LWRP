using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPlacerDataset", menuName = "ObjectPlacer/Dataset")]
public class ObjectSpawnerDataset : ScriptableObject
{
    public List<GameObject> objectsToPlace;
}
