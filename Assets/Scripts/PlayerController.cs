using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public CharacterController playerController; // main player controller
    public Transform rotation; // main player rotation
    public Transform cam; // main camera
    CapsuleCollider playerCollider; // collider
    
    public Vector3 moveDirection; // moving Vector

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



    private bool aboveHead = false;
    private bool aboveBack = false;

    private bool inTheWater = false;

    private float controller_OriginalHeight;
    private float controller_CrouchHeight = 0.5f;
    private float collider_OriginalHeight;
    private float collider_CrouchHeight = 0.5f;
    float x;

    // Jumping actions and restrictions
    private bool canDoubleJump = false;
    private bool canTripleJump = false;
    private float angle = 0f;
    
    public bool crouching = false, jumping = false, lay = false, pounding = false, swimming = false;

    void Start(){
        playerController = GetComponent<CharacterController>();
        playerCollider = GetComponent<CapsuleCollider>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        changableSpeed = speed;

       

        controller_OriginalHeight = playerController.height;
        collider_OriginalHeight = playerCollider.height;
    }


    void FixedUpdate() {

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
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(x, angle, 0f);

            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;     
        }

        if(!crouching && !lay && !swimming)
        {
            Jumping();
        }


        if(Input.GetKey(KeyCode.Mouse0))
        {
            changableGravity = gravity * 10;
            pounding = true;
        } 
        else {
            changableGravity = speed;
            pounding = false;
        } 

        if (Input.GetKey(KeyCode.LeftShift) && playerController.isGrounded && !lay && !crouching && !swimming)
        {
            changableSpeed = speed * 1.5f;
        }
        else {
            changableSpeed = speed;
        }

        // checking if can unCrouch if crouching
        if(crouching)checkCrouchAbove();
        if(!aboveHead && playerController.isGrounded)
        {
            if(Input.GetKeyDown(KeyCode.C) && !lay && !swimming)
            {
                if(crouching)
                    crouching = false;

                else 
                    crouching = true;

                    

                Crouch();
            }
            else if(Input.GetKeyDown(KeyCode.C) && lay)
            {
                lay = false;
                Lay();
                crouching = true;
                Crouch();
            }

        }

        if(swimming == false)
        {
            Debug.Log("NOT SWIMING GRAVITY BITCH");
            directionY -= changableGravity * Time.deltaTime;
        }           
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                directionY = 0.5f;
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                //x = 120f;
                directionY = -0.5f;
                //transform.rotation = Quaternion.Euler(x, angle, 0f);
            }
            else {
                //x = 45f;
                //transform.rotation = Quaternion.Euler(x, angle, 0f);
                directionY = 0f;
            }
            
        }

        // checking if can unLay if laying
        if(lay)checkLayAbove();
        if(!aboveBack && playerController.isGrounded)
        {
            if(Input.GetKeyDown(KeyCode.Z))
            {
                if(lay)
                    lay = false;               
                else 
                    lay = true;

                Lay();
            }
        }

       

        moveDirection.y = directionY;
        playerController.Move(moveDirection * changableSpeed * Time.deltaTime);
    }

    // jumping
    void Jumping()
    {
        if (playerController.isGrounded)
        {
            canDoubleJump = true;
            canTripleJump = false;

            if (Input.GetButtonDown("Jump"))
            {
                changableSpeed = speed;
                directionY = jumpSpeed;

                jumping = true;
            }
        } 
        else {
            if(Input.GetButtonDown("Jump") && canDoubleJump) 
            {
                directionY = jumpSpeed * doubleJump;
                canDoubleJump = false;
                canTripleJump = true;

                jumping = true;
            }
            else if(Input.GetButtonDown("Jump") && canTripleJump)
            {
                directionY = jumpSpeed * tripleJump;
                canTripleJump = false;
                jumping = true;
            }
        } 
        jumping = false;
    }

    // couching
    void Crouch()
    {
        if(crouching) 
        {  
            playerController.height = controller_CrouchHeight;        
        }
        else {
            playerController.height = controller_OriginalHeight;
           
        }
    }

    // laying
    void Lay()
    {
        if(lay) 
        {
            x = 90f;
            playerController.height = controller_CrouchHeight;
            playerController.radius = 0.4f;
            transform.rotation = Quaternion.Euler(x, angle, 0f);

            
        }
        else {
            x = 0f;
            playerController.height = controller_OriginalHeight;
            playerController.radius = 0.5f;
            transform.rotation = Quaternion.Euler(x, angle, 0f);    
        }
    }

    void checkCrouchAbove()
    {
        var up = transform.TransformDirection(Vector3.up);
        RaycastHit hit;  
        Debug.DrawRay(transform.position, up * 2f, Color.green);
       
        if(Physics.Raycast(transform.position, up, out hit, 2))
            aboveHead = true;
        else
            aboveHead = false;
    }

    void checkLayAbove()
    {
        var up = transform.TransformDirection(Vector3.back);
        RaycastHit hit;  
        Debug.DrawRay(transform.position, up * 2f, Color.green);
        
        if(Physics.Raycast(transform.position, up, out hit, 2))
            aboveBack = true;
        else
            aboveBack = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "BreakableBlock")
        {
            if(!playerController.isGrounded && pounding == true)
            {
                Destroy(other.gameObject);
                //Destroy(this.gameObject);
                //Debug.Log("IT WILL BREAK");
            }
        }
        else if(other.tag == "water")
        {
            swimming = true;
            Swim();
        }
        else if(other.tag == "Air")
        {
            swimming = false;

            Swim();
        }
        if(other.tag == "Portal")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void OnTriggerExit(Collider other)
    {/*
        if(other.tag == "water")
        {
            swimming = false;
            lay = false;
            Lay();
        }*/

    }


    void Swim()
    {
        if(swimming) 
        {
            x = 45f;
            playerController.height = controller_CrouchHeight;
            playerController.radius = 0.4f;
            transform.rotation = Quaternion.Euler(x, angle, 0f);

            
        }
        else {
            x = 0f;
            playerController.height = controller_OriginalHeight;
            playerController.radius = 0.5f;
            transform.rotation = Quaternion.Euler(x, angle, 0f);    
        }
    }



    

}
