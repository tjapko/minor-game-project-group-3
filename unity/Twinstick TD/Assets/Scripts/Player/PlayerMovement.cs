using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;

    private string m_MovementAxisName;
    private string m_TurnAxisName;
    private Rigidbody m_Rigidbody;
    private float m_MovementInputValueV;
	private float m_MovementInputValueH;
    private float m_TurnInputValue;
    private float m_OriginalPitch;

	private Vector3 mouseInput;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>(); //get rigid body
    }

    //Every physics step
    private void FixedUpdate()
    {
		// Store the player's input.
		m_MovementInputValueV = Input.GetAxisRaw("Vertical");
		m_MovementInputValueH = Input.GetAxisRaw("Horizontal");

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

		Vector3 movement = new Vector3(movementX, 0f, movementZ);

		movement += m_Rigidbody.position;

		m_Rigidbody.MovePosition(movement);

    }
		
    private void Turn()
	{

		Ray cameraRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		Plane groundPlane = new Plane (Vector3.up, Vector3.zero);
		float rayLength;

		if (groundPlane.Raycast (cameraRay, out rayLength)) 
		{
			Vector3 pointToLook = cameraRay.GetPoint (rayLength);
			Debug.DrawLine (cameraRay.origin, pointToLook, Color.yellow ); 
			transform.LookAt (pointToLook);
		}

	}
}
	