using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSmoothMovement : MonoBehaviour
{
    private float playerHeight = 2f;

    [SerializeField] private Transform orientation;
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Camera")]
    [SerializeField] private Camera cam;
    [SerializeField] private float fov;
    [SerializeField] private float runfov;
    [SerializeField] private float runfovTime;
    [SerializeField] private float movementCameraTilt;
    [SerializeField] private float momevementCameraTiltTime;
    public float tilt { get; private set; }

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float airMultiplier = 0.4f;
    [SerializeField] private float movementMultiplier = 10f;
    private float velY = 0;

    [Header("Sprinting")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float acceleration = 10f;

    [Header("Jumping")]
    [SerializeField] private float gravity = -9.81f;
    public float jumpForce = 5f;

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Drag")]
    [SerializeField] private float groundDrag = 6f;
    [SerializeField] private float airDrag = 2f;

    private float horizontalMovement;
    private float verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDistance = 0.2f;
    public bool isGrounded { get; private set; }

    private Vector3 moveDirection;
    private Vector3 slopeMoveDirection;

    private Rigidbody rb;

    private RaycastHit slopeHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if(Physics.Raycast(groundCheck.position, Vector3.down, out RaycastHit hit, groundDistance, groundMask)){
            isGrounded = true;
        }else{
            isGrounded = false;
        }

        if(isGrounded){
            velY = 0;
            rb.velocity = new Vector3(rb.velocity.x, velY, rb.velocity.z);
        }else if(!isGrounded){
            if(rb.velocity.y < 1f){
                velY += gravity * Time.deltaTime;
                rb.velocity = new Vector3(rb.velocity.x, velY, rb.velocity.z);
            }
        }

        MyInput();
        ControlDrag();
        ControlSpeed();
        OnSlope();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;

        if(Input.GetKey(KeyCode.D)){
            tilt = Mathf.Lerp(tilt, -movementCameraTilt, momevementCameraTiltTime);
        }else if(Input.GetKey(KeyCode.A)){
            tilt = Mathf.Lerp(tilt, movementCameraTilt, momevementCameraTiltTime);
        }else{
            tilt = Mathf.Lerp(tilt, 0, momevementCameraTiltTime);
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            //rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void ControlSpeed()
    {
        if (Input.GetKey(sprintKey))
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, runfov, runfovTime * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, runfovTime * Time.deltaTime);
        }
    }

    private void ControlDrag()
    {
        if (isGrounded)
        {
           rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }

    public PlayerHealth ReturnPlayerHealth(){
        return playerHealth;
    }
}