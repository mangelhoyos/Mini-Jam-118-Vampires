using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    public Transform playerBody;
    float xRotation = 0f;
    float yRotation = 0f;
    
    public float tiltAmount = 5;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        CameraRotateForPlayerMovement();

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void CameraRotateForPlayerMovement()
    {
        float rotZ = 0;
        rotZ += -Input.GetAxis("Horizontal") * tiltAmount * Time.deltaTime;
 
        Quaternion finalRot = Quaternion.Euler(xRotation, transform.localRotation.eulerAngles.y, rotZ);
        transform.localRotation = finalRot;
    }
}
