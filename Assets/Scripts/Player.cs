using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] InputActionAsset action;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jump = 2f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private int health = 3;
    private int _health;
    [SerializeField] private int hurtTime = 5;
    private bool hurt = false;
    private float cyoteTime = 0.25f;
    [SerializeField, Range(0.00f, 1.00f)] private float airControl = 1f;
    [SerializeField, Range(0.00f, 1.00f)] private float airDempen = 1f;

    public float speedmult = 1f;

    private InputAction moveInputAction;
    private InputAction jumpInputAction;


    private CharacterController controller;
    private Vector2 moveInput;
    public float velocity;
    private bool Jumped = false;
    private bool DJumped = false;
    public Vector3 movingPlatform = Vector3.zero;
    private Vector3 airVelocity = Vector3.zero;
    private bool dead = false;
    private float deadTime = 5;

    private AnimationManager animManager;

    [Header("Higher values = faster snapping")]
    [SerializeField] private float rotationSpeed = 10f;

    void Start()
    {
        animManager = GetComponentInChildren<AnimationManager>();

        AudioManager.Instance.PlayGameplayMusic();

        _health = health;
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
            Jumped = true;

            airVelocity = controller.velocity;

            animManager.TriggerJumpAnimation();
            AudioManager.Instance.PlayJump();
        }
        else if (DJumped)
        {
            velocity = jump / 1.25f;
            DJumped = false;
        }
    }

    void FixedUpdate()
    {
        if (dead)
        {
            AudioManager.Instance.StopFootsteps();
            return;
        }

        moveInput = moveInputAction.ReadValue<Vector2>();

        if (controller.isGrounded && moveInput.sqrMagnitude > 0.01f)
            AudioManager.Instance.StartFootsteps();
        else
            AudioManager.Instance.StopFootsteps();

        Vector3 moveDir = (Camera.main.transform.forward.normalized * moveInput.y) + (Camera.main.transform.right.normalized * moveInput.x);
        moveDir.y = 0;
        moveDir = moveDir.normalized * speed * speedmult;

        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

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
            gameObject.transform.position = Vector3.zero;
        }



        if (controller.isGrounded)
        {
            moveDir = Vector3.ClampMagnitude(moveDir, speed * speedmult);
            moveDir.y = velocity;
            moveDir += movingPlatform;
            controller.Move(moveDir * Time.deltaTime);

        }
        else
        {
            airVelocity = new Vector3(airVelocity.x, 0, airVelocity.z) * airDempen + moveDir * airControl;
            airVelocity = Vector3.ClampMagnitude(airVelocity, speed * speedmult);
            airVelocity.y = velocity;
            controller.Move(airVelocity * Time.deltaTime);
        }

        movingPlatform = Vector3.zero;
    }

    public void Damage(int damage)
    {
        if (hurt) return;
        
        health -= damage;
        if (health <= 0)
        {
            //Play Death
            dead = true;
            gameObject.GetComponent<Checkpoint>().ReturnToCheckpoint();
            StartCoroutine(Deady());
        }
        else
        {
            hurt = true;
            //updateUi health
            StartCoroutine(Hurty());
        }
    }

    IEnumerator Hurty()
    {
        yield return new WaitForSeconds(hurtTime);
        hurt = false;
    }
    IEnumerator Deady()
    {
        AudioManager.Instance.PlayDeathMusic();
        yield return new WaitForSeconds(deadTime);
        AudioManager.Instance.PlayGameplayMusic();
        dead = false;
    }

    public void ResetHealth()
    {
        health = _health;
    }
}