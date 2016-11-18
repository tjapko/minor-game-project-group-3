using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;

    private string m_MovementAxisName;
    private string m_TurnAxisName;
    private Rigidbody m_Rigidbody;
    private float m_MovementInputValue;
    private float m_TurnInputValue;
    private float m_OriginalPitch;


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>(); //get rigid body
    }

    private void Start()
    {

    }


    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        m_MovementInputValue = Input.GetAxis("Vertical");
        m_TurnInputValue = Input.GetAxis("Horizontal");
    }

    //Every physics step
    private void FixedUpdate()
    {
        // Move and turn the tank.
        Move();
        Turn();

    }


    private void Move()
    {
        // Adjust the position of the tank based on the player's input.
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime; //Time.deltatime proportional to second (not per physics step)
        movement += m_Rigidbody.position;
        movement[1] = 0f;
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
