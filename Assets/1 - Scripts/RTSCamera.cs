using System;
using UnityEngine;

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
        
        _newPos = _newPos += facing * panSens;

        _transform.position = Vector3.Lerp(_transform.position, new Vector3(_newPos.x, _targetHeight + _difference, _newPos.z), Time.deltaTime * smoothDamp);
    }

    private void HandleRotationInput()
    {
        Cursor.visible = _rotating ? Cursor.visible = false : Cursor.visible = true; //shows and hides mouse when rotating

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

        //_transform.rotation = Quaternion.Lerp(_transform.rotation, _newRot, Time.deltaTime * rotationAmount); //old
        
        _transform.eulerAngles = Vector3.Lerp(_transform.eulerAngles, transformEulerAngles, Time.deltaTime * rotationSens);
    }

    private void HandleMouseInput()
    {
        _distanceToGround = DistanceToGround();
        
        _zoomPos += _mouseScroll * Time.deltaTime * scrollZoomSensitivity;
        
        _zoomPos = Mathf.Clamp01(_zoomPos);

        _targetHeight = Mathf.Lerp(minHeight, maxHeight, _zoomPos);
        
        _difference = _distanceToGround != _targetHeight ? _targetHeight - _distanceToGround : 0;
        
        _transform.position = Vector3.Lerp(_transform.position, 
            new Vector3(_transform.position.x, _targetHeight + _difference, _transform.position.z), Time.deltaTime * heightDampening);
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