using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseUpgradeScript : MonoBehaviour {
    //Temp variables
    string use_button = "f";        //Set to use button of player[i]

    //References
    [Header("References")]
    public GameObject m_baseupgradeground;  //Reference to the gameobject containing the base collider
    public GameObject m_baseupgradeUI;      //Reference set by UIManager:CanvasBaseUpgrade
    private GameManager m_gamemanager;      //Reference to the game manager
    private UserManager m_usermanager;      //Reference to the User Manager
    private BasePlayerPresentScript m_playerspresentscript; //Reference to the BasePlayerPresentScript script
    private CanvasBaseUpgrade ui_baseupgrades;  //Reference to the base upgrades UI

    //Public Variables
    [Header("Public Variables")]
    [Header("Player Health settings")]
    public int price_restorePlayerHealth;   //Price of restoring player health
    public int amount_restorePlayerHealth;  //Amount that is restored per price_restorePlayerHealth
    public int[] price_upgradePlayerHealth;     //Cost per upgrade
    public int[] amount_upgradePlayerHealth;    //Added Amountof hp per upgrade

    [Header("Base Healt settings")]
    public int price_restoreBaseHealth;     //Price of restoring base health
    public int amount_restoreBaseHealth;    //Amount that is restored per price_restoreBaseHealth
    public int[] price_upgradeBaseHealth;   //Cost per upgrade
    public int[] amount_upgradeBaseHealth;  //Added Amountof hp per upgrade

    //Private Variables
    private List<BaseUpgrade> player_upgradelist;
    private List<BaseUpgrade> base_upgradelist;
    private bool ui_active;

    // Use this for initialization
    public void StartInitialization () {
        //Set references
        m_gamemanager = GameObject.FindWithTag("Gamemanager").GetComponent<GameManager>();
        m_usermanager = m_gamemanager.getUserManager();
        m_playerspresentscript = m_baseupgradeground.GetComponent<BasePlayerPresentScript>();

        //Initialize other scripts
        m_playerspresentscript.StartInitialization();

        //Fill upgrade list
        player_upgradelist = new List<BaseUpgrade>();
        base_upgradelist = new List<BaseUpgrade>();
        player_upgradelist.Add(new Upgrade_PlayerHealth(price_upgradePlayerHealth, amount_upgradePlayerHealth));
        player_upgradelist.Add(new Restore_PlayerHealth(new int[]{ price_restorePlayerHealth }, amount_restorePlayerHealth));
        base_upgradelist.Add(new Upgrade_BaseHealth(price_upgradeBaseHealth, amount_upgradeBaseHealth));
        base_upgradelist.Add(new Restore_BaseHealth(new int[] { price_restoreBaseHealth }, amount_restorePlayerHealth));
        

        //Set variables
        ui_active = false;
    }
	
	// Update is called once per frame
	void Update () {
        ui_active = m_baseupgradeUI.activeSelf;

        //Fix:Should check every player use_button of players that are present
        if (Input.GetKeyUp(use_button))
        {
            List<GameObject> players = m_playerspresentscript.getPlayersPresent();
            if(players.Count > 0)
            {
                //Set UI (in)active
                ui_active = !ui_active;
                m_baseupgradeUI.SetActive(ui_active);

                //Set reference in UI to player that has pressed te button
                int playernumber = players[0].GetComponent<PlayerStatistics>().m_playernumber;
                ui_baseupgrades.setSelectedPlayer(m_usermanager.m_playerlist[playernumber]);
            }
        }

        // Disable UI when game is paused
        if (Time.timeScale == 0)
        {
            ui_active = false;
            m_baseupgradeUI.SetActive(ui_active);
        }
    }

    //remove first entry of list
    public void removeUpgrade(List<int> cost_upgrade_list)
    {
        if(cost_upgrade_list.Count > 0)
        {
            cost_upgrade_list.RemoveAt(0);
        }
    }

    //Getter for base_upgradelist
    public List<BaseUpgrade> getBaseUpgradeList()
    {
        return base_upgradelist;
    }

    //Getter for player_upgradelist
    public List<BaseUpgrade> getPlayerUpgradeList()
    {
        return player_upgradelist;
    }
}
