using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using ProjectColoni;
using UnityEngine;

public class UI_BuildController : MonoBehaviour
{
    private bool _buildButtonPanelOpen;
    private bool _zoneButtonPanelOpen;
    private bool _constructButtonPanelOpen;
    
    
    
    public void OnBuildButtonPress()
    {
        ResetWindows();

        _buildButtonPanelOpen = !_buildButtonPanelOpen;
        BuildingManager.Instance.inBuildingMode = _buildButtonPanelOpen;

        UI_SelectionController.ToggleWindowViewWithReset(_buildButtonPanelOpen, "Build", "BuildOptions");
    }
    
    public void OnConstructionButtonPress()
    {
        ResetBuildWindows(_constructButtonPanelOpen);
        
        _constructButtonPanelOpen = !_constructButtonPanelOpen;

        UI_SelectionController.SwitchToWindowView(_constructButtonPanelOpen, "Build", "ConstructionOptions");
    }
    
    public void OnZoneButtonPress()
    {
        ResetBuildWindows(_zoneButtonPanelOpen);

        _zoneButtonPanelOpen = !_zoneButtonPanelOpen;

        UI_SelectionController.SwitchToWindowView(_zoneButtonPanelOpen, "Build", "ZoneOptions");
    }

    private void ResetWindows()
    {
        _zoneButtonPanelOpen = false;
        _constructButtonPanelOpen = false;
    }
    
    private void ResetBuildWindows(bool active)
    {
        if (active)
        {
            return;
        }
            
        _zoneButtonPanelOpen = false;
        _constructButtonPanelOpen = false;
    }
}
