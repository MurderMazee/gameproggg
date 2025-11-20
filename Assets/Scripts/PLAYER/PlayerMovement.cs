using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float footstepInterval = 0.4f;
    public AudioClip footstepSound;

    Rigidbody rb;
    PlayerInput playerInput;
    AudioSource audioSource;
    Vector2 moveInput;
    float footstepTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = new PlayerInput();
        playerInput.OnFoot.Enable();
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        playerInput.OnFoot.Enable();
    }

    void OnDisable()
    {
        playerInput.OnFoot.Disable();
    }

    void Update()
    {
        moveInput = playerInput.OnFoot.Movement.ReadValue<Vector2>();
        HandleFootsteps();
    }

    void FixedUpdate()
    {
        Vector3 move = transform.forward * moveInput.y + transform.right * moveInput.x;
        rb.linearVelocity = new Vector3(move.x * moveSpeed, rb.linearVelocity.y, move.z * moveSpeed);
    }

    void HandleFootsteps()
    {
        bool grounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);
        bool moving = moveInput.magnitude > 0.1f;

        if (grounded && moving)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f && footstepSound != null)
            {
                audioSource.PlayOneShot(footstepSound);
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }
}
