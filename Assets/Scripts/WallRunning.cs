using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("WallRun")]
    public LayerMask is_wall;
    public LayerMask is_ground;
    public float wallRunForce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float wallClimbSpeed;
    public float maxWallRunTime;
    private float wallRunTimer;

    [Header("Inputs")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode upwardsRunKey = KeyCode.LeftShift;
    public KeyCode downwardsRunKey = KeyCode.LeftControl;
    private bool upwardsRunning;
    private bool downwardsRunning;
    private float horizontalInput;
    private float verticalInput;

    [Header("Detect")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;


    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;


    [Header("Gravity")]
    public bool useGravity;
    public float gravityCounterForce;


    [Header("References")]
    public Transform orientation;
    public FP_Camera cam;
    private Player_Movement pm;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start(){

        rb = GetComponent<Rigidbody>();
        pm = GetComponent<Player_Movement>();

    }

    // Update is called once per frame
    void Update(){

        CheckForWall();
        StateMachine();

    }

    private void FixedUpdate()
    {
        if (pm.wallrunning){
            WallRunningMovement();
        }
    }


    private void CheckForWall(){

        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, is_wall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, is_wall);

    }

    private bool AboveGround(){

        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, is_ground);

    }

    private void StateMachine(){
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");


        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);

        //Wallrunning
        if((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall){
            
            if (!pm.wallrunning){
                StartWallRun();

            }
            // wallrun timer
            if (wallRunTimer > 0){
                wallRunTimer -= Time.deltaTime;
            }

            if(wallRunTimer <= 0 && pm.wallrunning){
                exitingWall = true;
                exitWallTimer = exitWallTime;

            }

            // wall jump
            if (Input.GetKeyDown(jumpKey)){
                WallJump();

            } 

        } else if (exitingWall){  ///Exiting

            if (pm.wallrunning){
                StopWallRun();

            }

            if (exitWallTimer > 0){
                exitWallTimer -= Time.deltaTime;
            }

            if (exitWallTimer <= 0){
                exitingWall = false;
            }

        }else{

            if (pm.wallrunning){
                StopWallRun();
            }

        }

    }

    private void StartWallRun(){
        pm.wallrunning = true;

        wallRunTimer = maxWallRunTime;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // camera effects
        if (wallLeft) cam.DoTilt(-5f);
        if (wallRight) cam.DoTilt(5f);
    }

    private void WallRunningMovement(){
        rb.useGravity = useGravity;

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude){
            wallForward = -wallForward;
        }
        // forward force
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        // upwards/downwards force
        if (upwardsRunning){
            rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);
        }

        if (downwardsRunning){
            rb.velocity = new Vector3(rb.velocity.x, -wallClimbSpeed, rb.velocity.z);
        }

        // push to wall force
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0)){
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }

        // weaken gravity
        if (useGravity){
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
        }
            
    }

    private void StopWallRun(){
        pm.wallrunning = false;

        // reset camera effects
        cam.DoTilt(0f);

    }

    private void WallJump(){
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        // reset and add force
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);

    }
}
