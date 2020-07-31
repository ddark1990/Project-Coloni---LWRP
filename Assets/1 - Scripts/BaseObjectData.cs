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
    public Sprite Sprite;

    public BaseObjectData(string id, string baseName, string description, Sprite sprite)
    {
        Id = id; //must be unique
        ObjectName = baseName;
        Description = description;
        Sprite = sprite;
    }

    public void SetName(string name)
    {
        ObjectName = name;
    }
    public void SetDescription(string description)
    {
        Description = description;
    }
    public void SetIcon(Sprite sprite)
    {
        Sprite = sprite;
    }
}

[CreateAssetMenu(fileName = "(BaseData)", menuName = "ProjectColoni/Objects/BaseScriptableData", order = 0)]
public class BaseScriptableData : ScriptableObject
{
    public string objectName;
    [TextArea] public string description;
    public Sprite sprite;
}

