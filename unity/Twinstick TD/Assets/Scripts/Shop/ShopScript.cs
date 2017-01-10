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
//        .................. Weapon(name              , id, description      , iconname , price , itemtype               ,  fireratef , launchforcef , maxDamagef, reloadTimef, clipsize ,  ammo , ammopriceperclip, ammoInClip, maxAmmo, lifetime)
		Weapon weapon1 = new Weapon("Default Weapon"  , 1 , "Default weapon!", "Weapon1", 100   , Weapon.ItemType.HandGun, 7f      , 75f          , 0.5f       , 0.5f       , 20        , 9999    , 50				,20 		,500     , 2f);
		Weapon weapon4 = new Weapon("Default Weapon 3", 4 , "Default weapon!", "sniper", 2000, Weapon.ItemType.Sniper ,      1.0f      , 100f         , 10f      , 1.0f       , 5         , 10    , 20				,5 			,50      , 3f);
		Weapon weapon2 = new Weapon("Default Weapon 2", 2, "Default weapon!", "shotgun", 4000, Weapon.ItemType.Shotgun,      1.0f   , 50f, 2f, 0.5f, 10, 20, 30, 10, 200, 0.4f);
		Weapon weapon3 = new Weapon("Default Weapon 3", 3, "Default weapon!", "Weapon3", 6000, Weapon.ItemType.MachineGun,   10f     ,    50f, 0.25f, 1.0f, 60, 120, 20, 60, 480, 2f);
//		addWeapon((Weapon)weapon1);
		addWeapon((Weapon)weapon4);
		addWeapon((Weapon)weapon2);
		addWeapon((Weapon)weapon3);
	
    }

	//Upgradable: ammo, ammoInClip
	void Upgrade(int UpgradeLevel) {
		switch (UpgradeLevel) {
		case 2: 
			weapon1.maxDamage = 1; weapon1.ammo = 1; 
			weapon2.maxDamage = 1; weapon2.ammo = 1; 
			weapon3.maxDamage = 1; weapon3.ammo = 1; 
			weapon4.maxDamage = 1; weapon3.ammo = 1;
			break;
		case 3: 

			break;
		default: 

			break;
		}
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
