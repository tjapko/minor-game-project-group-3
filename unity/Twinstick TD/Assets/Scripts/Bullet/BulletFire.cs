using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BulletFire : MonoBehaviour
{


    public int maxBulletDistance;
    public RaycastHit RayHit;
    public int m_PlayerNumber;      // Used to identify the different players.
    public Rigidbody m_Bullet;      // Prefab of the shell.
    public Rigidbody m_RayBullet;   // Prefab of the Rayshell.
    public Transform m_FireTransform;   // A child of the player where the shells are spawned.
    public LayerMask enemyLayer;
    Weapon currentWeapon;
    private string m_FireButton;                // The input axis that is used for launching shells.
    PlayerInventory playerinventory;
    private string reloadButton;

    //Private variables
    private bool weapon_reloading;      //Boolean if weapons is reloading



    private void Start()
    {


        // The fire axis is based on the player number.
        enemyLayer = LayerMask.GetMask("enemy");
        m_FireButton = "Fire" + m_PlayerNumber;
        playerinventory = GetComponent<PlayerInventory>();
        reloadButton = "r"; // Reload Button   ////// NEED FIX NOT YET IMPLEMENTED 

        weapon_reloading = false; //Set weapon reloading to false
    }


    private void Update()
    {

        currentWeapon = playerinventory.inventory[0];
       
        if (Input.GetButtonUp(m_FireButton))
        {
            Fire();
        }
        if (Input.GetKeyDown(reloadButton))
        {
            StartCoroutine(Reload());
        }

    }

    public void Fire()
    {
        if(currentWeapon.ammoInClip== 0 && currentWeapon.ammo !=0)
        {
            StartCoroutine(Reload());
        }
        else if (currentWeapon.itemtype == Weapon.ItemType.HandGun)
        {
            FireHandGun();
        }
        else if (currentWeapon.itemtype == Weapon.ItemType.Shotgun)
        {
            FireSG();
        }
        else
        {
            FireRay();
        }
    }

    public IEnumerator Reload()
    {
        if(!weapon_reloading)
        {
            if (currentWeapon.ammo > currentWeapon.clipSize)
            {
                currentWeapon.ammo -= (currentWeapon.clipSize - currentWeapon.ammoInClip);

                currentWeapon.ammoInClip = currentWeapon.clipSize;
            }
            else
            {
                currentWeapon.ammoInClip = currentWeapon.ammo;
                currentWeapon.ammo = 0;
            }

            weapon_reloading = true;
            yield return new WaitForSeconds(currentWeapon.reloadTime);
            weapon_reloading = false;
        }
    }


    public void FireRay()
    {
        if (currentWeapon.hasAmmo() && !weapon_reloading)
        {
            Rigidbody shellInstance =
                 GameObject.Instantiate(m_RayBullet, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

            // Set the shell's velocity to the launch force in the fire position's forward direction.
            shellInstance.velocity = currentWeapon.launchForce * m_FireTransform.forward;

            currentWeapon.ammoInClip--;

            Debug.DrawRay(m_FireTransform.position, transform.forward * 20, Color.green, 5f);
            if (Physics.Raycast(m_FireTransform.position, transform.forward, out RayHit, maxBulletDistance))
            {

                float damage = currentWeapon.maxDamage;
                RayHit.transform.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            }
        }
        

    }


    public void FireHandGun()
    {
        if (currentWeapon.hasAmmo() && !weapon_reloading)
        {
            // Create an instance of the shell and store a reference to it's rigidbody.
            Rigidbody shellInstance =
                GameObject.Instantiate(m_Bullet, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
            shellInstance.transform.Rotate(0f, 90f, 0);
            
            // Set the shell's velocity to the launch force in the fire position's forward direction.
            shellInstance.velocity = currentWeapon.launchForce * m_FireTransform.forward;

            currentWeapon.ammoInClip--;
        }
    }

    public void FireSG()
    {

        if (currentWeapon.hasAmmo() && !weapon_reloading)
        {
            Rigidbody shellInstance1 =
               GameObject.Instantiate(m_Bullet, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
            Rigidbody shellInstance2 =
               GameObject.Instantiate(m_Bullet, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
            Rigidbody shellInstance3 =
               GameObject.Instantiate(m_Bullet, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

            shellInstance2.transform.Rotate(0, 90f, 0);
            shellInstance3.transform.Rotate(0, -90f, 0);


            // Set the shell's velocity to the launch force in the fire position's forward direction.
            shellInstance1.velocity = currentWeapon.launchForce * m_FireTransform.forward;
            shellInstance2.velocity = currentWeapon.launchForce * m_FireTransform.forward;
            shellInstance3.velocity = currentWeapon.launchForce * m_FireTransform.forward;

            currentWeapon.ammoInClip--;
        }
    }

}