using System;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private Transform m_LookAtDefault;
    private CinemachineVirtualCameraBase m_Camera;
    private Transform m_LookAtCurrent;


    // Inputs
    public InputActionAsset InputActions;
    private InputAction m_InputActionRotateCamera;
    private float m_RotateCameraValue;


    void Start()
    {
        m_Camera = GetComponent<CinemachineVirtualCameraBase>();
        m_LookAtDefault = m_Camera.LookAt;
        m_LookAtCurrent = m_LookAtDefault;

        //events (?)
    }

    void Awake()
    {
        m_InputActionRotateCamera = InputSystem.actions.FindAction(GlobalParams.InputActionNames.ROTATE_CAMERA);
    }

     private void OnEnable()
    {
        InputActions.FindActionMap(GlobalParams.InputMapName.PLAYER).Enable();
    }
    
    private void OnDisable() 
    {
        InputActions.FindActionMap(GlobalParams.InputMapName.PLAYER).Disable();
    }

    private void RefreshCamera()
    {
        m_Camera.LookAt = m_LookAtCurrent;
        m_Camera.Follow = m_LookAtCurrent;
    }

    void Update()
    {
        m_RotateCameraValue = m_InputActionRotateCamera.ReadValue<float>();
    }

    void FixedUpdate()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        Vector3 directionRotation = Vector3.up * m_RotateCameraValue;

        if (directionRotation.magnitude > 0)
        {
            transform.Rotate(directionRotation, Space.World);

            // TODO Show and Hide walls
        }

        //Triiger game event
        GameEvent.Instance.TriggerEvent(GameEvent.Id.RotateCamera);
        
    }
}
