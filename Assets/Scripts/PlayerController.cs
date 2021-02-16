using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public Vector3 moveDirection;

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

    float changableGravity;
    float changableSpeed;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // Let player walk into camera
    private float directionY;

    private bool canDoubleJump = false;
    private bool canTripleJump = false;


    void Start(){
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        changableSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if(moveDirection.magnitude >= 0.1f)
        {     
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;     
        }

        if (controller.isGrounded)
        {
            canDoubleJump = true;
            canTripleJump = false;

            if (Input.GetButtonDown("Jump"))
            {
                changableSpeed = speed;
                directionY = jumpSpeed;
            }
        } 
        else {
            if(Input.GetButtonDown("Jump") && canDoubleJump) 
            {
                directionY = jumpSpeed * doubleJump;
                canDoubleJump = false;
                canTripleJump = true;
            }
            else if(Input.GetButtonDown("Jump") && canTripleJump)
            {
                directionY = jumpSpeed * tripleJump;
                canTripleJump = false;
            }
        } 

        if(Input.GetKey(KeyCode.Mouse0))
        {
            changableGravity = gravity * 10;
        } 
        else {
            changableGravity = speed;
        } 

        if (Input.GetKey(KeyCode.LeftShift) && controller.isGrounded)
        {
            changableSpeed = speed * 1.5f;
        }
        else {
            changableSpeed = speed;
        }


        directionY -= changableGravity * Time.deltaTime;
        moveDirection.y = directionY;
        controller.Move(moveDirection * changableSpeed * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(!controller.isGrounded && hit.normal.y < 0.1f)
        {
            if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                moveDirection = hit.normal;
                Debug.DrawRay(hit.point, hit.normal, Color.red, 1.25f);             
            }
        }
    }
    
}
