using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalk : MonoBehaviour
{

    /* ***************************************
    *  INPUT COMPONENTS
    * *************************************** */
    

    public InputActionAsset InputActions;

    private InputAction m_InputActionMove;

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

    [SerializeField] private float m_Speed = 10f;
    [SerializeField] private Transform m_CameraTransform;


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
        m_InputActionMove = InputSystem.actions.FindAction(GlobalParams.InputActionNames.MOVE);
        //m_InputActionLook = InputSystem.actions.FindAction(ActionNames.LOOK);
        //m_InputActionJump = InputSystem.actions.FindAction(ActionNames.JUMP);

        // TODO: animator
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get value from movements buttons
        m_MoveAmt = m_InputActionMove.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        Walking();
    }

    private void Walking()
    {
        // Get new direction
        Vector3 directionDep = m_CameraTransform.forward * m_MoveAmt.y + m_CameraTransform.right * m_MoveAmt.x;
        directionDep.Normalize();
        
        // Fix direction
        transform.forward = directionDep;

        // TODO: Check Sloop (?)
        // TODO: Check Grounded


        var deplace = m_Rigidbody.position + directionDep * m_Speed * Time.deltaTime;
        m_Rigidbody.MovePosition(deplace);
    }

}
