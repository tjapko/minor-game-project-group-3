﻿using UnityEngine;
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
    private WaveManager m_wavemanager;
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
    private List<List<Text>> m_weaponammo;            //Reference to the ammo Text component
    private GameObject m_wavestats;             //Reference tot he Wave stats UI panel
    private GameObject m_waveremaining;         //Reference to the wave remaining UI panel
    private Text m_waveremainingText;           //Reference to the text in the wave remaining UI panel

    private void Start()
    {
        //Find gamemanager
        m_gamemanager = GameObject.FindWithTag("Gamemanager").GetComponent<GameManager>();
        m_usermanager = m_gamemanager.getUserManager();
        m_players = m_usermanager.m_playerlist;
        m_wavemanager = m_gamemanager.getWaveManager();

        //Set references to children
        m_pausemenu = gameObject.transform.GetChild(0).gameObject;
        m_wavecontrol = gameObject.transform.GetChild(1).gameObject;
        m_constructionpanel = gameObject.transform.GetChild(2).gameObject;
        m_gameovermenu = gameObject.transform.GetChild(5).gameObject;
        m_wavestats = gameObject.transform.GetChild(8).gameObject;
        m_waveremaining = gameObject.transform.GetChild(9).gameObject;

        //Player stats and children
        int amountofplayers = m_gamemanager.m_amountofplayers;
        m_playerstats = new List<GameObject>();
        m_playerstats.Add(gameObject.transform.GetChild(3).gameObject); //Add player 1
        m_playerstats.Add(gameObject.transform.GetChild(4).gameObject); //Add player 2
        //m_playerstats.Add(gameObject.transform.GetChild().gameObject); //Add player 3
        //m_playerstats.Add(gameObject.transform.GetChild().gameObject); //Add player 4

        //Set m_playerstats active
        for (int i = 0; i < m_playerstats.Count; i++)
        {
            if (i < amountofplayers)
            {
                m_playerstats[i].SetActive(true);
            }
            else
            {
                m_playerstats[i].SetActive(false);
            }
        }

        //Set player money Text
        m_player_money = new List<Text>();
        for(int i = 0; i < amountofplayers; i++)
        {
            m_player_money.Add(m_playerstats[i].transform.GetChild(1).GetComponent<Text>());
        }

        //Set player kills Text
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

        //Activate weaponstats 
        for(int i = 0; i < m_weaponstats.Count; i++)
        {
            if(i < amountofplayers)
            {
                m_weaponstats[i].SetActive(true);
            } else
            {
                m_weaponstats[i].SetActive(false);
            }
        }

        //Set lists of weapon icons
        m_weaponIcon = new List<List<Image>>();
        for (int i = 0; i < amountofplayers; i++)
        {
            List<Image> image_list = new List<Image>();
            image_list.Add(m_weaponstats[i].transform.GetChild(0).GetComponent<Image>());
            image_list.Add(m_weaponstats[i].transform.GetChild(1).GetComponent<Image>());
            image_list.Add(m_weaponstats[i].transform.GetChild(2).GetComponent<Image>());
            m_weaponIcon.Add(image_list);
        }

        //Set list of weaponammo
        m_weaponammo = new List<List<Text>>();
        for (int i = 0; i < amountofplayers; i++)
        {
            List<Text> temp = new List<Text>();
            temp.Add(m_weaponstats[i].transform.GetChild(3).GetComponent<Text>());
            temp.Add(m_weaponstats[i].transform.GetChild(4).GetComponent<Text>());
            temp.Add(m_weaponstats[i].transform.GetChild(5).GetComponent<Text>());
            m_weaponammo.Add(temp);
        }

        m_waveremainingText = m_waveremaining.transform.GetChild(0).GetComponent<Text>();

        //Set active UI
        m_pausemenu.SetActive(false);
        m_wavecontrol.SetActive(true);
        m_constructionpanel.SetActive(true);
        m_gameovermenu.SetActive(false);
        m_waveremaining.SetActive(false);

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
        setEnemiesRemainingText();

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

    // Sets the enemies remaining text
    private void setEnemiesRemainingText()
    {
        m_waveremainingText.text = "Enemies left: " + m_wavemanager.enemiesRemaining();
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
        int player_index = 0;
        foreach(List<Text> player_ui in m_weaponammo)
        {
            int element_index = 0;
            foreach (Text ammo_text in player_ui)
            {
                ammo_text.text = m_usermanager.m_playerlist[player_index].m_inventory.inventory[element_index].ammoInClip + "/" + m_usermanager.m_playerlist[player_index].m_inventory.inventory[element_index].ammo;
                element_index++;
            }
            player_index++;
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
        m_pausemenu.SetActive(status);

    }

    // Show/hide Pause menu
    public void showWaveControl(bool status)
    {
        m_wavecontrol.SetActive(status);
    }

    // Show/hide Pause menu
    public void showConstructonPanel(bool status)
    {
        m_constructionpanel.SetActive(status);
    }

    // Show/hide the gameover menu
    public void showGameoverMenu(bool status)
    {
        m_gameovermenu.SetActive(status);
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

    // Show/hide the wave remaining panel
    public void showWaveRemaining(bool status)
    {
        m_waveremaining.SetActive(status);
    }
}
