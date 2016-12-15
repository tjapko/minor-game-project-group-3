using UnityEngine;

public class BulletExplosion : MonoBehaviour
{
    public LayerMask m_EnemyMask;   // Used to filter what the explosion affects, this should be set to "Enemies".
    public float m_MaxDamage;       // The amount of damage done if the explosion is centred on an enemy.
    public float m_MaxLifeTime;     // The time in seconds before the shell is removed.
    
    private void Start()
    {
        // If it isn't destroyed by then, destroy the shell after it's lifetime.
        Destroy(gameObject, m_MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(m_MaxDamage);
        }
              
        // Destroy the shell.
        Destroy(gameObject);
    }


   
}