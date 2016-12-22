using System.Collections;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// The script that contains firing your weapon.
/// </summary>
public class BulletFire : MonoBehaviour
{
    public AudioSource gunSource;
    public AudioClip reloadingSound;
    public AudioClip emptyGunSound;
    public float maxBulletDistance;         // maximum bulletdistance (applied in Raycast shooting, the length of the ray)
    public RaycastHit RayHit;               // contains the information of the Raycast Hit
	[HideInInspector] public int m_PlayerNumber;              // Used to identify the different players.
//	private int m_PlayerNumber = 0;              // Used to identify the different players.
    public Rigidbody m_Bullet;              // Prefab of the shell.
    public Rigidbody m_RayBullet;           // Prefab of the Rayshell.
    public Transform m_FireTransform;       // A child of the player where the shells are spawned.
    public Transform m_FireTransformSG;     // second transform for the Shotgun (only difference is the rotation)
    public Transform m_FireTransformSG2;    // third transform for the Shotgun (only difference is the rotation) 
    Weapon currentWeapon;                   // the current weapon the player is holding
    private string m_FireButton;            // The input axis that is used for launching shells.
    PlayerInventory playerinventory;        // The playerinventory of this player
    private string reloadButton;            // The reloadButton set on the 'r' button
    private bool weapon_reloading;          //Boolean if weapons is reloading
	private float m_timer = 0.0f;

    /// <summary>
    ///  initiating the fire and reload button and retrieving the related inventory 
    /// </summary>
    private void Start()
    {       

		m_FireButton = "Fire1_" + ((m_PlayerNumber+1));
        playerinventory = GetComponent<PlayerInventory>();

        reloadButton = "r"; // Reload Button   ////// NEED FIX NOT YET IMPLEMENTED 

        weapon_reloading = false; //Set weapon reloading to false

    }


    /// <summary>
    ///  checking every updateFrame if the fire or reload button is pressed  
    /// </summary>
    private void Update()
    {

        currentWeapon = playerinventory.inventory[0];
        if (currentWeapon.itemtype == Weapon.ItemType.MachineGun)
        {
            if (Input.GetButton(m_FireButton))
            {
                Fire();
            }

            if (Input.GetKeyUp(reloadButton))

            {
                StartCoroutine(Reload());
            }
        }
        else
        {
            if (Input.GetButtonDown(m_FireButton))
            {
                Fire();
            }

            if (Input.GetKeyUp(reloadButton))

            {
                StartCoroutine(Reload());
            }
        }
		m_timer += Time.deltaTime;  // the m_timer is updated 
    }


    /// <summary>
    /// invokes the right firemethod for the currentweapon when this method is called.
    /// More weapons must be implemented manually 
    /// </summary>
    public void Fire()
    {
		
		if (m_timer >= 1.0f / currentWeapon.fireRate) { 
			
			if (currentWeapon.itemtype == Weapon.ItemType.HandGun || currentWeapon.itemtype == Weapon.ItemType.MachineGun) {
				FireHandGun ();
				
			} else if (currentWeapon.itemtype == Weapon.ItemType.Shotgun) {
				FireSG ();
			} else {
				FireRay ();
			}
			if (currentWeapon.ammoInClip == 0 && currentWeapon.ammo != 0) {
				StartCoroutine (Reload ());
			}
			m_timer = 0.0f;
		}
    }



    /// <summary>
    /// Reloading the weapon.
    /// </summary>
    
    public IEnumerator Reload()
    {
        if(!weapon_reloading)
        {
            Weapon reload_weapon = currentWeapon;
            weapon_reloading = true;
            yield return new WaitForSeconds(currentWeapon.reloadTime);

            if (reload_weapon.equals(currentWeapon))
            {
                if ((currentWeapon.ammo + currentWeapon.ammoInClip) >= currentWeapon.clipSize)
                {
                    currentWeapon.ammo -= (currentWeapon.clipSize - currentWeapon.ammoInClip);

                    currentWeapon.ammoInClip = currentWeapon.clipSize;
                }
                else
                {
                    currentWeapon.ammoInClip += currentWeapon.ammo;
                    currentWeapon.ammo = 0;

                }
                gunSource.clip = reloadingSound;
                gunSource.Play();
            }
            
            weapon_reloading = false;
        }
    }


    /// <summary>
    /// Fire the weapon from the raycast principle.
    /// cast a ray. When it hits an enemy , deal damage. 
    /// </summary>
    public void FireRay()
    {

        if (currentWeapon.hasAmmo()) {
            Rigidbody shellInstance =
                     GameObject.Instantiate(m_RayBullet, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

            
           
           // shellInstance.velocity = currentWeapon.launchForce * m_FireTransform.forward;
            DestroyRayBullet bullet = shellInstance.GetComponent<DestroyRayBullet>();
            bullet.m_MaxLifeTime = currentWeapon.bulletLifeTime;

            currentWeapon.ammoInClip--;

           
            if (Physics.Raycast(m_FireTransform.position, transform.forward, out RayHit, maxBulletDistance))
            {

                
                bullet.hit = true;


                bullet.timeToTarget = RayHit.distance / currentWeapon.launchForce;
                bullet.endPos = RayHit.point;
               
                float damage = currentWeapon.maxDamage;
                if (RayHit.collider.CompareTag("Enemy"))
                {
                    GameObject enemy = RayHit.transform.gameObject;
                    if (enemy.activeSelf)
                    {
                        EnemyHealth healthscript = enemy.GetComponent<EnemyHealth>();
                        if (healthscript != null)
                        {
							healthscript.setLastHit((m_PlayerNumber));
                            RayHit.transform.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
                        }
                    }

                }
            }
            else
            { 
                // Set the shell's velocity to the launch force in the fire position's forward direction.
                shellInstance.velocity = currentWeapon.launchForce * m_FireTransform.forward;
            }

        }
        else if (!currentWeapon.hasAmmo() && !weapon_reloading)
        {
            gunSource.clip = emptyGunSound;
            gunSource.Play();
        }
    }


    /// <summary>
    /// Firing the weapon from the Handgun principle (default).
    /// Launching a single bullet, when it hits an enemy the bullet deals damage.
    /// </summary>
    public void FireHandGun()
    {
        if (currentWeapon.hasAmmo() && !weapon_reloading)
        {
            // Create an instance of the shell and store a reference to it's rigidbody.
            Rigidbody shellInstance =
                GameObject.Instantiate(m_Bullet, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;


            BulletExplosion bullet = shellInstance.gameObject.GetComponent<BulletExplosion>();
            bullet.setPlayernumber((m_PlayerNumber));

            bullet.m_MaxDamage = currentWeapon.maxDamage;
            bullet.m_MaxLifeTime = currentWeapon.bulletLifeTime;



            // Set the shell's velocity to the launch force in the fire position's forward direction.
            shellInstance.velocity = currentWeapon.launchForce * m_FireTransform.forward;

            currentWeapon.ammoInClip--;
        }
        else if (!currentWeapon.hasAmmo() && !weapon_reloading)
        {
            gunSource.clip = emptyGunSound;
            gunSource.Play();
        }

    }


    /// <summary>
    /// Firing the weapon from the Shotgun principle.
    /// launching 3 bullets with each a different rotation. 
    /// </summary>
    public void FireSG()
    {

        if (currentWeapon.hasAmmo() && !weapon_reloading)
        {
                  
                    
            Rigidbody shellInstance1 =
               GameObject.Instantiate(m_Bullet, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
			shellInstance1.gameObject.GetComponent<BulletExplosion>().setPlayernumber((m_PlayerNumber));

            Rigidbody shellInstance2 =
                GameObject.Instantiate(m_Bullet, m_FireTransformSG.position, m_FireTransformSG.rotation) as Rigidbody;
			shellInstance2.gameObject.GetComponent<BulletExplosion>().setPlayernumber((m_PlayerNumber));

            Rigidbody shellInstance3 =
               GameObject.Instantiate(m_Bullet, m_FireTransformSG2.position, m_FireTransformSG2.rotation) as Rigidbody;
			shellInstance3.gameObject.GetComponent<BulletExplosion>().setPlayernumber((m_PlayerNumber));

            BulletExplosion bullet1 = shellInstance1.GetComponent<BulletExplosion>();
            BulletExplosion bullet2 = shellInstance2.GetComponent<BulletExplosion>();
            BulletExplosion bullet3 = shellInstance3.GetComponent<BulletExplosion>();

            bullet1.m_MaxDamage = currentWeapon.maxDamage;
            bullet1.m_MaxLifeTime = currentWeapon.bulletLifeTime;
            bullet2.m_MaxDamage = currentWeapon.maxDamage;
            bullet2.m_MaxLifeTime = currentWeapon.bulletLifeTime;
            bullet3.m_MaxDamage = currentWeapon.maxDamage;
            bullet3.m_MaxLifeTime = currentWeapon.bulletLifeTime;


            // Set the shell's velocity to the launch force in the fire position's forward direction.
            shellInstance1.velocity = currentWeapon.launchForce * m_FireTransform.forward ;
            shellInstance2.velocity = currentWeapon.launchForce * m_FireTransformSG.forward ;
            shellInstance3.velocity = currentWeapon.launchForce * m_FireTransformSG2.forward ;

            

            currentWeapon.ammoInClip--;
        }
        else if(!currentWeapon.hasAmmo() && !weapon_reloading)
        {
            gunSource.clip = emptyGunSound; 
            gunSource.Play();
        }
    }

}