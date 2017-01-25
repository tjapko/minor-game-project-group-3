using UnityEngine;
using System.Collections;

/// <summary>
/// Class PlayerStatistics
/// </summary>
public class PlayerStatistics : MonoBehaviour {

    //Public variables
    public int m_playernumber;  // Current player number
    public int m_playerstartcurrency;   //Start currency of player

	public static int m_currencyBasePerWave = 1000;
	public static int m_currencyPerKill = 100;

    //Private variables

    private int m_kills;      // int which will hold the number of kills per player
    public int m_currency;  // the currency can be the amount of carrots in the base (Carrots-farm) = baseHealth
	private int m_metSalesmanAmount;  // the amount that the player meets the Salesman

    // Use this for initialization
    void Start () {
        m_kills = 0;      // start m_kills 
		m_metSalesmanAmount = 0;  // start m_metSalesmanAmount

        //m_currency = 500;      // start m_currency
    }
	
	// Update is called once per frame
	void Update () {
        updateCurrency();
        updateKills();
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

    // Get m_currency
    public int getCurrency()
    {
        return m_currency;
    }

    // Get m_kills
    public int getkills()
    {
        return m_kills;
    }

	// Get m_metSalesmanAmount
	public int getMetSalesmanAmount()
	{
		return m_metSalesmanAmount;
	}

	// Set m_metSalesmanAmount
	public void setMetSalesmanAmount()
	{
		m_metSalesmanAmount += 1;
	}

	// checks if a player has met the Salesman
	void OnTriggerEnter(Collider other) { 
		if (other.gameObject.tag == "TravellingSalesman") {
			setMetSalesmanAmount ();
//			Debug.Log ("met Salesman: " + m_metSalesmanAmount.ToString() +" times");
		}
	}

    public void reset()
    {
        m_kills = 0;
        m_currency = m_playerstartcurrency;

    }
}
