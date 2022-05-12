using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{

    [Header ("Movement")]
    public float moveSpeed;
    public float walkSpeed;
    public float maxSpeed;
    public float sprintSpeed;
    public float groundDrag;
    public float slideSpeed;

    private float desMoveSpeed;
    private float finalDesMoveSpeed;
    public float speedIncreaseMult;
    public float slopeIncreaseMult;

    [Header ("Jumping")]
    public float jumpForce;
    public float JumpCooldown;
    public float airMultiplier;
    bool ready_to_jump = true;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;
    bool crouching = false;
    private RaycastHit roofHit;


    [Header("Keybinds")]
    public KeyCode jumpkey = KeyCode.Space;

    public KeyCode runkey = KeyCode.LeftShift;
    public KeyCode crouchkey = KeyCode.C;



    [Header ("CheckForGround")]
    public float playerHeight;
    public LayerMask is_ground;
    bool Grounded;

    [Header ("Slope Handler")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    public float slopeSpeedMod;
    private bool exitSlope;

    /*
    [Header ("Step Climb")]
    public GameObject stepUpper;
    public GameObject stepLower;
    public float stepHeight;
    public float stepSmoothness;
    */

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    public bool sliding;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    public enum MovementState{

        walking,
        sprinting,
        crouching,
        sliding,
        air
    }

    // Start is called before the first frame update
    private void Start()
    {

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        ready_to_jump = true;

        startYScale = transform.localScale.y;

        ///stepUpper.transform.position = new Vector3(stepUpper.transform.position.x, stepHeight, stepUpper.transform.position.z);
    }

    private void FixedUpdate() {
        
        MovePlayer();
        //stepClimb();

        Vector3 speed = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        Debug.Log("Speed: " + (speed.magnitude).ToString("F2"));
        //Debug.Log(OnSlope());
    }

    // Update is called once per frame
    private void Update()
    {
        /// Checks ground
        Grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, is_ground);
        ///Grounded = Physics.CheckSphere(GroundCheck.position, groundDistance, is_ground);

        P_Inputs();
        Speed_Control();
        StateHandle();

        if (Grounded)

            rb.drag = groundDrag;
        else
            rb.drag = 0;

        
    }

    private void P_Inputs(){

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        ///jump
        if (Input.GetKey(jumpkey) && ready_to_jump && Grounded){

            ready_to_jump = false;


            Jump();

            Invoke(nameof(JumpReset), JumpCooldown);
        }

        /// Crouching
        if(Input.GetKeyDown(crouchkey)){

            /// Cannot crouch if there is not enough space 
            if(crouching && !Physics.Raycast(transform.position, Vector3.up, out roofHit, playerHeight * 0.75f + 0.3f)){

                transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
                crouching = false;

            } else {

                transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
                rb.AddForce(Vector3.down * 7f, ForceMode.Impulse);
            
                crouching = true;
            }
            
            
        }
        

    }    


    private void StateHandle(){
        /// sliding
        if (sliding){

            state = MovementState.sliding;

            if (OnSlope() && rb.velocity.y < 0.1f){

                desMoveSpeed = slideSpeed;

            } else {
                desMoveSpeed = sprintSpeed;

            }

        } else if(crouching){
            /// Crouching
            state = MovementState.crouching;
            desMoveSpeed = crouchSpeed;

        } else if(Grounded && Input.GetKey(runkey)){
            /// Running state
            state = MovementState.sprinting;
            desMoveSpeed = sprintSpeed;

        } else if (Grounded){
            /// Walk State
            state = MovementState.walking;
            desMoveSpeed = walkSpeed;

        } else{
            state = MovementState.air;
        }

        if(Mathf.Abs(desMoveSpeed - finalDesMoveSpeed) > 6f && moveSpeed != 0){
            StopAllCoroutines();
            StartCoroutine(SmoothLerpMoveSpeed());

        } else{
            moveSpeed = finalDesMoveSpeed;

        }

        finalDesMoveSpeed = desMoveSpeed;
    }

    /// Smooths out the momentum loss
    private IEnumerator SmoothLerpMoveSpeed(){
        
        float time = 0;
        float dif = Mathf.Abs(desMoveSpeed - moveSpeed);
        float startVal = moveSpeed;

        while (time < dif){

            moveSpeed = Mathf.Lerp(startVal, desMoveSpeed, time / dif);

            if (OnSlope()){
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float angleIncrease = 1 + (angle / 90f);

                time += Time.deltaTime * speedIncreaseMult * slopeIncreaseMult * angleIncrease;

            } else {

                time += Time.deltaTime * speedIncreaseMult;
                
            }

            yield return null;
        }

        moveSpeed = desMoveSpeed;

    }

    private void MovePlayer(){

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        /// On a slope
        if (OnSlope() && !exitSlope){

            rb.AddForce(getSlopeMoveDir(moveDirection) * moveSpeed * slopeSpeedMod, ForceMode.Force);

            if(rb.velocity.y != 0){

                rb.AddForce(Vector3.down * 60f, ForceMode.Force);
            }
        }

        /// Is on ground
        if (Grounded){

            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        } else if (!Grounded){

         /// Is not grounded
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Force);
        }

        ///Remove gravity when on slope
        rb.useGravity = !OnSlope();

    }

    private void Speed_Control(){

        /// Limit speed on slope
        if(OnSlope() && !exitSlope){

            if(rb.velocity.magnitude > maxSpeed){

                rb.velocity = rb.velocity.normalized * maxSpeed;
            }

        } else{ ///Limit speed on ground and in the air

            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVelocity.magnitude > maxSpeed){

            Vector3 limitVelocity = flatVelocity.normalized * maxSpeed;
            rb.velocity = new Vector3(limitVelocity.x, rb.velocity.y, limitVelocity.z);

            }

        }

        

    }

    private void Jump(){

       Grounded = false;

       exitSlope = true;
       
        /// Resets y velocity (Jump the same height)
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

    }    

    private void JumpReset(){
        ready_to_jump = true;

        exitSlope = false;

    }

    public bool OnSlope(){

        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f)){

            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 getSlopeMoveDir(Vector3 direction){

        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    ////// TODO    
/*    private void stepClimb(){
        
        if (moveDirection != Vector3.zero && !OnSlope()){

            

        }
        
        //RaycastHit hitLower;
        //if (Physics.Raycast(stepLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.2f))


    }
    */
}
