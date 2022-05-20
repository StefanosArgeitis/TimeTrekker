using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FP_Camera : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform camHolder;

    float rotationX;
    float rotationY;

    private void Start() {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    private void Update() {
        
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        rotationY += mouseX;

        rotationX -= mouseY;

        /// Restricts rotation
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        /// Rotate the cam and the orientation
        camHolder.rotation = Quaternion.Euler(rotationX , rotationY, 0);
        orientation.rotation = Quaternion.Euler(0, rotationY, 0);

    }

    public void DoFov(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }
        
 }

