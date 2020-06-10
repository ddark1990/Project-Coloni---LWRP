using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;

public class UI_EscapeMenu : MonoBehaviour
{
    public bool _showEscapeMenu;
    
    void Update()
    {
        ToggleEscapeMenu();
    }

    private void ToggleEscapeMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            _showEscapeMenu = !_showEscapeMenu;
        
        if(_showEscapeMenu) UIView.ShowView("UI" ,"EscapeMenu");
        else UIView.HideView("UI" ,"EscapeMenu");
    }
}
