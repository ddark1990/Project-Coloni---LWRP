using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectColoni
{
    public class UI_EscapeMenu : MonoBehaviour
{
    [SerializeField] private Slider moveSpeedSlider;
    [SerializeField] private Slider stopSpeedSlider;
    [SerializeField] private Slider rotationSpeedSlider;
    [SerializeField] private Slider scrollSensSlider;
    [SerializeField] private Slider zoomMaxHeightSlider;
    [SerializeField] private Slider zoomSmoothingSlider;
    
    private bool _showEscapeMenu;
    private bool _showOptionsMenu;
    private bool _showControlOptionsMenu;


    private void Start()
    {
        InitializeSliders();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ToggleEscapeMenu();

        UpdateSliders(); //update sliders every time slider is moved instead of every frame
        
        if(_showEscapeMenu && _showOptionsMenu) UIView.ShowView("UI" ,"EscapeMenuOptions");
        else
        {
            _showOptionsMenu = false;
            UIView.HideView("UI" ,"EscapeMenuOptions");
        }
        
        if(_showEscapeMenu && _showControlOptionsMenu) UIView.ShowView("UI" ,"EscapeMenuControlsOptions");
        else
        {
            _showControlOptionsMenu = false;
            UIView.HideView("UI" ,"EscapeMenuControlsOptions");
        }
    }

    public void ToggleEscapeMenu()
    {
        _showEscapeMenu = !_showEscapeMenu;
        
        if(_showEscapeMenu) UIView.ShowView("UI" ,"EscapeMenu");
        else UIView.HideView("UI" ,"EscapeMenu");
    }

    private void InitializeSliders()
    {
        moveSpeedSlider.value = RTSCamera.Instance.panSens;
        stopSpeedSlider.value = RTSCamera.Instance.smoothDamp;
        rotationSpeedSlider.value = RTSCamera.Instance.rotationSens;
        scrollSensSlider.value = RTSCamera.Instance.scrollZoomSensitivity;
        zoomMaxHeightSlider.value = RTSCamera.Instance.maxHeight;
        zoomSmoothingSlider.value = RTSCamera.Instance.heightDampening;
    }

    private void UpdateSliders()
    {
        RTSCamera.Instance.panSens = moveSpeedSlider.value;
        RTSCamera.Instance.smoothDamp = stopSpeedSlider.value;
        RTSCamera.Instance.rotationSens = rotationSpeedSlider.value;
        RTSCamera.Instance.scrollZoomSensitivity = scrollSensSlider.value;
        RTSCamera.Instance.maxHeight = zoomMaxHeightSlider.value;
        RTSCamera.Instance.heightDampening = zoomSmoothingSlider.value;
    }
    
    public void ToggleOptions()
    {
        _showOptionsMenu = !_showOptionsMenu;
    }

    public void ToggleControlOptions()
    {
        _showControlOptionsMenu = !_showControlOptionsMenu;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

}