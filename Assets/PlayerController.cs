using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float gravity = 9.81f;
    [SerializeField]
    private float jumpSpeed = 3.5f;
    [SerializeField]
    private float doubleJump = 0.5f;
    [SerializeField]
    private float tripleJump = 0.5f;


    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    private float directionY;
    private bool canDoubleJump = false;
    private bool canTripleJump = false;

    void Start(){
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
    }


    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 moveDirection = new Vector3();

        if(direction.magnitude >= 0.1f)
        {     
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;     
        }

        if (controller.isGrounded)
        {
            canDoubleJump = true;
            canTripleJump = true;

            if (Input.GetButtonDown("Jump"))
            {
                directionY = jumpSpeed;
            }
        } 
        else {
            if(Input.GetButtonDown("Jump") && canDoubleJump) 
            {
                directionY = jumpSpeed * doubleJump;
                canDoubleJump = false;
            }
        }

        directionY -= gravity * Time.deltaTime;
        moveDirection.y = directionY;
        controller.Move(moveDirection * speed * Time.deltaTime);
    }
    
}
