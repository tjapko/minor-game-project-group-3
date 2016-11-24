using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// PlayerManager
/// </summary>
[Serializable]
public class PlayerManager
{
    // Public variables
    // Still need to figure out how to represent the visibilty of the currency and kills when there are multiple players
    public Transform m_SpawnPoint;      // Spawn position of player
    public int m_PlayerNumber;        // Number of player
    public GameObject m_Instance;     // A reference to the instance of the player (Instantiated by gamer manager)
    public PlayerMovement m_movement; // Reference to player's movement script
    public BulletFire m_shooting;     // Reference to player's shooting script

    //Private variables
    private int m_kills;      // int which will hold the number of kills per player
    private int m_currency;  // the currency can be the amount of carrots in the base (Carrots-farm) = baseHealth

    //Constructor
    public PlayerManager (Transform spawnpoint, int playernumber, GameObject instance)
    {
        m_SpawnPoint = spawnpoint;
        m_PlayerNumber = playernumber;
        m_Instance = instance;

        Setup();
    }

    //Setup
    // Initializes Currency, m_kills and their Text representations which are visible on the screen
    public void Setup()
    {
        m_currency = 1;   // start m_currency
        m_kills = 0;      // start m_kills 
        // Get references to the components.
        m_movement = m_Instance.GetComponent<PlayerMovement>();
        m_shooting = m_Instance.GetComponent<BulletFire>();


        //m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

    }

    // Update is called once per frame
    public void Update()
    {
        updateCurrency();   // checks whether the currency needs to be updated
        updateKills();      // checks whether the kills needs to be updated
    }

    // Enable control of player
    public void EnableControl()
    {
        m_movement.enabled = true;
        m_shooting.enabled = true;
        //m_CanvasGameObject.SetActive(true);
    }

    // Disable control of player
    public void DisableControl()
    {
        m_movement.enabled = false;
        m_shooting.enabled = false;
        //m_CanvasGameObject.SetActive(false);
    }

    /* NEEDS TO BE IMPLEMENTED!(already made this format for later)
	 checks if some amount needs to be added to m_currency or substracted (waites from a reaction to update the m_currency) 
	 maybe this is also needs to be updated in the gameManager!
	*/
    public void updateCurrency()
    {
        int amount; // initialize int amount (for adding to or substracrting from the m_currency)
        bool kill = false, purchase = false;    //boolean kill and purchase to check if condition is met 
        string reward = "", price = ""; // for different rewards of killing enemies and different prices of purchases

        if (kill)  // needs to check if a kill is registered (e.g. if some reward is earned)
        {
            switch (reward)
            {
                //  maybe less or more types of rewards are needed
                case "a": amount = 1; break;
                case "b": amount = 2; break;
                case "c": amount = 3; break;
                default: amount = 0; break;
            }
            addCurrency(amount); // update Currency
        }
        else if (purchase)  // needs to check if something is purchased (e.g. if some money is spended)
        {
            switch (price)
            {
                // maybe less or more types of prices are needed
                case "a": amount = 1; break;
                case "b": amount = 2; break;
                case "c": amount = 3; break;
                default: amount = 0; break;
            }
            substractCurrency(amount); //update Currency
        }
    }

    /* NEEDS TO BE IMPLEMENTED! 
	 maybe this is needed to be updated in the gameManager!
	*/
    public void updateKills()
    {
        bool kill = false;

        if (kill)  // needs to check if a kill is registered (e.g. if some reward is earned)
        {
            addKill(); // update int Kills
        }
    }

    // adds +1 to kills 
    public void addKill()
    {
        m_kills += 1;
    }

    // adds amount to currency (e.g. when an enemy is killed or when surviving a wave)
    public void addCurrency(int amount)
    {
        m_currency += amount;
    }

    // subtracts amount from currency (e.g. when something is purchased)
    public void substractCurrency(int amount)
    {
        m_currency -= amount;
    }

    // Get currency
    public int getCurrency()
    {
        return m_currency;
    }

    // Get kills
    public int getkills()
    {
        return m_kills;
    }

    // Reset state of player
    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }

    
}
