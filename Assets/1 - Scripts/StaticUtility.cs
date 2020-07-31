using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public static class StaticUtility 
{
    public static Vector3 GetRandomRadialPos(Transform transform, float radiusSize)
    {
        var randomPos = transform.position + OnUnitSphere() * radiusSize;
        return randomPos;
    }
    
    private static Vector3 OnUnitSphere()
    {
        return new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y);
    }

    public static string GenerateUniqueHashId()
    {
        return Guid.NewGuid().ToString();
    }
}
