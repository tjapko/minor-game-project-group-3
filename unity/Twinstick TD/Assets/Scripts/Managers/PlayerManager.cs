using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// PlayerManager
/// </summary>
[Serializable]
public class PlayerManager
{
	// Still need to figure out how to represent the visibilty of the currency and kills when there are multiple players
	public Text CurrencyText;	// for holding the Currency text which will be visible on the screen
	public Text KillsText;		// for holding the Kills text which will be visible on the screen
    //public Color m_PlayerColor;       // Colour of player
    public Transform m_SpawnPoint;      // Spawn position of player

    [HideInInspector] public int m_PlayerNumber;        // Number of player
    [HideInInspector] public GameObject m_Instance;     // A reference to the instance of the player (Instantiated by gamer manager)
    [HideInInspector] public PlayerMovement m_movement; // Reference to player's movement script

    //private GameObject m_CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.
	private int Kills; 		// int which will hold the number of kills per player
	private int Currency;  // the currency can be the amount of carrots in the base (Carrots-farm) = baseHealth

	//Setup
    public void Setup()
    {
        // Get references to the components.
        m_movement = m_Instance.GetComponent<PlayerMovement>();
		//m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;
    }
		
	// Disable control of player
	public void DisableControl()
	{
		if(m_movement == null)
		{
			Debug.Log("ISNULL");
		}
		m_movement.enabled = false;

		//m_CanvasGameObject.SetActive(false);
	}

	// Enable control of player
	public void EnableControl()
	{
		m_movement.enabled = true;

		//m_CanvasGameObject.SetActive(true);
	}

	// Reset state of player
	public void Reset()
	{
		m_Instance.transform.position = m_SpawnPoint.position;
		m_Instance.transform.rotation = m_SpawnPoint.rotation;

		m_Instance.SetActive(false);
		m_Instance.SetActive(true);
	}

	// Initializes Currency, Kills and their Text representations which are visible on the screen
	public void Start() 
	{
		Currency = 1; // start currency
		Kills = 0;	// start kills 
		SetCurrencyText (); // updates the text representation of the Currency
		SetKillsText (); 	// updates the text representation of the Kills
	}
		
	// Update is called once per frame
	void Update () 
	{
//		Currency += 1;
//		Kills += 1;
//		Debug.Log("Currency:" + Currency); 
//		Debug.Log("Kills:" + Kills); 
//		Debug.Log("CurrencyText:" + CurrencyText.text); 
//		Debug.Log("KillsText:" + KillsText.text); 
		updateCurrency ();	// checks whether the currency needs to be updated
		updateKills ();		// checks whether the kills needs to be updated
	}

	// Update the statistics of the player
	public void updateStatistics () 
	{
		Update ();
	}
		
	/* NEEDS TO BE IMPLEMENTED!(already made this format for later)
	 checks if some amount needs to be added to currency or substracted (waites from a reaction to update the currency) 
	 maybe this is also needs to be updated in the gameManager!
	*/
	public void updateCurrency() 
	{
		int amount;	// initialize int amount (for adding to or substracrting from the currency)
		bool kill=false, purchase = false;	//boolean kill and purchase to check if condition is met 
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
			addCurrency (amount); // update Currency
		} 
		else if (purchase)  // needs to check if something is purchased (e.g. if some money is spended)
		{
			switch(price) 
			{
				// maybe less or more types of prices are needed
				case "a": amount = 1; break;
				case "b": amount = 2; break;
				case "c": amount = 3; break;
				default: amount = 0; break;
			}
			substractCurrency (amount); //update Currency
		}
		SetCurrencyText (); //update Currency text
	}

	/* NEEDS TO BE IMPLEMENTED! 
	 maybe this is needed to be updated in the gameManager!
	*/
	public void updateKills() 
	{
		bool kill = false; 

		if (kill)  // needs to check if a kill is registered (e.g. if some reward is earned)
		{
			addKill (); // update int Kills
		} 
		SetKillsText (); // update Kills text
	}

	// adds +1 to kills 
	public void addKill() 
	{
		Kills += 1;
	}

	// adds amount to currency (e.g. when an enemy is killed or when surviving a wave)
	public void addCurrency(int amount) 
	{
		Currency += amount;
	}

	// subtracts amount from currency (e.g. when something is purchased)
	public void substractCurrency(int amount) 
	{
		Currency -= amount;
	}

	// sets the currencyText which is visible on the screen to the current Currency
	public void SetCurrencyText () 
	{
		CurrencyText.text = "Currency: " + Currency.ToString (); 
	}

	// sets the KillsText which is visible on the screen to the current amount of kills
	public void SetKillsText () 
	{
		KillsText.text = "Kills: " + Kills.ToString (); 
	}

}
