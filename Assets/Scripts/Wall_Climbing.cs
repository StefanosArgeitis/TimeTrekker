using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Climbing : MonoBehaviour
{
    
    [Header("Misc")]
    public Transform orientation;
    public Rigidbody rb;
    public LayerMask whatIsGround;
    public Player_Movement pm;


    [Header("W_Climbing")]
    public float climbSpeed;
    public float climbMaxTime;
    private float climbTimer;


    private bool climbing;

    [Header("W_Detection")]
    public float detectLength;
    public float sphereCastRadius;
    public float maxWallAngle;
    private float wallAngle;

    [Header("W_Exit")]
    public bool exitWall;
    public float exitWallTime;
    private float exitWallTimer;

    private RaycastHit frontWallHit;
    private bool frontWall;
    public KeyCode jumpKey = KeyCode.Space;
    public int jumps;
    public int jumpsLeft;
    private Transform lastWall;
    private Vector3 lastWallNormal;
    public float wallNormalAngleChange;

    [Header("Jump")]
    public float jumpUpForce;
    public float jumpBackForce;


    private void Update() {

        WallCheck();
        StateHandler();

        if (climbing && !exitWall){
            W_ClimbingMovement();
        }

    }

    private void StateHandler(){

        if ((frontWall && Input.GetKey(KeyCode.W)) && wallAngle < maxWallAngle && !exitWall){

            if (!climbing && climbTimer > 0){
                Start_W_Climbing();
            }

            if (climbTimer > 0){
                climbTimer -= Time.deltaTime;
            }

            if (climbTimer < 0){
                Stop_W_Climbing();
            }

        }else if(exitWall){

            if (climbing){
                Stop_W_Climbing();
            }

            if (exitWallTimer > 0){
                exitWallTimer -= Time.deltaTime;
            }

            if (exitWallTimer <0){
                exitWall = false;
            }

        } else{

            if (climbing){
                Stop_W_Climbing();
            }

        }

        if(frontWall && Input.GetKeyDown(jumpKey) && jumpsLeft > 0){
            W_ClimbJump();
        }

    }

    private void WallCheck(){

        frontWall = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectLength, whatIsGround);

        wallAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);
        bool newWall = frontWallHit.transform != lastWall || Mathf.Abs(Vector3.Angle(lastWallNormal, frontWallHit.normal)) > wallNormalAngleChange;


        if ((frontWall && newWall) || pm.Grounded){
            climbTimer = climbMaxTime;
            jumpsLeft = jumps;
        }
       
    }


    private void W_ClimbingMovement(){

        rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);

    }

    private void W_ClimbJump(){

        Vector3 forceApplied = transform.up * jumpUpForce + frontWallHit.normal * jumpBackForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceApplied, ForceMode.Impulse);

        exitWall = true;
        exitWallTimer = exitWallTime;

        jumpsLeft--;
    }


    private void Start_W_Climbing(){
        climbing = true;
        pm.w_climbing = true;

        lastWall = frontWallHit.transform;
        lastWallNormal = frontWallHit.normal;
    }


    private void Stop_W_Climbing(){
        climbing = false;
        pm.w_climbing = false;
    }
}
