using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats {

	public int startCurrency = 5;
	public Text CurrencyText;
	public Text KillsText;

	private int Kills = 0; 
	private int Currency;  // the currency can be the amount of carrots in the base (Carrots-farm) = baseHealth


	// initializes Currency to startCurrency
	void Start () 
	{
		Currency = startCurrency;
		SetCurrencyText (); 
	}

	// Update is called once per frame
	void Update () 
	{
		updateCurrency ();
		updateKills ();
	}

	/* NEEDS TO BE IMPLEMENTED! (already made this format for later)
	 checks if some amount needs to be added to currency or substracted (waites from a reaction to update the currency) 
	 maybe this is needed to be updated in the gameManager!
	*/
	void updateCurrency() 
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
	void updateKills() 
	{
		bool kill = false;

		if (kill)  // needs to check if a kill is registered (e.g. if some reward is earned)
		{
			addKill ();
		} 
		SetKillsText ();
	}

	// adds +1 to kills 
	void addKill() 
	{
		Kills += 1;
	}

	// adds amount to currency (e.g. when an enemy is killed or when surviving a wave)
	void addCurrency(int amount) 
	{
		Currency += amount;
	}

	// subtracts amount from currency (e.g. when something is purchased)
	void substractCurrency(int amount) 
	{
		Currency -= amount;
	}

	// sets the currencyText which is visible on the screen to the current Currency
	void SetCurrencyText () 
	{
		CurrencyText.text = "Currency: " + Currency.ToString (); 
	}

	// sets the KillsText which is visible on the screen to the current amount of kills
	void SetKillsText () 
	{
		KillsText.text = "Kills: " + Kills.ToString (); 
	}
	
}

