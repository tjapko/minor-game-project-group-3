using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CanvasConstructionScript : MonoBehaviour {

    //References
    private GameObject m_constructionmenu;  //Reference to construction menu
    private GameObject m_constructionpanel; //Reference to construction panel
    private List<GameObject> m_stats;       //Reference to stats of player
    private List<Text> txt_currency;        //Reference to currency text
    private List<Text> txt_kills;           //Reference to kills text
    private GameObject m_wavecontrolpanel;  //Reference to the wave control panel

    //Private variables
    private GameManager m_gamemanager;  //Reference to Game manager
    private UserManager m_usermanager;  //Reference to User Manager

    public void StartInitialization()
    {
        //Set references
        m_gamemanager = GameObject.FindWithTag("Gamemanager").GetComponent<GameManager>();
        m_usermanager = m_gamemanager.getUserManager();

        //Initialize variables
        int number_players = m_gamemanager.m_amountofplayers;
        m_stats = new List<GameObject>();
        txt_currency = new List<Text>();
        txt_kills = new List<Text>();

        //Set references to children
        m_constructionpanel = gameObject.transform.GetChild(0).gameObject;
        m_stats.Add(gameObject.transform.GetChild(1).gameObject);
        m_stats.Add(gameObject.transform.GetChild(2).gameObject);
        m_wavecontrolpanel = gameObject.transform.GetChild(3).gameObject;

        //Set UI active
        for(int i = 0; i < m_stats.Count; i++)
        {
            if(i < number_players)
            {
                m_stats[i].SetActive(true);
                txt_currency.Add(m_stats[i].transform.GetChild(1).GetComponent<Text>());
                txt_kills.Add(m_stats[i].transform.GetChild(2).GetComponent<Text>());
            } else
            {
                m_stats[i].SetActive(false);
            }
            
        }
    }

    //OnEnable Function
    void OnEnable()
    {
        InvokeRepeating("UpdateUI", 0.0f, 0.1f);
    }

    //OnDisble Function
    void OnDisable()
    {
        CancelInvoke("UpdateUI");
    }

    //Updates the UI
    public void UpdateUI()
    {
        try
        {
            SetCurrencyText();
            SetKillsText();
        }
        catch
        {
            //Start() hasn't finished setting references
        }


    }

    // sets the currencyText which is visible on the screen to the current Currency
    private void SetCurrencyText()
    {
        foreach (Text currencytext in txt_currency)
        {
            currencytext.text = "Currency: " + m_usermanager.m_playerlist[0].m_stats.getCurrency().ToString();
        }
    }

    // sets the KillsText which is visible on the screen to the current amount of kills
    private void SetKillsText()
    {
        foreach (Text killstext in txt_kills)
        {
            killstext.text = "Kills: " + m_usermanager.m_playerlist[0].m_stats.getkills().ToString();
        }
    }
}
