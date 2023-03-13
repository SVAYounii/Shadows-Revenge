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
    bool sprinting;
    private float trueSpeed;

    public float jumpHeight = 2f;
    public float gravity =-4f;
    bool isGrounded;
    Vector3 velocity;
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
        isGrounded = Physics.CheckSphere(transform.position, .1f, 1);
        anim.SetBool("IsGrounded", isGrounded);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1;
        }


        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 direction = new Vector3(movement.x, 0, movement.y).normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            trueSpeed = sprintSpeed;
            sprinting = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            trueSpeed = walkSpeed;
            sprinting = false;

        }

        anim.transform.localPosition = Vector3.zero;
        anim.transform.localEulerAngles = Vector3.zero;
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
           
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDirection.normalized * trueSpeed * Time.deltaTime);
            if (sprinting == true)
            {
                anim.SetFloat("Speed", 2);

            }
            else
            {
                anim.SetFloat("Speed", 1);
            }
          
        }
        else
        {
            anim.SetFloat("Speed", 0);

        }
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt((jumpHeight * 10) * -2 * gravity);
        }
        if (velocity.y > -20)
        {
            velocity.y += (gravity * 10) * Time.deltaTime;
        }
    
        characterController.Move(velocity * Time.deltaTime);
    }
}
