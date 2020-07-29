using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using ProjectColoni;
using UnityEngine;
using UnityEngine.UI;

public class BaseObjectData
{
    private string _name;
    private string _description;
    private Sprite _sprite;

    public BaseObjectData(string name, string description, Sprite sprite)
    {
        _name = name;
        _description = description;
        _sprite = sprite;
    }

    public void SetName(string name)
    {
        _name = name;
    }
    public void SetDescription(string description)
    {
        _description = description;
    }
    public void SetIcon(Sprite sprite)
    {
        _sprite = sprite;
    }
}

[CreateAssetMenu(fileName = "ObjectData", menuName = "ProjectColoni/Objects/BaseScriptableData", order = 0)]
public class BaseScriptableData : ScriptableObject
{
    public new string name;
    [TextArea] public string description;
    public Sprite sprite;
}

