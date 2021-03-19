using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public CharacterController playerController; // main player controller
    public Transform rotation; // main player rotation
    public Transform cam; // main camera
    public Animator _animator;

    CapsuleCollider playerCollider; // collider
    PlayerData data;

    public float[] position;
    public int level;

    public Camera mainCamera;
    public Camera notMainCamera;
    public Camera notMainCamera2;

    private bool simpleMovement = false;
    private bool inverted = false;

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

    // Check if something above player head then crouching, or above back then laying
    private bool aboveHead = false;
    private bool aboveBack = false;

    // Check if player in the water
    private bool inTheWater = false;

    // Controller height then crouch and lay
    private float controller_OriginalHeight;
    private float controller_CrouchHeight = 1.5f;
    private float controller_LayHeight = 0.5f;
    float x;

    // Jumping actions and restrictions
    private bool canDoubleJump = false;
    private bool canTripleJump = false;
    public float angle = 0f;
    
    // Player actions
    public bool crouching = false, jumping = false, lay = false, pounding = false, swimming = false;

    void Start(){
        playerController = GetComponent<CharacterController>();
        playerCollider = GetComponent<CapsuleCollider>();

        level = SceneManager.GetActiveScene().buildIndex;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        changableSpeed = speed;

        controller_OriginalHeight = playerController.height;

        data = SaveSystem.LoadPlayer();
        SaveSystem.SavePlayer(this);
        /*
        Vector3 position;
        if(data.position != null){
            position.x = data.position[0];
            position.y = data.position[1];
            position.z = data.position[2];   
        }
        else {
            position.x = 0f;
            position.y = 1f;
            position.z = -7f;
        }*/

        
        //transform.position = position;  
    }

    // Update is called once per frame
    void Update()
    {      
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        if(horizontal != 0 || vertical != 0)
            _animator.SetBool("Move", true);
        else
            _animator.SetBool("Move", false);

        if(!simpleMovement){

            moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
            if(moveDirection.magnitude >= 0.1f)
            {     
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(x, angle, 0f);

                moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;     
            }
        }
        else if(simpleMovement && !inverted) {
            moveDirection = new Vector3(-horizontal, 0f, -vertical).normalized;  

            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(x, angle, 0f);  
        }
        else if(simpleMovement && inverted) {
            
            moveDirection = new Vector3(horizontal, 0f, vertical).normalized;    

            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(x, angle, 0f);
        }


        if(!crouching && !lay && !swimming)
        {
            Jumping();
        }

        if(Input.GetKey(KeyCode.R))
        {
            RestartGame();
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
            _animator.SetBool("Run", true);
            changableSpeed = speed * 1.5f;
        }
        else {
            _animator.SetBool("Run", false);
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
                lay = false; Lay();
                crouching = true; Crouch();
            }

        }

        if(swimming == false)
        {
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
                directionY = -0.5f;
            }
            else {
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

        if(playerController.isGrounded) {
			_animator.SetBool("Grounded", true);
			_animator.SetBool("Jump", false);
		} else {
			_animator.SetBool("Grounded", false);
		}

        if (!playerController.isGrounded && transform.position.y - 2 < 0) {
			_animator.SetBool("Fall", true);
		}
		else{
			_animator.SetBool("Fall", false);
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
                _animator.SetBool("Jump", true);
                changableSpeed = speed;
                directionY = jumpSpeed;

                jumping = true;
            }
        } 
        else {
            if(Input.GetButtonDown("Jump") && canDoubleJump) 
            {
                _animator.SetBool("Jump", true);
                directionY = jumpSpeed * doubleJump;
                canDoubleJump = false;
                canTripleJump = true;

                jumping = true;
            }
            else if(Input.GetButtonDown("Jump") && canTripleJump)
            {
                _animator.SetBool("Jump", true);
                directionY = jumpSpeed * tripleJump;
                canTripleJump = false;
                jumping = true;
            }
        } 
        jumping = false;

    }

    // couching with animation
    void Crouch()
    {
        if(crouching) 
        {  
            _animator.SetBool("Crouch", true);
            playerController.center = new Vector3(0, -0.25f, 0);
            playerController.height = controller_CrouchHeight;        
        }
        else {
            _animator.SetBool("Crouch", false);
            playerController.center = new Vector3(0, 0, 0);
            playerController.height = controller_OriginalHeight;
           
        }
    }

    // laying
    void Lay()
    {
        if(lay) 
        {
            _animator.SetBool("Down", true);
            x = 90f;
            playerController.height = controller_LayHeight;
            playerController.radius = 0.3f;
            transform.rotation = Quaternion.Euler(x, angle, 0f);

            
        }
        else {
             _animator.SetBool("Down", false);
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

        if(other.tag == "Portal" )
        {
            // PEREITI I KITA LYGI 
            /*if(level != 2)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            else
                SceneManager.LoadScene(0);*/

            SceneManager.LoadScene(0);
        }

        if(other.tag == "CameraTrigger1")
        {
            simpleMovement = true;
            inverted = false;
            mainCamera.enabled = false;
            notMainCamera.enabled = true;
            notMainCamera2.enabled = false;
        }
        else if(other.tag == "CameraTrigger2") 
        {
            simpleMovement = true;
            inverted = true;
            mainCamera.enabled = false;
            notMainCamera.enabled = false;
            notMainCamera2.enabled = true;
        }
        else{
            simpleMovement = false;
            mainCamera.enabled = true;
            notMainCamera.enabled = false;
            notMainCamera2.enabled = false;
        }
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

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }


    

}