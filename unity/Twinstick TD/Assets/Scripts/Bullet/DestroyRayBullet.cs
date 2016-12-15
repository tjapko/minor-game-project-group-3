using UnityEngine;
using System.Collections;

public class DestroyRayBullet : MonoBehaviour
{
    public float m_MaxLifeTime;     // The time in seconds before the shell is removed.
    public Weapon weapon;
    public BulletFire bulletFire;

    void Start()
    {
        bulletFire.GetComponent<BulletFire>();
        Destroy(gameObject, m_MaxLifeTime);
    }


    void FixedUpdate()
    {

       /* if (bulletFire.RayHit.transform.position.GetType() == typeof(Vector3))
        {
            Debug.Log(bulletFire.RayHit.transform.position.GetType());
            if (Vector3.Distance(bulletFire.RayHit.transform.position, transform.position) <= 0.2f)
            {
                Destroy(gameObject);
            }
        }*/
    }

    /// <summary>
    /// Destroying the shell when it touches another collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    { 
        // Destroy the shell.
        Destroy(gameObject);
    }
}
