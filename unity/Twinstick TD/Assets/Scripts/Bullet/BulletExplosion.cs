using UnityEngine;

public class BulletExplosion : MonoBehaviour
{
    public LayerMask m_EnemyMask;                       // Used to filter what the explosion affects, this should be set to "Enemies".
    public float m_MaxDamage = 100f;                    // The amount of damage done if the explosion is centred on a tank.
    public float m_MaxLifeTime = 2f;                    // The time in seconds before the shell is removed.
    
    private void Start()
    {
        // If it isn't destroyed by then, destroy the shell after it's lifetime.
        Destroy(gameObject, m_MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        // Collect all the colliders in a sphere from the shell's current position to a radius of the explosion radius.
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_EnemyMask);

        // Go through all the colliders...
        for (int i = 0; i < colliders.Length; i++)
        {
            // ... and find their rigidbody.
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

            // If they don't have a rigidbody, go on to the next collider.
            if (!targetRigidbody)
                continue;

            // Find the EnemyHealth script associated with the rigidbody.
            EnemyHealth targetHealth = targetRigidbody.GetComponent<EnemyHealth>();

            // If there is no EnemyHealth script attached to the gameobject, go on to the next collider.
            if (!targetHealth)
                continue;

            // Calculate the amount of damage the target should take based on it's distance from the shell.
            float damage = m_MaxDamage;

            // Deal this damage to the tank.
            targetHealth.TakeDamage(damage);
        }

              
        // Destroy the shell.
        Destroy(gameObject);
    }


   
}