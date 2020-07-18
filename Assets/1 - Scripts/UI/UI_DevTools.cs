using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using ProjectColoni;
using UnityEngine;
using UnityEngine.UI;

public class UI_DevTools : MonoBehaviour
{
    [SerializeField] private GameObject textBox;

    [SerializeField] private GameObject iconSelector;
    private GameObject _iconSelector;
    
    private Text _textBoxText;
    
    private bool _devPanelOpen;
    private bool _placingRandomColonist;
    private bool _textBoxActivated;
    
    private static readonly int OnClick = Animator.StringToHash("OnClick");
    private const string SPAWN_RANDOM_COLONIST_MESSAGE = "Select Where To Spawn Random Colonist. Right click to cancel.";


    private void Start()
    {
        
        _iconSelector = Instantiate(iconSelector);
        _iconSelector.SetActive(false);
        
        _textBoxText = textBox.GetComponentInChildren<Text>();
    }

    private void Update()
    {
        textBox.SetActive(_textBoxActivated);

        if (Input.GetMouseButtonDown(0))
        {
            if (_placingRandomColonist) PlaceRandomColonist(_iconSelector.transform.position);
            
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //reset textbox and wtver else is canceled by right click
            _placingRandomColonist = false; 
            _textBoxActivated = false;
            _iconSelector.SetActive(false);
        }

        if (_iconSelector.activeSelf)
        {
            var ray = SelectionManager.Instance.cam.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out var hit, Mathf.Infinity);

            _iconSelector.transform.position = hit.point;
        }

    }

    public void ToggleDevTools()
    {
        _devPanelOpen = !_devPanelOpen;
        
        if(_devPanelOpen)
            UIView.ShowView("DevToolsUI", "DevToolsPanel");
        else 
            UIView.HideView("DevToolsUI", "DevToolsPanel");
    }

    public void InitiateColonistSpawn() //does not update per frame
    {
        _placingRandomColonist = !_placingRandomColonist;
        
        //turn on placement message
        _textBoxText.text = SPAWN_RANDOM_COLONIST_MESSAGE;
        _textBoxActivated = _placingRandomColonist;
        
        //project in 3d space a model or sprite for targeting the placement
        _iconSelector.SetActive(_placingRandomColonist);
    }

    private void PlaceRandomColonist(Vector3 placePos)
    {
        var selectorAnimator = _iconSelector.GetComponent<Animator>();
        selectorAnimator.SetTrigger(OnClick);
        
        ColonistGenerator.Instance.GenerateRandomColonist(placePos);
        
        Debug.Log("Spawning Random Colonist");
    }
}
