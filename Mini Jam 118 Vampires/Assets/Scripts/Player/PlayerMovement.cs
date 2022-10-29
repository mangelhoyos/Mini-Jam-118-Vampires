using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement setup")]
    [SerializeField]
    private CharacterController controller;
	[SerializeField]
    private float speed = 12f;
    [SerializeField]
    private float gravity = -9.81f;
    [SerializeField]
    private float jumpHeight = 3.5f;
	
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundDistance = 0.4f;
    [SerializeField]
    private LayerMask groundMask;

    [Header("Head tilt")]
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private float maxTiltAngle;
    [SerializeField]
    private float tiltSmoothSpeed;
	

	Vector3 velocity;
	bool isGrounded;
	
    // Update is called once per frame
    void Update()
    {
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
		
		if(isGrounded && velocity.y < 0)
		{
			velocity.y  = -2f;
		} 
		
       float x = Input.GetAxis("Horizontal");
	   float z = Input.GetAxis("Vertical");
	   
	   Vector3 move = transform.right * x + transform.forward * z; 
	   
	   controller.Move(move * speed * Time.deltaTime);
	   
	   if(Input.GetButtonDown("Jump") && isGrounded)
	   {
		   velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
	   }
	   
		velocity.y += gravity * Time.deltaTime;
		
		controller.Move(velocity * Time.deltaTime);
		
    }
}
