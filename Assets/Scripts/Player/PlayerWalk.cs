using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalk : MonoBehaviour
{

    /* ***************************************
    *  INPUT COMPONENTS
    * *************************************** */
    public class ActionNames
    {
        public const string MOVE = "Move";
        public const string LOOK = "Look";
        public const string JUMP = "Jump";
    }

    public InputActionAsset InputActions;

    private InputAction m_InputActionMove;
    private InputAction m_InputActionLook;
    private InputAction m_InputActionJump;

    private Vector2 m_MoveAmt;
    private Vector2 m_LookAmt;

    /* ***************************************
    *  PHYSICAL COMPONENTS
    * *************************************** */

    // TODO: private Animator m_Animator;
    private Rigidbody m_Rigidbody;


    /* ***************************************
    *  PLAYER VARIABLES
    * *************************************** */

    public float WalkSpeed = 5;
    public float RotateSpeed = 5;
    public float JumpForce = 5;


    private void OnEnable()
    {
        InputActions.FindActionMap(GlobalParams.InputMapName.PLAYER).Enable();
    }
    
    private void OnDisable() 
    {
        InputActions.FindActionMap(GlobalParams.InputMapName.PLAYER).Disable();
    }

    void Awake()
    {
        m_InputActionMove = InputSystem.actions.FindAction(ActionNames.MOVE);
        m_InputActionLook = InputSystem.actions.FindAction(ActionNames.LOOK);
        m_InputActionJump = InputSystem.actions.FindAction(ActionNames.JUMP);

        // TODO: animator
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        m_MoveAmt = m_InputActionMove.ReadValue<Vector2>();
        m_LookAmt = m_InputActionLook.ReadValue<Vector2>();

        if (m_InputActionJump.WasPressedThisFrame())
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        Walking();
        Rotating();
    }
    private void Jump()
    {
        var jumpVector = new Vector3(0f, JumpForce, 0f);
        m_Rigidbody.AddForceAtPosition(jumpVector, Vector3.up, ForceMode.Impulse);
        // TODO: Trigger animation JUMP
    }

    private void Rotating()
    {
        if(m_MoveAmt.y != 0) // rotate the camera to follow the view of player
        {
            float rotationAmount = m_LookAmt.x * RotateSpeed * Time.deltaTime;
            // Just rotate in Y 
            Quaternion deltaRotation = Quaternion.Euler(0f, rotationAmount, 0f);
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * deltaRotation);
        }
    }

    private void Walking()
    {
        // TODO : Animator walk
        var direction = transform.forward * m_MoveAmt.y;
        var deplace = m_Rigidbody.position + direction * WalkSpeed * Time.deltaTime;
        m_Rigidbody.MovePosition(deplace);
    }

}
