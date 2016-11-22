using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

//	public float m_RotationSpeed = 1f; // not used now!
	public float m_MovementSpeed = 10f;

	private Rigidbody m_playerRigidbody;

	private float m_MovementInputValueV;
	private float m_MovementInputValueH;
	private Vector3 m_RotationInputM;

	private int Floor;
	private float camRayLength = 100f;

	Vector3 movement;
	private Transform cameraTransform;


	// Initializes the Floormask 
	void Awake()
	{
		Floor = LayerMask.GetMask("mouseFloor"); 
	}

	// Initializes the player's Rigidbody
    private void Start()
    {
        m_playerRigidbody = GetComponent<Rigidbody>(); 
    }

	//Every physics step: Abstracting Vertical,Horizontal and mousePosition input and Updating player's position and rotation
	private void FixedUpdate()
	{
		// Store the player's input.
		m_MovementInputValueV = Input.GetAxis("Vertical");   // use Input.GetAxisRaw("Vertical") for instant reaction of the Vertical movement 
		m_MovementInputValueH = Input.GetAxis("Horizontal"); // use Input.GetAxisRaw("Horizontal") for instant reaction of the Horizontal movement 
		m_RotationInputM = Input.mousePosition;

		// Move and turn the player.
		Move();
		Turn();
	}

	// Adjust the position of the player based on the player's keyboard input.
	private void Move()
	{
		// Horizontal movement (x-axis)
		float movementX = m_MovementInputValueH * m_MovementSpeed * Time.deltaTime; //Time.deltatime proportional to second (not per physics step)
		// Vertical movement (z-axis)
		float movementZ = m_MovementInputValueV * m_MovementSpeed * Time.deltaTime;

		// movement vector, no movement in the y-axis (because it's fixed)
		movement = new Vector3(movementX, 0f, movementZ);

		// Rotates (Vector3) movement 
		movement = RotateWithView ();

		// adding movement to player's position
		movement += m_playerRigidbody.position;

		// move player to the new (moved) position
		m_playerRigidbody.MovePosition(movement);

	}

	// Rotate Vector3 movement with respect to the camera view
	private Vector3 RotateWithView() 
	{
		// camera is moved (not fixed)
		if (cameraTransform != null) 
		{
			// calculate the new rotated, right-oriented movement vector 
			Vector3 dir = cameraTransform.TransformDirection (movement);
			dir.y = 0f; // y-value is keeped zero
			return dir.normalized * movement.magnitude; // same length as before ()
		}
		// camera is not moved (fixed)
		else 
		{
			cameraTransform = Camera.main.transform; 
			return movement; // just return movement (already right orientation)
		}

	}

	//		//Chahid's Turn()
	//    private void Turn()
	//	{
	//
	//		Ray cameraRay = Camera.main.ScreenPointToRay (Input.mousePosition);
	//
	//		Plane groundPlane = new Plane (Vector3.up, Vector3.zero);
	//		float rayLength;
	//
	//		if (groundPlane.Raycast (cameraRay, out rayLength)) 
	//		{
	//			Vector3 pointToLook = cameraRay.GetPoint (rayLength);
	//			Debug.DrawLine (Camera.main.transform.position, pointToLook, Color.yellow ); 
	//			transform.LookAt (pointToLook);
	//		}
	//
	//	}

	// Adjust the rotation of the player based on the mousePosition input.
	void Turn()
	{
		// creating a ray from the camera to the mouseposition
		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		// a variable, which is true when the ray hits the floor 
		RaycastHit floorHit; 

		if(Physics.Raycast(camRay , out floorHit, camRayLength , Floor)) // checking if the ray hits the floor 
		{
			Vector3 playerToMouse = floorHit.point - transform.position; // creating a vector from the player to the mousePosition
			playerToMouse.y = 0f; // because it's a projection on the x-z-plane

			// Creating newRotation, towards the new mouse-direction (x-z-plane)
			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
			// Rotate the player to the newRotation
			m_playerRigidbody.MoveRotation(newRotation);

			//Line from Main Camera to the point selected with the mouse (for debugging purposes)
			Debug.DrawLine (Camera.main.transform.position, floorHit.point, Color.yellow ); 
		}
	}

}
