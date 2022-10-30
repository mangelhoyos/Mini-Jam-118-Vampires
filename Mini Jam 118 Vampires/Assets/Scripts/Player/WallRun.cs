using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform orientation;

    [Header("Detection")]
    [SerializeField] private float wallDistance = .5f;
    [SerializeField] private float minimumJumpHeight = 1.5f;
    [SerializeField] private LayerMask wallLayerMask;

    [Header("Wall Running")]
    [SerializeField] private float wallRunGravity;
    [SerializeField] private float wallRunJumpForce;

    [Header("Camera")]
    [SerializeField] private Camera cam;
    [SerializeField] private float fov;
    /* [SerializeField] private float wallRunfov;
    [SerializeField] private float wallRunfovTime;
    [SerializeField] private float cameraTilt;
    [SerializeField] private float cameraTiltTime; */
    [SerializeField] private float movementCameraTilt;
    [SerializeField] private float momevementCameraTiltTime;
    [SerializeField] private float runfov;
    [SerializeField] private float runfovTime;

    public float tilt { get; private set; }

    private bool wallLeft = false;
    private bool wallRight = false;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    private Rigidbody rb;

    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance, wallLayerMask);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance, wallLayerMask);
    }

    private void Update()
    {   
        if(Input.GetKey(KeyCode.D)/*  && !wallLeft && !wallRight */){
            tilt = Mathf.Lerp(tilt, -movementCameraTilt, momevementCameraTiltTime);
        }else if(Input.GetKey(KeyCode.A)/*  && !wallLeft && !wallRight */){
            tilt = Mathf.Lerp(tilt, movementCameraTilt, momevementCameraTiltTime);
        }else{
            tilt = Mathf.Lerp(tilt, 0, momevementCameraTiltTime);
        }

        if(Input.GetKey(KeyCode.LeftShift)){
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, runfov, runfovTime * Time.deltaTime);
        }else{
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, runfovTime * Time.deltaTime);
        }

        /* CheckWall();

        if (CanWallRun())
        {
            if (wallLeft)
            {
                StartWallRun();
                Debug.Log("wall running on the left");
            }
            else if (wallRight)
            {
                StartWallRun();
                Debug.Log("wall running on the right");
            }
            else
            {
                StopWallRun();
            }
        }
        else
        {
            StopWallRun();
        } */
    }

    void StartWallRun()
    {
        rb.useGravity = false;

        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        //cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunfov, wallRunfovTime * Time.deltaTime);

        if (wallLeft)
        {
            //tilt = Mathf.Lerp(tilt, -cameraTilt, cameraTiltTime * Time.deltaTime);
        }
        else if (wallRight)
        {
            //tilt = Mathf.Lerp(tilt, cameraTilt, cameraTiltTime * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
            }
            else if (wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); 
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
            }
        }
    }

    void StopWallRun()
    {
        rb.useGravity = true;

        //cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunfovTime * Time.deltaTime);
        //tilt = Mathf.Lerp(tilt, 0, cameraTiltTime * Time.deltaTime);
    }
}
