using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	[HideInInspector]public int m_PlayerNumber = 1; // not used yet; can be used to identify the different players (later), each players needs different controls!
	[HideInInspector]public Vector3 mouseposition;
    //public float m_RotationSpeed = 1f; // not used!
	public float m_MovementSpeed = 15f;
	[HideInInspector]public bool useController;
	public bool windowsAndXBOX=false; // mac is default 

	private Rigidbody m_playerRigidbody;

	private float m_MovementInputValueV;
	private float m_MovementInputValueH;
	//private Vector3 m_RotationInputM;

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
		
		string m_MovementAxisNameV = "Vertical"; //		string m_MovementAxisNameV = "Vertical" + m_PlayerNumber; // can be used later
        string m_MovementAxisNameH = "Horizontal";//		string m_MovementAxisNameH = "Horizontal" + m_PlayerNumber; // can be used later

        // Store the player's input.
        m_MovementInputValueV = Input.GetAxisRaw(m_MovementAxisNameV);   // use Input.GetAxisRaw("Vertical") for instant reaction of the Vertical movement 
		m_MovementInputValueH = Input.GetAxisRaw(m_MovementAxisNameH); // use Input.GetAxisRaw("Horizontal") for instant reaction of the Horizontal movement 
		//m_RotationInputM = Input.mousePosition;

		Move(); // Move the player (both keyboard and controller)

		if (!useController) {
			mouseTurn (); // Rotate the player with mouse
		} 
		else if (useController && !windowsAndXBOX) {
			controllerTurnMac (); // Rotate the player with controller
		} 
		else if (useController && windowsAndXBOX) {
			controllerTurnWindowsAndXBOX (); // Rotate the player with controller
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

	// Adjust the rotation of the player based on the controller's right-joystick input for the PS3-controller (windows and mac) and XBOX-controller (mac)
	private void controllerTurnMac() {
		// horizontal & vertical movement, used GetAxis() istead of GetAxisRaw()
		Vector3 playerDir = Vector3.right * Input.GetAxis ("RightJoystickHorizontalMac") + 
							Vector3.forward * -1 * Input.GetAxis ("RightJoystickVerticalMac");  

		// check if player's input isn't zero, sp player is actually rotating 
		if (playerDir.sqrMagnitude > 0.0f) { 
			transform.rotation = Quaternion.LookRotation(playerDir, Vector3.up);
		}
	}

	// Adjust the rotation of the player based on the controller's right-joystick input for the XBOX-controller (windows) 
	private void controllerTurnWindowsAndXBOX() {
		// horizontal & vertical movement, used GetAxis() istead of GetAxisRaw()
		Vector3 playerDir = Vector3.right * Input.GetAxis ("RightJoystickHorizontalWindowsXBOX") + 
							Vector3.forward * -1 * Input.GetAxis ("RightJoystickVerticalWindowsXBOX");  

		// check if player's input isn't zero, sp player is actually rotating 
		if (playerDir.sqrMagnitude > 0.0f) { 
			transform.rotation = Quaternion.LookRotation(playerDir, Vector3.up);
		}
	}

}
