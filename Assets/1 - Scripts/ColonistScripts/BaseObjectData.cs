using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using ProjectColoni;
using UnityEngine;
using UnityEngine.UI;

public class BaseObjectData
{
    public string Id;
    public string ObjectName;
    public string Description;
    public Texture SpriteTexture;

    public BaseObjectData(string id, string baseName, string description, Texture spriteTexture)
    {
        Id = id; //must be unique
        ObjectName = baseName;
        Description = description;
        SpriteTexture = spriteTexture;
    }

    public void SetName(string name)
    {
        ObjectName = name;
    }
    public void SetDescription(string description)
    {
        Description = description;
    }
    public void SetIcon(Texture spriteTexture)
    {
        SpriteTexture = spriteTexture;
    }
}

