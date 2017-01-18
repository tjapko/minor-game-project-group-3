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
    public int m_rebuildbasecost;           //Rebuild base cost 
    public int[] upgrade_cost = new int[4]; //Upgrade to next tier [0] should be empty

    //References
    public GameObject helpbox_prefab;   //Helpbox prefab
    [HideInInspector] public GameObject m_instance_UI;  //Reference to instance of shop UI (set by UIManager)
    public Rigidbody m_Bullet;          // Prefab of the shell.
    public Rigidbody m_RayBullet;       // Prefab of the Rayshell.
    public Transform m_FireTransform;   // A child of the player where the shells are spawned.
    private GameManager m_gamemanager;  // Reference to the game manager
    private UserManager m_usermanager;  // Reference to the user manager

    //Private variables 
    private Weapon weapon1, weapon2, weapon3, weapon4;
    private List<GameObject> players_present;   //List of players that are near the shop
    private bool showUI;    //Boolean if shopUI should be visible
    private bool box_shown; //Boolean if helpbox has been shown
    private int current_tier;   //int : current tier of purchasable weapons (starts at 1)


    // Use this for initialization
    public void Start() {
        //Instantiate Lists
        players_present = new List<GameObject>();
        weaponsforsale = new List<Weapon>();
        //ammoforsale = new List<Weapon>();

        //Set references
        m_gamemanager = GameObject.FindWithTag("Gamemanager").GetComponent<GameManager>();
        m_usermanager = m_gamemanager.getUserManager();


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


        //        For testing
        //      ............. Weapon(name               , id , description       , iconname  , price            , itemtype                   , fireratef           , launchforcef           , maxDamagef           , reloadTimef           , clipsize            , ammo            , ammopriceperclip     , ammoInClip            , maxAmmo            , lifetime)
        weapon1 = new Weapon("Default Weapon", 1, "Default weapon!", "Weapon1", HandGun.price, Weapon.ItemType.HandGun, HandGun.fireRate, HandGun.launchForce, HandGun.maxDamage, HandGun.reloadTime, HandGun.clipSize, HandGun.ammo, HandGun.ammoprice, HandGun.ammoInClip, HandGun.maxAmmo, HandGun.bulletLifeTime);
        weapon4 = new Weapon("Default Weapon 4", 4, "Default weapon!", "sniper", Sniper.price[0], Weapon.ItemType.Sniper, Sniper.fireRate[0], Sniper.launchForce, Sniper.maxDamage[0], Sniper.reloadTime, Sniper.clipSize[0], Sniper.ammo[0], Sniper.ammoprice, Sniper.ammoInClip[0], Sniper.maxAmmo[0], Sniper.bulletLifeTime);
        weapon2 = new Weapon("Default Weapon 2", 2, "Default weapon!", "shotgun", ShotGun.price[0], Weapon.ItemType.Shotgun, ShotGun.fireRate[0], ShotGun.launchForce, ShotGun.maxDamage[0], ShotGun.reloadTime, ShotGun.clipSize[0], ShotGun.ammo[0], ShotGun.ammoprice, ShotGun.ammoInClip[0], ShotGun.maxAmmo[0], ShotGun.bulletLifeTime);
        weapon3 = new Weapon("Default Weapon 3", 3, "Default weapon!", "Weapon3", MachineGun.price[0], Weapon.ItemType.MachineGun, MachineGun.fireRate[0], MachineGun.launchForce, MachineGun.maxDamage[0], MachineGun.reloadTime, MachineGun.clipSize[0], MachineGun.ammo[0], MachineGun.ammoprice, MachineGun.ammoInClip[0], MachineGun.maxAmmo[0], MachineGun.bulletLifeTime);
        addWeapon((Weapon)weapon1);
        addWeapon((Weapon)weapon4);
        addWeapon((Weapon)weapon2);
        addWeapon((Weapon)weapon3);
       
        //		Upgrade (2); // for testing purposes

    }

    public void Upgrade(int level)
    {
        SetGun(level, "ammo");
        SetGun(level, "price");
        SetGun(level, "firerate");
        SetGun(level, "maxdamage");
        SetGun(level, "clipsize");
        SetGun(level, "ammoinclip");
        SetGun(level, "maxammo");

        List<Weapon> temp = new List<Weapon>();
        temp.Add((Weapon)weapon1);
        temp.Add((Weapon)weapon4);
        temp.Add((Weapon)weapon2);
        temp.Add((Weapon)weapon3);

        weaponsforsale = temp;
    }

    // sets the ammo for weapons1-4 
    private void SetGun(int level, string specification)
    {
        switch (specification.ToLower())
        {
            case "ammo":
                // Set ammo a level higher for all weapons
                weapon2.ammo = ShotGun.ammo[level];
                weapon3.ammo = MachineGun.ammo[level];
                weapon4.ammo = Sniper.ammo[level];
                break;

        // Set ammoInClip a level higher for all weapons
            case "ammoinclip":
                weapon2.ammoInClip = ShotGun.ammoInClip[level];
                weapon3.ammoInClip = MachineGun.ammoInClip[level];
                weapon4.ammoInClip = Sniper.ammoInClip[level];
                break;
            // Set clipSize a level higher  for all weapons
            case "slipsize":
                weapon2.clipSize = ShotGun.clipSize[level];
                weapon3.clipSize = MachineGun.clipSize[level];
                weapon4.clipSize = Sniper.clipSize[level];
                break;
            // Set Damage a level higher for all weapons 
            case "maxdamage":
                weapon2.maxDamage = ShotGun.maxDamage[level];
                weapon3.maxDamage = MachineGun.maxDamage[level];
                weapon4.maxDamage = Sniper.maxDamage[level];
                break;
            // Set the price a level higher for all weapons 
            case "price":
                weapon2.price = ShotGun.price[level];
                weapon3.price = MachineGun.price[level];
                weapon4.price = Sniper.price[level];
                break;

            // Set the fireRate a level higher for all weapons
            case "firerate":
                weapon2.fireRate = ShotGun.fireRate[level];
                weapon3.fireRate = MachineGun.fireRate[level];
                weapon4.fireRate = Sniper.fireRate[level];
                break;

            // Set the maxAmmo a level higher for all weapons
            case "maxammo":
                weapon2.maxAmmo = ShotGun.maxAmmo[level];
                weapon3.maxAmmo = MachineGun.maxAmmo[level];
                weapon4.maxAmmo = Sniper.maxAmmo[level];
                break;

            case "id":
                weapon2.itemID += weaponsforsale.Count;
                weapon3.itemID += weaponsforsale.Count;
                weapon4.itemID += weaponsforsale.Count;
                break;

        }
    }

    // Update is called once per frame
    void Update () {
        showUI = m_instance_UI.activeSelf;

        //Check for keyinput
        if (Input.GetKeyDown(use_button))
        {
            //Fix:Should checke every key input of players present in list
            if (players_present.Count > 0)
            {
                showUI = !showUI;
                m_instance_UI.SetActive(showUI);

                //Set reference to player that has opened the UI
                //Fix:
                int playernumber = players_present[0].GetComponent<PlayerStatistics>().m_playernumber;
                m_instance_UI.GetComponent<ShopUIScript>().m_currentplayer = m_usermanager.m_playerlist[playernumber];
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
                m_usermanager.m_playerlist[playernumber].m_shooting.enabled = true;
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
        Upgrade(current_tier);
        current_tier++;
    }

    //Getter for current_tier
    public int getCurrentTier()
    {
        return current_tier;
    }

    //Reset shop
    public void resetShop()
    {
        Start();
    }
}
