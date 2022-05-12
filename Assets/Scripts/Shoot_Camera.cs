using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shoot_Camera : MonoBehaviour
{

    public GameObject bullet;

    public float shootForce;

    public float timeBetweenShooting, reloadTime;

    public float decayTime;
    public int magSize;

    int ammoLeft, ammoShot;

    bool shooting, readyToShoot, reloading;

    public Camera PlayerCam;
    public Transform Attackpoint;

    public ParticleSystem muzzleFlash;
    public TextMeshProUGUI ammoDisplay;

    ///Debugging
    public bool allowInvoke = true;


    private void Awake() {
        
        ammoLeft = magSize;

        readyToShoot = true;

    }

    private void Update() {

        Inputs();

        if (ammoDisplay != null){
           ammoDisplay.SetText(ammoLeft + " / " + magSize);
        }
    }


    private void Inputs(){

        shooting = Input.GetKeyDown(KeyCode.Mouse0);

        ///Reloading
        if (Input.GetKeyDown(KeyCode.R) && ammoLeft < magSize && !reloading){

            Reload();

        }

        ///Reloads if no ammo left
        if (readyToShoot && shooting && !reloading && ammoLeft <= 0){

            Reload();

        }

        if (readyToShoot && shooting && !reloading && ammoLeft > 0){

            ammoShot = 0;

            Shoot();

        }

    }

    private void Shoot(){

        readyToShoot = false;

        muzzleFlash.Play();

        ///Find exact position of hit
        Ray ray = PlayerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        //checks if ray hits something
        Vector3 targetPoint;

        if (Physics.Raycast (ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); ///will take point that is far away

    
        Vector3 directionOfShoot = targetPoint - Attackpoint.position;

        ///Instantiates bullet
        GameObject currentBullet = Instantiate(bullet, Attackpoint.position, Quaternion.identity);

        ///Rotate Bullet to Direction
        currentBullet.transform.forward = directionOfShoot.normalized;

        /// Add force to Bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionOfShoot.normalized * shootForce, ForceMode.Impulse);
        ///currentBullet.GetComponent<Rigidbody>().AddForce(PlayerCam.transform.up * shootForce, ForceMode.Impulse);

        /// Destroys bullet after 3 seconds
        Destroy (currentBullet, decayTime);

        ammoLeft--;
        ammoShot++;

       if (allowInvoke){

           Invoke("ResetShot", timeBetweenShooting);
           allowInvoke = false;
       }

    }

    private void ResetShot(){

            readyToShoot = true;
            allowInvoke = true;

    }

    private void Reload(){

        reloading = true;
        Invoke("ReloadFinished", reloadTime);

    }

    private void ReloadFinished(){

        ammoLeft = magSize;
        reloading = false;
    }
}
