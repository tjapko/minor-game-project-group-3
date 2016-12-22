using UnityEngine;
using System.Collections;
/// <summary>
/// The script on the Raybullet, this bullet is only initiated when Rayshooting has occured
/// </summary>
public class DestroyRayBullet : MonoBehaviour
{
    [HideInInspector]public float m_MaxLifeTime;     // The time in seconds before the shell is removed.
    public Vector3 endPos;          // The endposition of this bullet. Where the Ray hitted a collider
    public float timeToTarget;      // The time it takes to travel from the beginposition to the endposition (distance / launchforce)
    public Vector3 StartPos;        // the position where the bullet is initiated (m_FireTransform)
    public bool hit = false;        // is true when the ray hits a target

    private float t;                // a float to store the time data each updateframe
    internal object m_MaxDamage;

    /// <summary>
    /// On start, defining the startposition and set the maxLifetime of this bullet
    /// </summary>
    void Start()
    {

        StartPos = transform.position;
        Destroy(gameObject, m_MaxLifeTime);

    }

    /// <summary>
    /// Traveling from start position to the endposition. the bullet selfdestroys when reached the endposition 
    /// </summary>
    void FixedUpdate()
    {
        if (hit)
        {
            t += Time.deltaTime / timeToTarget;
            transform.position = Vector3.Lerp(StartPos, endPos, t);

            if (transform.position == endPos)
            {
                Destroy(gameObject);
            }
        }
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
