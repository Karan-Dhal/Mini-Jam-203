using System.Collections;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator animator;
    private CharacterController controller;
    private Player player;

    [Header("Speed to start runninng anim")]
    [SerializeField] private float runThreshold = 6.0f;

    private bool wasGrounded = false;

    private bool bugHappenedForAFrame = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponentInParent<CharacterController>();
        player = GetComponentInParent<Player>();
    }

    void FixedUpdate()
    {
        UpdateMovementStates();
    }

    private void UpdateMovementStates()
    {
        Vector3 horizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        float currentSpeed = horizontalVelocity.magnitude;

        bool isMoving = currentSpeed > 0.1f;
        bool isRunning = currentSpeed > runThreshold;

        animator.SetBool("Walking", isMoving && !isRunning);
        animator.SetBool("Running", isRunning);

        if (controller.isGrounded && !wasGrounded)
        {
            animator.SetBool("JustFell", true);

            //GARBAGE BUGFIX
            StartCoroutine(disableJustFell());
        }

        bool isFalling = !controller.isGrounded && player.velocity < -1f;
        animator.SetBool("Falling", isFalling);

        wasGrounded = controller.isGrounded;

        //GARBAGE BUGFIX 
        if(animator.GetBool("JustFell") && animator.GetBool("JustJumped"))
        {
            if(bugHappenedForAFrame)
            {
                animator.SetBool("JustFell", false);
                animator.SetBool("JustJumped", false);
            }
            else
            {
                bugHappenedForAFrame = true;
            }
        }
        else
        {
            bugHappenedForAFrame = false;
        }
    }

    //GARBAGE BUGFIX
    IEnumerator disableJustFell()
    {
        yield return new WaitForSecondsRealtime(0.6f);
        animator.SetBool("JustFell", false);
    }

    public void TriggerJumpAnimation()
    {
        animator.SetBool("JustJumped", true);
    }

    public void TriggerPunch()
    {
        animator.SetBool("Punching", true);
    }

    public void PunchingAnimEnded()
    {
        animator.SetBool("Punching", false);
    }

    public void JustJumpedAnimEnded()
    {
        animator.SetBool("JustJumped", false);
    }

    public void JustFellAnimEnded()
    {
        animator.SetBool("JustFell", false);
    }
}