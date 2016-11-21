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


    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>(); //get rigid body
    }

    private void Update()
    {
        // Store the player's input.
        m_MovementInputValueV = Input.GetAxis("Vertical");
		m_MovementInputValueH = Input.GetAxis("Horizontal");
    }

    //Every physics step
    private void FixedUpdate()
    {
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
        // Adjust the rotation of the tank based on the player's input.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f); //Needed for turning (x,y,z)
        turnRotation = turnRotation * m_Rigidbody.rotation;
        turnRotation.x = 0f;
        turnRotation.z = 0f;
        m_Rigidbody.MoveRotation(turnRotation);
    }
}
