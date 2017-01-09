using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// Class UI script
/// Functions for the GameManager to implement UI 
/// </summary>
public class MapUIScript : MonoBehaviour {

    //References to managers
    private GameManager m_gamemanager;      //Reference to game manager (used to invoke next wave)
    private UserManager m_usermanager;      //Reference to the usermanager in the game manager
    private List<PlayerManager> m_players;  //Reference to the players in the game 

    //References to GameObject
    private GameObject  m_pausemenu;            //Reference to the pausemenu window (child of this GameObject)
    private GameObject  m_wavecontrol;          //Reference to the wave control panel
    private GameObject  m_constructionpanel;    //Reference to the construction panel
    private List<GameObject> m_playerstats;     //Reference to the player stats UI
    private List<Text> m_player_money;          //Reference to the text displaying the money of the player
    private List<Text> m_player_kills;          //Reference to the text displaying the kills ma
    private GameObject  m_gameovermenu;         //Reference to the game over menu
    private List<GameObject> m_weaponstats;     //Referene to the weapons of the player
    private List<List<Image>> m_weaponIcon;     //Reference to the weapon icon slots
    private List<Text> m_weaponammo;            //Reference to the ammo Text component

    private GameObject  m_wavestats;            //Reference tot he Wave stats UI panel

    private void Start()
    {
        //Find gamemanager
        m_gamemanager = GameObject.FindWithTag("Gamemanager").GetComponent<GameManager>();
        m_usermanager = m_gamemanager.getUserManager();
        m_players = m_usermanager.m_playerlist;

        //Set references to children
        m_pausemenu = gameObject.transform.GetChild(0).gameObject;
        m_wavecontrol = gameObject.transform.GetChild(1).gameObject;
        m_constructionpanel = gameObject.transform.GetChild(2).gameObject;
        m_gameovermenu = gameObject.transform.GetChild(5).gameObject;
        m_wavestats = gameObject.transform.GetChild(8).gameObject;

        //Player stats and children
        int amountofplayers = m_gamemanager.m_amountofplayers;
        m_playerstats = new List<GameObject>();
        m_playerstats.Add(gameObject.transform.GetChild(3).gameObject); //Add player 1
        m_playerstats.Add(gameObject.transform.GetChild(4).gameObject); //Add player 2
        //m_playerstats.Add(gameObject.transform.GetChild().gameObject); //Add player 3
        //m_playerstats.Add(gameObject.transform.GetChild().gameObject); //Add player 4

        m_player_money = new List<Text>();
        for(int i = 0; i < amountofplayers; i++)
        {
            m_player_money.Add(m_playerstats[i].transform.GetChild(1).GetComponent<Text>());
        }

        m_player_kills = new List<Text>();
        for (int i = 0; i < amountofplayers; i++)
        {
            m_player_kills.Add(m_playerstats[i].transform.GetChild(2).GetComponent<Text>());
        }

        //Weapon stats and children
        m_weaponstats = new List<GameObject>();
        m_weaponstats.Add(gameObject.transform.GetChild(6).gameObject);
        m_weaponstats.Add(gameObject.transform.GetChild(7).gameObject);
        //m_weaponstats.Add(gameObject.transform.GetChild().gameObject);
        //m_weaponstats.Add(gameObject.transform.GetChild().gameObject);

        m_weaponIcon = new List<List<Image>>();
        for (int i = 0; i < amountofplayers; i++)
        {
            List<Image> image_list = new List<Image>();
            image_list.Add(m_weaponstats[i].transform.GetChild(0).GetComponent<Image>());
            image_list.Add(m_weaponstats[i].transform.GetChild(1).GetComponent<Image>());
            image_list.Add(m_weaponstats[i].transform.GetChild(2).GetComponent<Image>());
            m_weaponIcon.Add(image_list);
        }

        m_weaponammo = new List<Text>();
        for (int i = 0; i < amountofplayers; i++)
        {
            m_weaponammo.Add(m_weaponstats[i].transform.GetChild(3).GetComponent<Text>());
        }

        //Set active UI
        m_pausemenu.SetActive(false);
        m_wavecontrol.SetActive(true);
        m_constructionpanel.SetActive(false);
        m_gameovermenu.SetActive(false);

        //Invoke functions
        InvokeRepeating("UpdateUI", 0.0f, 0.2f);

    }

    //Updates the UI
    public void UpdateUI()
    {
        SetCurrencyText();
        SetKillsText();
        updateWeaponIcon();
        UpdateAmmoText();

    }

    //Show or hide the UI panel
    public IEnumerator showWaveStatsUI()
    {
        //Set the text of the UI
        int wavenumber = m_gamemanager.getWaveNumber();
        m_wavestats.transform.GetChild(0).GetComponent<Text>().text = "Wave " + wavenumber;

        m_wavestats.SetActive(true);
        m_wavestats.GetComponent<Image>().CrossFadeAlpha(1.0f, 2.0f, false);
        m_wavestats.transform.GetChild(0).GetComponent<Text>().CrossFadeAlpha(1.0f, 2.0f, false);

        yield return new WaitForSeconds(1.0f);

        m_wavestats.GetComponent<Image>().CrossFadeAlpha(0.0f, 2.0f, false);
        m_wavestats.transform.GetChild(0).GetComponent<Text>().CrossFadeAlpha(0.0f, 2.0f, false);
        m_wavestats.SetActive(false);
    }

    // sets the currencyText which is visible on the screen to the current Currency
    private void SetCurrencyText()
    {
        foreach(Text currencytext in m_player_money)
        {
            currencytext.text = "Currency: " + m_usermanager.m_playerlist[0].m_stats.getCurrency().ToString();
        }
    }

    // sets the KillsText which is visible on the screen to the current amount of kills
    private void SetKillsText()
    {
        foreach(Text killstext in m_player_kills)
        {
            killstext.text = "Kills: " + m_usermanager.m_playerlist[0].m_stats.getkills().ToString();
        }
    }

    // Sets the icons of the guns
    private void updateWeaponIcon()
    {
        int playerindex = 0;
        foreach(List<Image> player in m_weaponIcon)
        {
            int iconindex = 0;
            foreach(Image icon in player)
            {
                icon.sprite = getWeaponIcon(m_usermanager.m_playerlist[playerindex], iconindex);
                iconindex++;
            }
            playerindex++;
        }
    }

    // Sets the ammo text
    private void UpdateAmmoText()
    {
        int index = 0;
        foreach(Text ammo_text in m_weaponammo)
        {
            ammo_text.text = m_usermanager.m_playerlist[index].m_inventory.inventory[0].ammoInClip + "/" + m_usermanager.m_playerlist[index].m_inventory.inventory[0].ammo;
            index++;
        }
    }

    // Gets the icon of the gun
    private Sprite getWeaponIcon(PlayerManager player, int index)
    {
        return player.m_inventory.inventory[index].itemicon;
    }

    // Show/hide Pause menu
    public void showPauseMenu(bool status)
    {
        if (status)
        {
            m_pausemenu.SetActive(true);

        } else
        {
            m_pausemenu.SetActive(false);
        }
        
    }

    // Show/hide Pause menu
    public void showWaveControl(bool status)
    {
        if(m_wavecontrol == null)
        {
            Debug.Log("Wvecontrol is null");
        }
        if (status)
        {
            m_wavecontrol.SetActive(true);

        }
        else
        {
            m_wavecontrol.SetActive(false);
        }
    }

    // Show/hide Pause menu
    public void showConstructonPanel(bool status)
    {
        /*
        if (status)
        {
            m_constructionpanel.SetActive(true);

        }
        else
        {
            m_constructionpanel.SetActive(false);
        }
        */
        m_constructionpanel.SetActive(false);
    }

    // Show/hide the gameover menu
    public void showGameoverMenu(bool status)
    {
        if (status)
        {
            m_gameovermenu.SetActive(true);
        } else
        {
            m_gameovermenu.SetActive(false);
        }
    }

    // Get the score of the player
    public void setScore()
    {
        Text scoretext = m_gameovermenu.transform.GetChild(1).GetComponent<Text>();
        scoretext.text = "";

        List<PlayerManager> playerlist = m_gamemanager.getUserManager().m_playerlist;
        int amountofplayers = playerlist.Count;
        int[] score = new int[amountofplayers];

        for(int i = 0; i < amountofplayers; i++)
        {
            PlayerManager player = playerlist[i];
            score[i] = player.m_stats.getkills() * 10 + player.m_stats.getCurrency();
            scoretext.text += "Player" + (i+1) + " : " + score[i];
        }

    }

}
