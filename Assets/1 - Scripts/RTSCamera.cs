using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectColoni
{
[RequireComponent(typeof(Camera))]
public class RTSCamera : MonoBehaviour
{
    public static RTSCamera Instance { get; private set; }

    [Header("Movement Settings")]
    public float panSens = 0.3f;
    public float smoothDamp = 4f;
    public float rotationSens = 1.5f;
    public float rotationSmooth = 25;

    [Header("Zoom Settings")]
    public float minHeight = 10f;
    public float maxHeight = 75f;
    public float scrollZoomSensitivity = 10f;
    public float heightDampening = 5f;

    [Header("Axis Strings")]
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public string zoomingAxis = "Mouse ScrollWheel";
    public string mouseHorizontalAxis = "Mouse X";
    public string mouseVerticalAxis = "Mouse Y";
    public LayerMask groundMask;

    private Vector3 _newPos;
    private Quaternion _newRot;
    private Transform _transform;
    private Vector2 _inputAxis;
    private Vector2 _mouseAxis;
    private float _mouseScroll;
    private float _zoomPos;
    private float _difference;
    private float _targetHeight;
    private float _distanceToGround;
    private bool _rotating;
    public bool cameraSmoothing;
    
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        _transform = transform;
        
        _newPos = _transform.position;
        _newRot = _transform.rotation;
    }

    private void Update()
    {
        _inputAxis = new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis)).normalized;
        _mouseAxis = new Vector2(Input.GetAxis(mouseHorizontalAxis), Input.GetAxis(mouseVerticalAxis)).normalized;
        _mouseScroll = Input.GetAxisRaw(zoomingAxis);
        
        HandleMovementInput();
        HandleRotationInput();
        HandleMouseInput();
    }
    
    private void HandleMovementInput() //add camera smoothing
    {
        var transform1 = transform;
        var facing = _inputAxis.magnitude > 0 ? transform1.forward.normalized * _inputAxis.y + transform1.right.normalized * _inputAxis.x : Vector3.zero;
        
        _newPos += facing * panSens;

        _transform.position = Vector3.Lerp(_transform.position, new Vector3(_newPos.x, _targetHeight + _difference, _newPos.z), Time.deltaTime * GetNormalizedValue(smoothDamp, 1f, 10f));
    }

    private void HandleRotationInput() //add smoothing
    {
        Cursor.visible = _rotating ? Cursor.visible = false : Cursor.visible = true;
        Cursor.lockState = _rotating ? CursorLockMode.Locked : CursorLockMode.None;

        var transformEulerAngles = _transform.eulerAngles;

        if (Input.GetKey(KeyCode.Mouse2))
        {
            _rotating = true;

            //_newRot *= Quaternion.Euler(_transform.InverseTransformDirection(new Vector3(0, _mouseAxis.x, 0)) * panTime); //old
            
            transformEulerAngles.x = 45;
            transformEulerAngles.z = 0;
            transformEulerAngles.y += _mouseAxis.x;
            
        }
        else _rotating = false;

        //_transform.rotation = Quaternion.Lerp(_transform.rotation, Quaternion.Euler(transformEulerAngles), Time.deltaTime * rotationSmooth); //old
        
        _transform.eulerAngles = Vector3.Lerp(_transform.eulerAngles, transformEulerAngles, Time.deltaTime * GetNormalizedValue(rotationSens, 1f, 200));
    }

    public float shit;
    public Vector3 fuck;

    float GetNormalizedValue(float value,float newMin,float newMax)
    {
        return newMin + value * (newMax - newMin);
    }
    
    private void HandleMouseInput()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
    
        _distanceToGround = DistanceToGround();
        
        _zoomPos += _mouseScroll * Time.deltaTime * GetNormalizedValue(scrollZoomSensitivity, 1f, 50);
        
        _zoomPos = Mathf.Clamp01(_zoomPos);

        _targetHeight = Mathf.Lerp(minHeight, maxHeight, _zoomPos);
        
        _difference = _distanceToGround != _targetHeight ? _targetHeight - _distanceToGround : 0;
        
        _transform.position = Vector3.Lerp(_transform.position, 
            new Vector3(_transform.position.x, _targetHeight + _difference, _transform.position.z), Time.deltaTime * GetNormalizedValue(heightDampening, 1f, 10));
    }
    
    private float DistanceToGround()
    {
        var ray = new Ray(_transform.position, Vector3.down);
        
        var distanceToGround = Physics.Raycast(ray, out var hit, groundMask) ? (hit.point - _transform.position).magnitude : 0f;
        
        Debug.DrawLine(ray.origin, hit.point);

        return distanceToGround;
    }
}

}