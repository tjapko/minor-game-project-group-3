using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Shopscript
/// Contains the basic information of weapons and ammo that are sold in the shop
/// Allows the user to 
/// </summary>
public class ShopScript : MonoBehaviour {
    //Temp variables 
    string use_button = "f";    //Set to use button of player[i]

    //Public variables
    public int maxweapons = 3;
    public int maxammotype = 3;
    public List<Weapon> weaponsforsale;
    public List<Weapon> ammoforsale;
    public Rigidbody m_Bullet;  // Prefab of the shell.
    public Rigidbody m_RayBullet;  // Prefab of the Rayshell.
    public Transform m_FireTransform;           // A child of the player where the shells are spawned.

    //Reference
    public GameObject m_ShopUIprefab;
    public GameObject m_instance_UI;

    //Private variables
    private List<GameObject> players_present;
    private bool showUI;
	private Weapon weapon1, weapon2, weapon3, weapon4;

	// Use this for initialization
	void Start () {
        //Instantiate Lists
        players_present = new List<GameObject>();
        weaponsforsale = new List<Weapon>();
        ammoforsale = new List<Weapon>();
        

        //Create empty weaponsforsale
        while (weaponsforsale.Count < maxweapons)
        {
            weaponsforsale.Add(new Weapon());
        }

        //Create empty ammoforsale
        while(ammoforsale.Count < maxammotype)
        {
            ammoforsale.Add(new global::Weapon());
        }

        //Instantiate UI
        //m_instance_UI = GameObject.Instantiate(m_ShopUIprefab);
        //m_instance_UI.GetComponent<ShopUIScript>().m_shopscript = gameObject.GetComponent<ShopScript>();
        //m_instance_UI.SetActive(false);

        //Set UI active
        showUI = false;

//        For testing
//      ............. Weapon(name               , id , description       , iconname  , price            , itemtype                   , fireratef           , launchforcef           , maxDamagef           , reloadTimef           , clipsize            , ammo            , ammopriceperclip     , ammoInClip            , maxAmmo            , lifetime)
		weapon1 = new Weapon("Default Weapon"   , 1  , "Default weapon!" , "Weapon1" , HandGun.price    , Weapon.ItemType.HandGun    , HandGun.fireRate    , HandGun.launchForce    , HandGun.maxDamage    , HandGun.reloadTime    , HandGun.clipSize1    , HandGun.ammo1    , HandGun.ammoprice    , HandGun.ammoInClip1    , HandGun.maxAmmo    , HandGun.bulletLifeTime);
		weapon4 = new Weapon("Default Weapon 4" , 4  , "Default weapon!" , "sniper"  , Sniper.price     , Weapon.ItemType.Sniper     , Sniper.fireRate     , Sniper.launchForce     , Sniper.maxDamage     , Sniper.reloadTime     , Sniper.clipSize1    , Sniper.ammo1     , Sniper.ammoprice     , Sniper.ammoInClip1     , Sniper.maxAmmo     , Sniper.bulletLifeTime);
		weapon2 = new Weapon("Default Weapon 2" , 2  , "Default weapon!" , "shotgun" , ShotGun.price    , Weapon.ItemType.Shotgun    , ShotGun.fireRate    , ShotGun.launchForce    , ShotGun.maxDamage    , ShotGun.reloadTime    , ShotGun.clipSize1    , ShotGun.ammo1    , ShotGun.ammoprice    , ShotGun.ammoInClip1    , ShotGun.maxAmmo    , ShotGun.bulletLifeTime);
		weapon3 = new Weapon("Default Weapon 3" , 3  , "Default weapon!" , "Weapon3" , MachineGun.price , Weapon.ItemType.MachineGun , MachineGun.fireRate , MachineGun.launchForce , MachineGun.maxDamage , MachineGun.reloadTime , MachineGun.clipSize1 , MachineGun.ammo1 , MachineGun.ammoprice , MachineGun.ammoInClip1 , MachineGun.maxAmmo , MachineGun.bulletLifeTime);
		addWeapon((Weapon)weapon1);
		addWeapon((Weapon)weapon4);
		addWeapon((Weapon)weapon2);
		addWeapon((Weapon)weapon3);
//		Upgrade (2); // for testing purposes
    }

	//Upgradable: ammo, ammoInClip
	public void Upgrade(int UpgradeLevel) {
		switch (UpgradeLevel) {
			case 2: 
				setAllWeaponAmmo(HandGun.ammo2, ShotGun.ammo2, MachineGun.ammo2, Sniper.ammo2);
				setAllWeaponAmmoInClip(HandGun.ammoInClip2, ShotGun.ammoInClip2, MachineGun.ammoInClip2, Sniper.ammoInClip2);
                setAllWeaponClipSize(HandGun.clipSize2, ShotGun.clipSize2, MachineGun.clipSize2, Sniper.clipSize2);
				break;
			case 3:
				setAllWeaponAmmo(HandGun.ammo3, ShotGun.ammo3, MachineGun.ammo3, Sniper.ammo3);
				setAllWeaponAmmoInClip(HandGun.ammoInClip3, ShotGun.ammoInClip3, MachineGun.ammoInClip3, Sniper.ammoInClip3);
                setAllWeaponClipSize(HandGun.clipSize3, ShotGun.clipSize3, MachineGun.clipSize3, Sniper.clipSize3);
                break;
			default: 
				setAllWeaponAmmo(HandGun.ammo1, ShotGun.ammo1, MachineGun.ammo1, Sniper.ammo1);
				setAllWeaponAmmoInClip(HandGun.ammoInClip1, ShotGun.ammoInClip1, MachineGun.ammoInClip1, Sniper.ammoInClip1);
				break;
		}
	}

	// sets the ammo for weapons1-4 
	private void setAllWeaponAmmo(int weapon1Ammo, int weapon2Ammo, int weapon3Ammo, int weapon4Ammo) {
		weapon1.ammo = weapon1Ammo; 
		weapon2.ammo = weapon2Ammo; 
		weapon3.ammo = weapon3Ammo; 
		weapon4.ammo = weapon4Ammo;  
	}

	// sets the ammoInClip for weapons1-4 
	private void setAllWeaponAmmoInClip(int weapon1AmmoInClip, int weapon2AmmoInClip, int weapon3AmmoInClip, int weapon4AmmoInClip) {
		weapon1.ammoInClip = weapon1AmmoInClip; 
		weapon2.ammoInClip = weapon2AmmoInClip; 
		weapon3.ammoInClip = weapon3AmmoInClip; 
		weapon4.ammoInClip = weapon4AmmoInClip;  
	}

    private void setAllWeaponClipSize(int weapon1ClipSize, int weapon2ClipSize, int weapon3ClipSize, int weapon4ClipSize)
    {
        
        weapon1.clipSize = weapon1ClipSize;
        weapon2.clipSize = weapon2ClipSize;
        weapon3.clipSize = weapon3ClipSize;
        weapon4.clipSize = weapon4ClipSize;
    
}

    // Update is called once per frame
    void Update () {
        showUI = m_instance_UI.activeSelf;

        //Change plz (check for every player)
        if (players_present.Count > 0)
        {
            if (Input.GetKeyDown(use_button))
            {
                showUI = !showUI;
                m_instance_UI.SetActive(showUI);


                //Set reference to player that has opened the UI
                GameObject root = GameObject.FindWithTag("Gamemanager");
                GameManager gm = root.GetComponent<GameManager>();

                //NEEDS FIX
                int playernumber = players_present[0].GetComponent<PlayerStatistics>().m_playernumber;
                PlayerManager player = gm.getUserManager().m_playerlist[playernumber];
                m_instance_UI.GetComponent<ShopUIScript>().m_currentplayer = player;
            }

        }

        // Disable UI
        if (Time.timeScale == 0)
        {
            showUI = false;
            m_instance_UI.SetActive(showUI);
        }

    }

    //On trigger fuction
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            addplayer(other.gameObject);
        }
    }

    //On exit function
    public void OnTriggerExit(Collider other)
    {
        //Remove player that leaves the area
        if (other.gameObject.CompareTag("Player"))
        {
            //Remove player from area
            removeplayer(other.gameObject);

            //Close UI if player that's purchasing, leaves the area
            int playernumber = other.gameObject.GetComponent<PlayerStatistics>().m_playernumber;
       
            if (m_instance_UI.GetComponent<ShopUIScript>().m_currentplayer.m_PlayerNumber == playernumber)
            {
                showUI = false;
                m_instance_UI.SetActive(false);
            }
        }
    }

    //Add player to player present list
    private void addplayer(GameObject player)
    {
        bool addplayer = true;
        int playernumber = player.GetComponent<PlayerStatistics>().m_playernumber;

        //Only add unique players
        foreach (GameObject m_player in players_present)
        {
            if(m_player.GetComponent<PlayerStatistics>().m_playernumber == playernumber)
            {
                addplayer = false;
                break;
            }
        }

        if (addplayer)
        {
            players_present.Add(player);
        }
    }

    //Remove player in player present list
    private void removeplayer(GameObject player)
    {
        if (players_present.Count >= 1)
        {

            int playernumber = player.GetComponent<PlayerStatistics>().m_playernumber;
            for (int i = 0; i < players_present.Count; i++)
            {
                if (players_present[i].GetComponent<PlayerStatistics>().m_playernumber == playernumber)
                {
                    players_present.RemoveAt(i);
                }
            }
        }
    }

    //Add item to forsalelist
    private void addWeapon(Weapon newweapon)
    {
        for (int i = 0; i < weaponsforsale.Count; i++)
        {
            //Check for duplicate items
            if (newweapon.equals(weaponsforsale[i]))
            {
                break;
            }

            //Check for empty items
            if (weaponsforsale[i].itemtype.Equals(Weapon.ItemType.Empty))
            {
                weaponsforsale[i] = newweapon;
                break;
            }
        }
    }

    //Remove weapon in weaponforsale
    private void removeWeapon(Weapon removeweapon)
    {
        for (int i = 0; i < weaponsforsale.Count; i++)
        {
            if (removeweapon.equals(weaponsforsale[i]))
            {
                weaponsforsale.RemoveAt(i);
                weaponsforsale.Add(new Weapon());
                break;
            }
        }
    }

    //Add item to forsalelist
    private void addAmmo(Weapon newAmmo)
    {
        for (int i = 0; i < ammoforsale.Count; i++)
        {
            //Check for duplicate items
            if (newAmmo.equals(ammoforsale[i]))
            {
                break;
            }

            //Check for empty items
            if (ammoforsale[i].itemtype.Equals(Weapon.ItemType.Empty))
            {
                ammoforsale[i] = newAmmo;
                break;
            }
        }
    }

    //Remove Ammo in ammo for sale
    private void removeAmmo(Weapon removeAmmo)
    {
        for (int i = 0; i < ammoforsale.Count; i++)
        {
            if (removeAmmo.equals(ammoforsale[i]))
            {
                ammoforsale.RemoveAt(i);
                ammoforsale.Add(new Weapon());
                break;
            }
        }
    }

    //Getter show UI
    public bool getActiveUI()
    {
        return showUI;
    }

}
