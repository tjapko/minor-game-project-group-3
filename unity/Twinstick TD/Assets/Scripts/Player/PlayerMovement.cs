using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	// public variables 
	private int m_PlayerNumber = 0; // not used yet; can be used to identify the different players (later), each players needs different controls!
	[HideInInspector]public PlayerManager player;
	[HideInInspector]public Vector3 mouseposition;
	private float m_MovementSpeed = 15f;
	[HideInInspector]public static bool useController;
	[HideInInspector]public static bool windowsAndXBOX; 
	public static bool macAndXBOX;


	private Rigidbody m_playerRigidbody;
	private float m_MovementInputValueV;
	private float m_MovementInputValueH;
//	private Vector3 m_RotationInputM; not used 
	private int Floor;
	private float camRayLength = 100f;
	private Vector3 movement;
	private Vector3 playerDir;
	private Transform cameraTransform;
	private string m_MovementAxisNameV;
	private string m_MovementAxisNameH;
	private string m_RotationAxisNameV;
	private string m_RotationAxisNameH;
	static Animator anim;

	// Initializes the Floormask 
	void Awake()
	{
		anim = GetComponent<Animator> ();
		Floor = LayerMask.GetMask("mouseFloor"); 
	}

	// Initializes the player's Rigidbody
    private void Start()
    {
        m_playerRigidbody = GetComponent<Rigidbody>();
		// input field for movement 
		m_MovementAxisNameV = "Vertical_" + (m_PlayerNumber+1); 
		m_MovementAxisNameH = "Horizontal_" + (m_PlayerNumber+1);
		// input field for rotation, mac 
		if (!windowsAndXBOX) {
			m_RotationAxisNameH = "RightJoystickHorizontalMac_" + (m_PlayerNumber+1);
			m_RotationAxisNameV = "RightJoystickVerticalMac_" + (m_PlayerNumber+1);
		}
		// input field for rotation, for using XBOX controller on windows 
		else {
			m_RotationAxisNameH = "RightJoystickHorizontalWindowsXBOX_" + (m_PlayerNumber+1);
			m_RotationAxisNameV = "RightJoystickVerticalWindowsXBOX_" + (m_PlayerNumber+1);
		}

    }

	//Every physics step: Abstracting Vertical,Horizontal and mousePosition input and Updating player's position and rotation
	private void FixedUpdate()
	{
        // Store the player's input.
        m_MovementInputValueV = Input.GetAxisRaw(m_MovementAxisNameV); // use Input.GetAxisRaw("Vertical") for instant reaction of the Vertical movement 
		m_MovementInputValueH = Input.GetAxisRaw(m_MovementAxisNameH); // use Input.GetAxisRaw("Horizontal") for instant reaction of the Horizontal movement 
		//m_RotationInputM = Input.mousePosition;

		Move(); // Move the player (both keyboard and controller)

		if (GameManager.getWavephase ()) {
			if (!useController) {
				mouseTurn (); // Rotate the player with mouse
			} else {
				controllerTurn (); // Rotate the player with controller for both controllers (XBOX and PS3) on mac and PS3 on windows
			} 
		}
	}	

	// Adjust the position of the player based on the player's keyboard input.
	private void Move()
	{
		// Horizontal movement (x-axis)
		float movementX = m_MovementInputValueH;
		// Vertical movement (z-axis)
		float movementZ = m_MovementInputValueV;

		// movement vector, no movement in the y-axis (because it's fixed)
		movement = new Vector3(movementX, 0f, movementZ);

		// normalizing movement in order to get the speed of all directions the same 
		movement = movement.normalized;

		// adjusting the movement speed
		movement = movement *  m_MovementSpeed * Time.deltaTime;

		// Rotates (Vector3) movement 
		movement = RotateWithView (movement);

		// adding movement to player's position
		movement += m_playerRigidbody.position;

		// move player to the new (moved) position
		m_playerRigidbody.MovePosition(movement);

	}
		
	// Adjust the rotation of the player based on the mousePosition input.
	private void mouseTurn()
	{
		// creating a ray from the camera to the mouseposition
		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition); 

		// a variable, which is true when the ray hits the floor 
		RaycastHit floorHit;
        
        if (Physics.Raycast(camRay , out floorHit, camRayLength , Floor)) // checking if the ray hits the floor 
		{
            Vector3 playerToMouse = floorHit.point - transform.position; // creating a vector from the player to the mousePosition
            playerToMouse.y = 0f; // because it's a projection on the x-z-plane
            mouseposition = transform.position + playerToMouse;  //Used for dropping objects

            // Creating newRotation, towards the new mouse-direction (x-z-plane)
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
			// Rotate the player to the newRotation
			m_playerRigidbody.MoveRotation(newRotation);

			//Line from Main Camera to the point selected with the mouse (for debugging purposes)
			Debug.DrawLine (Camera.main.transform.position, floorHit.point, Color.yellow ); 
		}
	}

	// Adjust the rotation of the player based on the controller's right-joystick input
	private void controllerTurn() {
		// horizontal & vertical rotation, used GetAxis() istead of GetAxisRaw()
		playerDir = Vector3.right * Input.GetAxis (m_RotationAxisNameH) + 
					Vector3.forward * -1 * Input.GetAxis (m_RotationAxisNameV);  

		// check if player's input isn't zero, sp player is actually rotating 
		if (playerDir.sqrMagnitude > 0.0f) { 
			// Extra rotation to fix relative rotation axis
			playerDir = RotateWithView(playerDir);
			// Rotate the player to the newRotation
			Quaternion newRotation = Quaternion.LookRotation(playerDir);
			m_playerRigidbody.MoveRotation(newRotation);
		}
	}
		
	// Rotates a Vector3 with respect to the camera view, in order to obtain a relative vector (independent from the camera view)
	private Vector3 RotateWithView(Vector3 originalVector) 
	{
		// camera is moved (not fixed)
		if (cameraTransform != null) 
		{
			// calculate the new rotated, right-oriented vector 
			Vector3 dir = cameraTransform.TransformDirection (originalVector);
			dir.y = 0f; // y-value is keeped zero
			return dir.normalized * originalVector.magnitude; // same length as before ()
		}
		// camera is not moved (fixed)
		else 
		{
			cameraTransform = Camera.main.transform; 
			return originalVector; // just return the orignal vector (already right orientation)
		}

	}

}
