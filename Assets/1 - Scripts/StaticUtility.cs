using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
