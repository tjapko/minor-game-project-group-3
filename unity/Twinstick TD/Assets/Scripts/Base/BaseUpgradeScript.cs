using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseUpgradeScript : MonoBehaviour {
    //Temp variables
    string use_button = "f";        //Set to use button of player[i]

    //References
    [Header("References")]
    public GameObject m_baseupgradeground;  //Reference to the gameobject containing the base collider
    public List<GameObject> m_baseturrets;  //Reference to list of turrets
    private GameObject m_baseupgradeUI;      //Reference set by UIManager:CanvasBaseUpgrade
    private GameManager m_gamemanager;      //Reference to the game manager
    private UserManager m_usermanager;      //Reference to the User Manager
    private BasePlayerPresentScript m_playerspresentscript; //Reference to the BasePlayerPresentScript script
    private CanvasBaseUpgrade ui_baseupgrades;  //Reference to the base upgrades UI

    //Public Variables
//    [Header("Public Variables:")]
    [Header("Player Health settings")]
    public int price_restorePlayerHealth;   //Price of restoring player health
	public int amount_restorePlayerHealth;  //Amount that is restored per price_restorePlayerHealth
    public int[] price_upgradePlayerHealth;     //Cost per upgrade
    public int[] amount_upgradePlayerHealth;    //Added Amountof hp per upgrade

    [Header("Base Health settings")]
    public int price_restoreBaseHealth;     //Price of restoring base health
    public int amount_restoreBaseHealth;    //Amount that is restored per price_restoreBaseHealth
    public int[] price_upgradeBaseHealth;   //Cost per upgrade
    public int[] amount_upgradeBaseHealth;  //Added Amountof hp per upgrade

    [Header("Turret stats settings")]
    public float[] upgradePlayerTurretHealth;   //Upgrade the health of a player turret
	public float[] upgradeBaseTurretDamage;     //Damage of the turret per upgrade
	public float[] upgradeBaseTurretRange;  	//Range of the turret per upgrade
	public float[] upgradeBaseTurretAccuracy;   //Accuracy of the turret per upgrade
	public float[] upgradeBaseTurretFirerate;   //FireRate of the turret per upgrade
	public float[] upgradeBaseTurretLaunchForce;//LaunchForce of the turret per upgrade
	public float[] upgradeBaseTurretTurnRate;   //TurnRate of the turret per upgrade

    //Private Variables
    private List<List<BaseUpgrade>> player_upgradelist; //List containing upgrade duo's
    private List<List<BaseUpgrade>> base_upgradelist;   //List containing upgrade duo's
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
        player_upgradelist = new List<List<BaseUpgrade>>();
        base_upgradelist = new List<List<BaseUpgrade>>();
        List<BaseUpgrade> upgrade_duo_1 = new List<BaseUpgrade>();
        List<BaseUpgrade> upgrade_duo_2 = new List<BaseUpgrade>();
        upgrade_duo_1.Add((BaseUpgrade)new Upgrade_PlayerHealth(price_upgradePlayerHealth, amount_upgradePlayerHealth));
        upgrade_duo_1.Add((BaseUpgrade)new Restore_PlayerHealth(new int[] { price_restorePlayerHealth }, amount_restorePlayerHealth));
		upgrade_duo_2.Add((BaseUpgrade)new Upgrade_Base(price_upgradeBaseHealth, amount_upgradeBaseHealth, upgradePlayerTurretHealth, upgradeBaseTurretDamage, upgradeBaseTurretRange, upgradeBaseTurretAccuracy,upgradeBaseTurretFirerate, upgradeBaseTurretLaunchForce, upgradeBaseTurretTurnRate));
        upgrade_duo_2.Add((BaseUpgrade)new Restore_BaseHealth(new int[] { price_restoreBaseHealth }, amount_restoreBaseHealth));
        
        player_upgradelist.Add(upgrade_duo_1);
        base_upgradelist.Add(upgrade_duo_2);

        //Disable turrets
        foreach(GameObject turret in m_baseturrets)
        {
            turret.SetActive(false);
        }

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
    public List<List<BaseUpgrade>> getBaseUpgradeList()
    {
        return base_upgradelist;
    }

    //Getter for player_upgradelist
    public List<List<BaseUpgrade>> getPlayerUpgradeList()
    {
        return player_upgradelist;
    }

    public void setReferenceBaseUpgradeUI(GameObject ui)
    {
        m_baseupgradeUI = ui;
        ui_baseupgrades = m_baseupgradeUI.GetComponent<CanvasBaseUpgrade>();
    }
    
    //Set canvas active
    public void showUICanvas(bool status)
    {
        m_baseupgradeUI.SetActive(status);
    }

    public void resetTurrets()
    {
        foreach(GameObject turret in m_baseturrets)
        {
            turret.SetActive(false);
        }
    }
}
