using UnityEngine;
using UnityEngine.UI;

public class BulletFire : MonoBehaviour
{
    public int m_PlayerNumber;              // Used to identify the different players.
    public Rigidbody m_Bullet;                  // Prefab of the shell.
    public Transform m_FireTransform;           // A child of the player where the shells are spawned.
    public float m_LaunchForce;           // The force given to the shell if the fire button is not held.
    
    private string m_FireButton;                // The input axis that is used for launching shells.


    

    private void Start()
    {
        // The fire axis is based on the player number.
        m_FireButton = "Fire" + m_PlayerNumber;

    }


    private void Update()
    {
       
        if (Input.GetButtonUp(m_FireButton))
        {
            // ... launch the shell.
            Fire();
        }
    }


    private void Fire()
    {

        // Create an instance of the shell and store a reference to it's rigidbody.
        Rigidbody shellInstance =
            Instantiate(m_Bullet, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        // Set the shell's velocity to the launch force in the fire position's forward direction.
        shellInstance.velocity = m_LaunchForce * m_FireTransform.forward; ;
        

    }
}