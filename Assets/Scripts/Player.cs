using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float rotationSpeed = 20f;

    protected StateMachine stateMachine;
    public Rigidbody rb { get; private set; }
    public Animator anim { get; private set; }

    public Vector3 moveInput { get; private set; }
    public InputSystemPlayer input { get; private set; }
    public Player_IdleState idleState { get; private set; }
    public Player_RunningState runningState { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        input = new InputSystemPlayer();
        stateMachine = new StateMachine();

        idleState = new Player_IdleState(this, stateMachine, "isMoving");
        runningState = new Player_RunningState(this, stateMachine, "isMoving");
    }

    private void OnEnable()
    {
        input.Enable();
        input.PlayerInputs.Move.performed += ctx => 
        {
            Vector2 inputValue = ctx.ReadValue<Vector2>();
            moveInput = new Vector3(inputValue.x, 0f, inputValue.y);
        };
        input.PlayerInputs.Move.canceled += ctx => 
        {
            moveInput = Vector3.zero;
        };
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.currentState?.Execute();
        HandleRotation();
    }

    public void SetVelocity(float xVelocity, float yVelocity, float zVelocity)
    {
        rb.linearVelocity = new Vector3(xVelocity, yVelocity, zVelocity);
    }

    private void HandleRotation()
    {
        if (moveInput.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            rb.rotation = Quaternion.Lerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}