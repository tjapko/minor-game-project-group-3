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
	public Text CurrencyText;
	public Text KillsText;
    //public Color m_PlayerColor;       // Colour of player
    public Transform m_SpawnPoint;      // Spawn position of player

    [HideInInspector] public int m_PlayerNumber;        // Number of player
    [HideInInspector] public GameObject m_Instance;     // A reference to the instance of the player (Instantiated by gamer manager)
    [HideInInspector] public PlayerMovement m_movement; // Reference to player's movement script

    //private GameObject m_CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.
	private int Kills = 0; 
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

	// Initializes Currency, Kills and their Text representations on the screen
	public void Start() 
	{
		Currency = 1; // start currency
		Kills = 0;	// start kills 
		SetCurrencyText ();
		SetKillsText ();
	}
		
	// Update is called once per frame
	void Update () 
	{
		Currency += 1;
		Kills += 1;
//		Debug.Log("Currency:" + Currency); 
//		Debug.Log("Kills:" + Kills); 
		updateCurrency ();
		updateKills ();
	}

	// Update the statistics of the player
	public void updateStatistics () 
	{
		Update ();
	}
		
	/* NEEDS TO BE IMPLEMENTED! (already made this format for later)
	 checks if some amount needs to be added to currency or substracted (waites from a reaction to update the currency) 
	 maybe this is needed to be updated in the gameManager!
	*/
	public void updateCurrency() 
	{
		int amount = 0;
		bool kill=false, purchase = false;
		string reward = "", price = "";

		if (kill)  // needs to check if a kill is registered (e.g. if some reward is earned)
		{
			switch (reward) 
			{
			case "a": amount = 1; break;
			case "b": amount = 2; break;
			case "c": amount = 3; break;
			default: amount = 0; break;
			}
			addCurrency (amount);
		} 
		else if (purchase)  // needs to check if something is purchased (e.g. if some money is spended)
		{
			switch(price) 
			{
			case "a": amount = 1; break;
			case "b": amount = 2; break;
			case "c": amount = 3; break;
			default: amount = 0; break;
			}
			substractCurrency (amount);
		}
		SetCurrencyText ();
		SetKillsText ();
	}

	/* NEEDS TO BE IMPLEMENTED! 
	 maybe this is needed to be updated in the gameManager!
	*/
	public void updateKills() 
	{
		bool kill = false;

		if (kill)  // needs to check if a kill is registered (e.g. if some reward is earned)
		{
			addKill ();
		} 
		SetKillsText ();
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
