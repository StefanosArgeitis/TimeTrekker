using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    
        [Header("Refrence")]
        public Transform orientation;
        public Transform playerObj;
        private Rigidbody rb;
        private Player_Movement pm;


        [Header("Slide")]
        public float slideMaxTime;
        public float slideForce;
        private float slideTimer;

        [Header("Inputs")]
        public KeyCode slideKey = KeyCode.LeftControl;
        private float horizontalInput;
        private float verticalInput;


        public float slideYScale;
        private float startYScale;
        private RaycastHit roofHit;
        public float playerHeight;

        private void Start() {
            
            rb = GetComponent<Rigidbody>();
            pm = GetComponent<Player_Movement>();

            startYScale = playerObj.localScale.y;

        }


        private void Update() {
            
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0)){
                StartSlide();

            }

            if (Input.GetKeyUp(slideKey) && pm.sliding){
                StopSlide();

            }
        }

        private void FixedUpdate() {

            if (pm.sliding){
                SlideMovement();
            }

        }

        private void SlideMovement(){

            Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
            
            /// Sliding on ground or up slope
            if (pm.OnSlope() || rb.velocity.y > -0.1f){

                rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
                slideTimer -= Time.deltaTime;

            } else {    /// Sliding down slope

            rb.AddForce(pm.getSlopeMoveDir(inputDirection) * slideForce, ForceMode.Force);
                              
            }


            if(slideTimer <= 0){
                StopSlide();
            }
            
        }

        private void StartSlide(){

            pm.sliding = true;

            playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

            slideTimer = slideMaxTime;
        }

        private void StopSlide(){

            pm.sliding = false;
            playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
            
        }
}
