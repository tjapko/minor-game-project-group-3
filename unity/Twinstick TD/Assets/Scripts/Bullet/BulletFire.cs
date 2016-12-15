using UnityEngine;
using UnityEngine.UI;

public class BulletFire : MonoBehaviour
{
	// public variables 
	[HideInInspector]public int m_PlayerNumber = 1;  // Used to identify the different players. Needs to be updated automatically (in the usermanager or gamemanager) when multiplayed-mode
    public Rigidbody m_Bullet;                  	 // Prefab of the shell.
    public Transform m_FireTransform;          		 // A child of the player where the shells are spawned.
    public float m_LaunchForce;       			     // The force given to the shell if the fire button is not held.
//	public PlayerMovement PlayerMovement; // PlayerMovement script for updating boolean useController 
    // private variables
    private string m_FireButton;              	 	 // The input axis that is used for launching shells.

	// update method 
    private void Update()
    {
		if (!PlayerMovement.windowsAndXBOX && !PlayerMovement.macAndXBOX) {
			// The fire axis is based on the player number (for multiplayer purposes).
			m_FireButton = "Fire1_" + m_PlayerNumber;
		} else if (!PlayerMovement.windowsAndXBOX && PlayerMovement.macAndXBOX) {
			m_FireButton = "Fire1MacXBOX_" + m_PlayerNumber;	
		} else {
			m_FireButton = "Fire1WindowsXBOX_" + m_PlayerNumber;
		}
        if (Input.GetButtonUp(m_FireButton)) {
            // launch the bullet
            Fire();
        }
	}
		
	// fire the bullet
    private void Fire()
    {
        // Create an instance of the shell and store a reference to it's rigidbody.
        Rigidbody shellInstance =
            Instantiate(m_Bullet, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        // Set the shell's velocity to the launch force in the fire position's forward direction.
        shellInstance.velocity = m_LaunchForce * m_FireTransform.forward; ;
    }
}