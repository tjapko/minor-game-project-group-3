using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;              // Used to identify the different players.
    public Rigidbody m_Bullet;                  // Prefab of the shell.
    public Transform m_FireTransform;           // A child of the player where the shells are spawned.
    public float m_LaunchForce = 15f;           // The force given to the shell if the fire button is not held.
    


    private string m_FireButton;                // The input axis that is used for launching shells.
    private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.
    private bool m_Fired;                       // Whether or not the shell has been launched with this button press.


    private void OnEnable()
    {
        // When the tank is turned on, reset the launch force and the UI
        m_CurrentLaunchForce = m_LaunchForce;
        
    }


    private void Start()
    {
        // The fire axis is based on the player number.
        m_FireButton = "Fire" + m_PlayerNumber;

    }


    private void Update()
    {
       
        if (Input.GetButtonUp(m_FireButton) && !m_Fired)
        {
            // ... launch the shell.
            Fire();
        }
    }


    private void Fire()
    {
        // Set the fired flag so only Fire is only called once.
        m_Fired = true;

        // Create an instance of the shell and store a reference to it's rigidbody.
        Rigidbody shellInstance =
            Instantiate(m_Bullet, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        // Set the shell's velocity to the launch force in the fire position's forward direction.
        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward; ;

               
    }
}