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
	private Weapon weapon1;
	private Weapon weapon2;
	private Weapon weapon3;
	private Weapon weapon4;

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
//      ............. Weapon(name              , id, description      , iconname , price , itemtype                  ,   fireratef , launchforcef , maxDamagef , reloadTimef, clipsize  ,  ammo   , ammopriceperclip , ammoInClip , maxAmmo,  lifetime)
		weapon1 = new Weapon("Default Weapon"  , 1 , "Default weapon!", "Weapon1", 100   , Weapon.ItemType.HandGun   ,   7f        , 75f          , 0.5f       , 0.5f       , 20        , 9999    , 50				 , 20 		  , 500     , 2f);
		weapon4 = new Weapon("Default Weapon 4", 4 , "Default weapon!", "sniper" , 2000  , Weapon.ItemType.Sniper    ,   1.0f      , 100f         , 10f        , 1.0f       , 5         , 10      , 20				 , 5 	      , 50      , 3f);
		weapon2 = new Weapon("Default Weapon 2", 2 , "Default weapon!", "shotgun", 4000  , Weapon.ItemType.Shotgun   ,   1.0f      , 50f          , 2f         , 0.5f       , 10        , 20      , 30               , 10         , 200     , 0.4f);
		weapon3 = new Weapon("Default Weapon 3", 3 , "Default weapon!", "Weapon3", 8000  , Weapon.ItemType.MachineGun,   10f       , 50f          , 0.5f       , 1.0f       , 60        , 120     , 20               , 60         , 480     , 2f);
		addWeapon((Weapon)weapon1);
		addWeapon((Weapon)weapon4);
		addWeapon((Weapon)weapon2);
		addWeapon((Weapon)weapon3);
		Upgrade (2);
    }

	//Upgradable: ammo, ammoInClip
	// toDO: make private variables? 
	public void Upgrade(int UpgradeLevel) {
		// ammo levels for all weapons 
		int weapon1Ammo1 = 9999, weapon1Ammo2 = 9999, weapon1Ammo3 = 9999;
		int weapon2Ammo1 = 20  , weapon2Ammo2 = 35  , weapon2Ammo3 = 50  ;
		int weapon3Ammo1 = 120 , weapon3Ammo2 = 160 , weapon3Ammo3 = 200 ;
		int weapon4Ammo1 = 10  , weapon4Ammo2 = 15  , weapon4Ammo3 = 20  ;
		// ammoInClip levels for all weapons
		int weapon1AmmoInClip1 = 20 , weapon1AmmoInClip2 = 25  , weapon1AmmoInClip3 = 30  ;
		int weapon2AmmoInClip1 = 10 , weapon2AmmoInClip2 = 15  , weapon2AmmoInClip3 = 20  ;
		int weapon3AmmoInClip1 = 60 , weapon3AmmoInClip2 = 80  , weapon3AmmoInClip3 = 100 ;
		int weapon4AmmoInClip1 = 5  , weapon4AmmoInClip2 = 10  , weapon4AmmoInClip3 = 15  ;

		switch (UpgradeLevel) {
		case 2: 
//			weapon1.ammo = weapon1Ammo2;
//			weapon1.ammoInClip = weapon1AmmoInClip2;
//			weapon2.ammo = weapon2Ammo2;
//			weapon2.ammoInClip = weapon2AmmoInClip2;
//			weapon3.ammo = weapon3Ammo2;
//			weapon3.ammoInClip = weapon3AmmoInClip2; 
//			weapon4.ammo = weapon4Ammo2;
//			weapon4.ammoInClip = weapon4AmmoInClip2; 

			setAllWeaponAmmo(weapon1Ammo2, weapon2Ammo2, weapon3Ammo2, weapon4Ammo2 );
			setAllWeaponAmmoInClip(weapon1AmmoInClip2, weapon2AmmoInClip2 ,weapon3AmmoInClip2 ,weapon4AmmoInClip2); 
			break;
		case 3:
//			weapon1.ammo = weapon1Ammo3; weapon1.ammoInClip = weapon1AmmoInClip3;
//			weapon2.ammo = weapon2Ammo3; weapon2.ammoInClip = weapon2AmmoInClip3;
//			weapon3.ammo = weapon3Ammo3; weapon3.ammoInClip = weapon3AmmoInClip3; 
//			weapon4.ammo = weapon4Ammo3; weapon4.ammoInClip = weapon4AmmoInClip3; 

			setAllWeaponAmmo(weapon1Ammo3, weapon2Ammo3, weapon3Ammo3, weapon4Ammo3);
			setAllWeaponAmmoInClip(weapon1AmmoInClip3, weapon2AmmoInClip3 ,weapon3AmmoInClip3 ,weapon4AmmoInClip3);
			break;
		default: 
//			weapon1.ammo = weapon1Ammo1	; weapon1.ammoInClip = weapon1AmmoInClip1;
//			weapon2.ammo = weapon2Ammo1	; weapon2.ammoInClip = weapon2AmmoInClip1;
//			weapon3.ammo = weapon3Ammo1 ; weapon3.ammoInClip = weapon3AmmoInClip1; 
//			weapon4.ammo = weapon4Ammo1 ; weapon4.ammoInClip = weapon4AmmoInClip1; 

			setAllWeaponAmmo(weapon1Ammo1, weapon2Ammo1, weapon3Ammo1, weapon4Ammo1);
			setAllWeaponAmmoInClip(weapon1AmmoInClip1, weapon2AmmoInClip1 ,weapon3AmmoInClip1 ,weapon4AmmoInClip1);
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
		weapon4.ammoInClip= weapon4AmmoInClip;  
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
