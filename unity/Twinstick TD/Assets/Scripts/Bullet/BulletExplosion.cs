using UnityEngine;

public class BulletExplosion : MonoBehaviour
{
    public LayerMask m_EnemyMask;   // Used to filter what the explosion affects, this should be set to "Enemies".
    public float m_MaxDamage = 1f;       // The amount of damage done if the explosion is centred on an enemy.
    public float m_MaxLifeTime;     // The time in seconds before the shell is removed.

    private int m_playernumber;
    
    private void Start()
    {
        // If it isn't destroyed by then, destroy the shell after it's lifetime.
        Destroy(gameObject, m_MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyHealth>().setLastHit(m_playernumber);
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(m_MaxDamage);

		}
		if (!other.gameObject.CompareTag("River") && !other.gameObject.CompareTag("Shop")) {
			// Destroy the shell.
			Destroy(gameObject);
		}
              

    }

    //Set player number
    public void setPlayernumber(int playernumber)
    {
        m_playernumber = playernumber;
    }

    //Get player number
    public int getPlayernumber()
    {
        return m_playernumber;
    }

}