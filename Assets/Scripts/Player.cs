using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] InputActionAsset action;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jump = 2f;
    [SerializeField] private float gravity = -9.8f;
    private float cyoteTime = 0.25f;

    public float speedmult = 1f;

    private InputAction moveInputAction;
    private InputAction jumpInputAction;


    private CharacterController controller;
    private Vector2 moveInput;
    public float velocity;
    private bool Jumped = false;
    private bool DJumped = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        action.FindActionMap("Player").Enable();

        moveInputAction = InputSystem.actions.FindAction("Move");
        jumpInputAction = InputSystem.actions.FindAction("Jump");
        jumpInputAction.performed += Jump;
    }

    

    private void Jump(InputAction.CallbackContext context)
    {
        if (controller.isGrounded || cyoteTime > 0)
        {
            velocity = jump;
            print("JUMPED");
            Jumped = true;
        }
        else if (DJumped)
        {
            velocity = jump / 1.25f;
            DJumped = false;
        }
    }


    void FixedUpdate()
    {
        moveInput = moveInputAction.ReadValue<Vector2>();

        Vector3 moveDir = (Camera.main.transform.forward.normalized * moveInput.y) + (Camera.main.transform.right.normalized * moveInput.x);
        moveDir.y = 0;
        moveDir = moveDir.normalized * speed * speedmult;

        if (moveDir != Vector3.zero) transform.rotation = Quaternion.LookRotation(moveDir);
        
        

        if (!Jumped && controller.isGrounded)
        {
            cyoteTime = 0.25f;
            velocity = gravity;
            DJumped = true;
        }
        else
        {
            cyoteTime -= Time.deltaTime;
            velocity += gravity;
            Jumped = false;
        }

        moveDir.y = velocity;

        controller.Move(moveDir * Time.deltaTime);
    }


}
