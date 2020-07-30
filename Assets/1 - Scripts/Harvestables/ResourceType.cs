using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceType : ScriptableObject
{
    [Serializable]
    public struct ResourceData
    {
        public string resourceName;
        [TextArea] public string resourceDescription;
        public Sprite resourceSprite;
        public int stackLimit;
    }
}
