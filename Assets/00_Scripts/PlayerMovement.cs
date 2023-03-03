using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Refrences")]
    public Transform Orientation;
    public Transform Player;
    public Transform PlayerObj;
    Rigidbody rb;
    Animator animator;

    [Header("Movement")]
    public float WalkSpeed;
    public float SprintSpeed;
    public float RotationSpeed;
    public float GroundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    bool isRunning;
    bool isWalking;
    bool isStandingStill;
    bool isStandingStillLong;

    [Header("Movement")]
    public float PlayerHeight;
    public LayerMask GroundLayer;
    bool isGrounded;

    Vector3 moveDirection;
    float horizontalInput;
    float verticalInput;

    //Animation
    float timeUntilNextIdle;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
        timeUntilNextIdle = Time.time + 5;

    }


    // Update is called once per frame
    void Update()
    {

        GetViewDirection();
        MyInput();
        SpeedControl();
        StartAnimation();

        Debug.DrawRay(transform.position, Vector3.down, Color.green, PlayerHeight * 0.5f);
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f, GroundLayer);
        Vector3 inputDir = Orientation.forward * verticalInput + Orientation.right * horizontalInput;
        animator.SetFloat("Velocity", rb.velocity.magnitude);
        Debug.Log(rb.velocity.magnitude);


        if (rb.velocity.magnitude > 0 && rb.velocity.magnitude < SprintSpeed - 1)
        {
            isWalking = true;
            isStandingStill = false;
            isStandingStill = false;
            isRunning = false;
        }
        else if (rb.velocity.magnitude >= SprintSpeed - 0.5f)
        {
            isWalking = false;
            isStandingStill = false;
            isStandingStill = false;
            isRunning = true;

        }
        else if (rb.velocity.magnitude < 0.05f)
        {
            isWalking = false;
            isRunning = false;

        }

        if (isGrounded)
            rb.drag = GroundDrag;
        else
            rb.drag = 0;


        if (inputDir != Vector3.zero)
        {
            timeUntilNextIdle = Time.time + 10;
            isStandingStill = false;
            //we arew moving so idle animation off
            isStandingStillLong = false;
            if (isGrounded)
            {

                PlayerObj.forward = Vector3.Slerp(PlayerObj.forward, inputDir.normalized, Time.deltaTime * RotationSpeed);
            }
            else
            {
                PlayerObj.forward = Vector3.Slerp(PlayerObj.forward, inputDir.normalized, Time.deltaTime * 1.5f);

            }
        }
        else
        {
            if (Time.time > timeUntilNextIdle)
            {
                //do next idle animation
                isStandingStillLong = true;
                isStandingStill = false;

                return;
            }
            isStandingStill = true;
        }


    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void StartAnimation()
    {
        if (isWalking)
        {
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsStanding", false);
            animator.SetBool("IsStandongLong", false);


        }
        else if (isRunning)
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsStanding", false);
            animator.SetBool("IsStandongLong", false);
        }
        else if (isStandingStill)
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsStanding", true);
            animator.SetBool("IsStandongLong", false);
        }
        else if (isStandingStillLong)
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsStanding", false);
            animator.SetBool("IsStandongLong", true);
        }
    }

    void GetViewDirection()
    {
        Vector3 viewDir = Player.position - new Vector3(Camera.main.transform.position.x, Player.position.y, Camera.main.transform.position.z);
        Orientation.forward = viewDir.normalized;
    }
    void MyInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        // when to jump
        if (Input.GetKey(KeyCode.Space) && readyToJump && isGrounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > WalkSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * WalkSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    void MovePlayer()
    {
        moveDirection = Orientation.forward * verticalInput + Orientation.right * horizontalInput;
        Debug.Log("Move: " + moveDirection.magnitude);
        // on ground
        if (isGrounded)
        {
            isRunning = Input.GetKey(KeyCode.LeftShift) && moveDirection.magnitude > 0;
            if (isRunning)
            {
                if (rb.velocity.magnitude < SprintSpeed)
                {

                    WalkSpeed += Time.deltaTime * 5f;
                }
            }
            else if (!isRunning)
            {
                if (rb.velocity.magnitude > 3)
                {

                    WalkSpeed -= Time.deltaTime * 5f;
                }
                else
                {
                    WalkSpeed = 3f;
                }
            }
            rb.AddForce(moveDirection.normalized * WalkSpeed * 10f, ForceMode.Force);
        }

        // in air
        else if (!isGrounded)
            rb.AddForce(moveDirection.normalized * WalkSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

}
