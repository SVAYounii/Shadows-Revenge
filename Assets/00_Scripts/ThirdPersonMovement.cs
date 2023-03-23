using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform cam;

    Animator anim;
    CharacterController characterController;
    float turnSmoothTime = .1f;
    float turnSmoothVelocity;
    Vector2 movement;
    public float walkSpeed = 3f;
    public float sprintSpeed = 5f;
    public float CrouchSpeed = 1.5f;
    bool sprinting;
    bool _isCrouching;

    [HideInInspector]
    public float trueSpeed;

    public float jumpHeight = 2f;
    public float gravity = -4f;
    bool isGrounded;
    bool isJumping;
    Vector3 velocity;

    public bool IsAttacking;
    bool ReadyForAttacking = true;
    float _delayAttack = 1.5f;
    float _nextTimeAttack;
    bool _ableToWalk = true;
    void Start()
    {
        trueSpeed = walkSpeed;
        anim = GetComponentInChildren<Animator>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Raycast to check if the character is on the ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.2f);

        // Set the "IsGrounded" parameter in the Animator
        anim.SetBool("isGrounded", isGrounded);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1;
        }

        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 direction = new Vector3(movement.x, 0, movement.y).normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            sprinting = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            sprinting = false;

        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _isCrouching = true;
            anim.SetBool("IsSneaking", _isCrouching);

        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _isCrouching = false;
            anim.SetBool("IsSneaking", _isCrouching);

        }

        if (!ReadyForAttacking)
        {
            _nextTimeAttack = Time.time + _delayAttack;
            ReadyForAttacking = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            print("Pressed");

            if (Time.time > _nextTimeAttack)
            {
                ReadyForAttacking = false;
                anim.SetTrigger("IsAttacking");
            }
        }


        anim.transform.localPosition = Vector3.zero;
        anim.transform.localEulerAngles = Vector3.zero;
        if (direction.magnitude >= 0.1f && _ableToWalk)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDirection.normalized * trueSpeed * Time.deltaTime);
            if (sprinting)
            {
                trueSpeed = sprintSpeed;
                anim.SetFloat("Speed", 2);

            }
            else
            {
                if (_isCrouching)
                {
                    trueSpeed = CrouchSpeed;
                }
                else
                {
                    trueSpeed = walkSpeed;
                }
                anim.SetFloat("Speed", 1);
            }

        }
        else
        {
            anim.SetFloat("Speed", 0);

        }
        //if (Input.GetButtonDown("Jump") && isGrounded)
        //{
        //    anim.SetBool("isJumping", true);
        //    isJumping = true;
        //    velocity.y = Mathf.Sqrt((jumpHeight * 10) * -2 * gravity);
        //}
        if (!isGrounded)
        {
            isJumping = true;
        }
        if (velocity.y > -20)
        {
            velocity.y += (gravity * 10) * Time.deltaTime;

        }
        else
        {
            if (isJumping)
            {
                anim.SetBool("isJumping", false);
                isJumping = false;
            }
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    IEnumerator StopWalking(float amount)
    {
        _ableToWalk = false;
        yield return new WaitForSeconds(amount);
        _ableToWalk = true;

    }
}
