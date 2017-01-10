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
    [HideInInspector] public int maxweapons = 4;    //Max weapons the store may contain
    //[HideInInspector] public int maxammotype = 4;   //Max ammo type the store may contain
    [HideInInspector] public List<Weapon> weaponsforsale;
    //[HideInInspector] public List<Weapon> ammoforsale;
    public int[] upgrade_cost = new int[4]; //Upgrade to next tier [0] should be empty
    
    //References
    public GameObject helpbox_prefab;   //Helpbox prefab
    [HideInInspector]
    public GameObject m_instance_UI;  //Reference to instance of shop UI
    public Rigidbody m_Bullet;          // Prefab of the shell.
    public Rigidbody m_RayBullet;       // Prefab of the Rayshell.
    public Transform m_FireTransform;   // A child of the player where the shells are spawned.

    //Private variables
    private List<GameObject> players_present;   //List of players that are near the shop
    private bool showUI;    //Boolean if shopUI should be visible
    private bool box_shown; //Boolean if helpbox has been shown
    private int current_tier;   //int : current tier of purchasable weapons (starts at 1)

	// Use this for initialization
	void Start () {
        //Instantiate Lists
        players_present = new List<GameObject>();
        weaponsforsale = new List<Weapon>();
        //ammoforsale = new List<Weapon>();
        

        //Create empty weaponsforsale
        while (weaponsforsale.Count < maxweapons)
        {
            weaponsforsale.Add(new Weapon());
        }

        //Create empty ammoforsale
        //while(ammoforsale.Count < maxammotype)
        //{
        //    ammoforsale.Add(new global::Weapon());
        //}

        showUI = false; //Set UI active
        box_shown = false;
        current_tier = 1;

        //For testing
        //.................. Weapon(name              , id, description      , iconname , price , itemtype               ,  fireratef , launchforcef , maxDamagef, reloadTimef, clipsize ,  ammo , ammopriceperclip, ammoInClip, maxAmmo, lifetime)
        Weapon weapon1 = new Weapon("Default Weapon 2", 1, "Default weapon!", "Weapon1", 200, Weapon.ItemType.HandGun, 1.0f, 50f, 2f, 0.5f, 10, 20, 30, 10, 200, 0.4f);
        Weapon weapon2 = new Weapon("Default Weapon 3", 4, "Default weapon!", "sniper", 2000, Weapon.ItemType.Sniper, 1.0f, 100f, 10f, 1.0f, 5, 10, 20, 5, 50, 3f);
        Weapon weapon3 = new Weapon("Default Weapon 2", 2, "Default weapon!", "shotgun", 4000, Weapon.ItemType.Shotgun,      1.0f   , 50f, 2f, 0.5f, 10, 20, 30, 10, 200, 0.4f);
        Weapon weapon4 = new Weapon("Default Weapon 3", 3, "Default weapon!", "Weapon3", 6000, Weapon.ItemType.MachineGun,   10f     ,    50f, 0.25f, 1.0f, 60, 120, 20, 60, 480, 2f);
        addWeapon(weapon1);
        addWeapon(weapon2);
        addWeapon(weapon3);
        addWeapon(weapon4);

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

            if (!box_shown)
            {
                GameObject instance = GameObject.Instantiate(helpbox_prefab, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
                SpeechBubbleScript instance_script = instance.GetComponent<SpeechBubbleScript>();
                instance_script.setText("Press '" + use_button + "' to open the store");
                box_shown = true;
            }
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
    /*
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
    */

    //Getter show UI
    public bool getActiveUI()
    {
        return showUI;
    }

    //Increase tier and load new weapons
    public void incTier()
    {
        current_tier++;
        //GET NEW WEAPONS
    }

    //Getter for current_tier
    public int getCurrentTier()
    {
        return current_tier;
    }
}
