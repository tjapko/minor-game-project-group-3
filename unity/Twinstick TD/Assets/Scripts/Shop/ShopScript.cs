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
    public List<Item> weaponsforsale;
    public List<Item> ammoforsale;

    //Reference
    public GameObject m_ShopUIprefab;
    private GameObject m_instance_UI;

    //Private variables
    private List<GameObject> players_present;
    private bool showUI;

	// Use this for initialization
	void Start () {
        //Instantiate Lists
        players_present = new List<GameObject>();
        weaponsforsale = new List<Item>();
        ammoforsale = new List<Item>();

        //Create empty weaponsforsale
        while (weaponsforsale.Count < maxweapons)
        {
            weaponsforsale.Add(new Item());
        }

        //Create empty ammoforsale
        while(ammoforsale.Count < maxammotype)
        {
            ammoforsale.Add(new global::Item());
        }

        //Instantiate UI
        m_instance_UI = GameObject.Instantiate(m_ShopUIprefab);
        m_instance_UI.GetComponent<ShopUIScript>().m_shopscript = gameObject.GetComponent<ShopScript>();
        m_instance_UI.SetActive(false);

        //Set UI active
        showUI = false;

        //For testing
        Item weapon1 = new Item("Default Weapon 1", 1, "Default weapon!", "Weapon1", 100, Item.ItemType.Weapon);
        Item weapon2 = new Item("Default Weapon 2", 2, "Default weapon!", "Weapon2", 1001, Item.ItemType.Weapon);
        Item weapon3 = new Item("Default Weapon 3", 3, "Default weapon!", "Weapon3", 100, Item.ItemType.Weapon);
        addWeapon(weapon1);
        addWeapon(weapon2);
        //addWeapon(weapon3);

    }

    // Update is called once per frame
    void Update () {
        //Change plz (check for every player)
        if (players_present.Count > 0 && Input.GetKeyDown(use_button))
        {
            //Show or hide the UI
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
        int playernumber = player.GetComponent<PlayerStatistics>().m_playernumber;
        for(int i = 0; i < players_present.Count; i++)
        {
            if (players_present[i].GetComponent<PlayerStatistics>().m_playernumber == playernumber)
            {
                players_present.RemoveAt(i);
            }
        }
    }

    //Add item to forsalelist
    private void addWeapon(Item newweapon)
    {
        for (int i = 0; i < weaponsforsale.Count; i++)
        {
            //Check for duplicate items
            if (newweapon.equals(weaponsforsale[i]))
            {
                break;
            }

            //Check for empty items
            if (weaponsforsale[i].itemtype.Equals(Item.ItemType.Empty))
            {
                weaponsforsale[i] = newweapon;
                break;
            }
        }
    }

    //Remove weapon in weaponforsale
    private void removeWeapon(Item removeweapon)
    {
        for (int i = 0; i < weaponsforsale.Count; i++)
        {
            if (removeweapon.equals(weaponsforsale[i]))
            {
                weaponsforsale.RemoveAt(i);
                weaponsforsale.Add(new Item());
                break;
            }
        }
    }

    //Add item to forsalelist
    private void addAmmo(Item newAmmo)
    {
        for (int i = 0; i < ammoforsale.Count; i++)
        {
            //Check for duplicate items
            if (newAmmo.equals(ammoforsale[i]))
            {
                break;
            }

            //Check for empty items
            if (ammoforsale[i].itemtype.Equals(Item.ItemType.Empty))
            {
                ammoforsale[i] = newAmmo;
                break;
            }
        }
    }

    //Remove Ammo in ammo for sale
    private void removeAmmo(Item removeAmmo)
    {
        for (int i = 0; i < ammoforsale.Count; i++)
        {
            if (removeAmmo.equals(ammoforsale[i]))
            {
                ammoforsale.RemoveAt(i);
                ammoforsale.Add(new Item());
                break;
            }
        }
    }

}
