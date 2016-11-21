using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	//	public float m_RotationSpeed = 1f; // not used now!
	public float m_Speed = 10f;

	private Rigidbody m_playerRigidbody;

	private float m_MovementInputValueV;
	private float m_MovementInputValueH;
	private Vector3 m_RotationInputM;

	private int FloorMask;
	private float camRayLength = 100f;


	// Initializes the Floormask 
	void Awake()
	{
		FloorMask = LayerMask.GetMask("mouseFloor"); 
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

	// Adjust the position of the player based on the player's input.
	private void Move()
	{
		// Horizontal movement (x-axis)
		float movementX = m_MovementInputValueH * m_Speed * Time.deltaTime; //Time.deltatime proportional to second (not per physics step)
		// Vertical movement (z-axis)
		float movementZ = m_MovementInputValueV * m_Speed * Time.deltaTime;

		// movement vector, no movement in the y-axis (because it's fixed)
		Vector3 movement = new Vector3(movementX, 0f, movementZ);

		// adding movement to player's position
		movement += m_playerRigidbody.position;

		// move player to the new (moved) position
		m_playerRigidbody.MovePosition(movement);

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

		if(Physics.Raycast(camRay , out floorHit, camRayLength , FloorMask)) // checking if the ray hits the floor 
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
