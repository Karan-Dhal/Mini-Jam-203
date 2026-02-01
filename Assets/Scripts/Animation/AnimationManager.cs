using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator animator;
    private CharacterController controller;
    private Player player;

    [Header("Speed to start runninng anim")]
    [SerializeField] private float runThreshold = 6.0f;

    private bool wasGrounded = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponentInParent<CharacterController>();
        player = GetComponentInParent<Player>();
    }

    void Update()
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
        }

        bool isFalling = !controller.isGrounded && player.velocity < -1f;
        animator.SetBool("Falling", isFalling);

        wasGrounded = controller.isGrounded;
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