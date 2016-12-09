﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// Class UI script
/// Functions for the GameManager to implement UI 
/// </summary>
public class MapUIScript {

    //Referene to managers
    private GameManager m_gamemanager;      //Reference to game manager (used to invoke next wave)
    private GameObject m_instance;          //Reference to instance of this script's prefab
    private UserManager m_usermanager;      //Reference to the usermanager in the game manager

    //References to GameObject
    private GameObject m_pausemenu;         //Reference to the pausemenu window (child of this GameObject)
    private GameObject m_wavecontrol;       //Reference to the wave control panel
    private GameObject m_constructionpanel; //Reference to the construction panel
    private GameObject m_textplayer1;       //Reference to the statistic panel of player 1
    private GameObject m_textplayer2;       //Reference to the statistic panel of player 2
    private GameObject m_gameovermenu;      //Reference to the game over menu
    private GameObject m_weaponplayer1;     //Reference to the weapon UI panel of player 1
    private GameObject m_weaponplayer2;     //Reference to the Weapon UI panel of player 2

    //Constructer
    public MapUIScript(GameManager gamemanager, GameObject ui_prefab, UserManager usermanager)
    {
        //Setting references
        //m_gamemanager = gamemanager;
        m_instance = GameObject.Instantiate(ui_prefab, Vector3.zero, Quaternion.identity) as GameObject; 
        m_usermanager = usermanager;

        //Getting references to childrens
        m_pausemenu = m_instance.transform.GetChild(0).gameObject;
        m_wavecontrol = m_instance.transform.GetChild(1).gameObject;
        m_constructionpanel = m_instance.transform.GetChild(2).gameObject;
        m_textplayer1 = m_instance.transform.GetChild(3).gameObject;
        m_textplayer2 = m_instance.transform.GetChild(4).gameObject;
        m_gameovermenu = m_instance.transform.GetChild(5).gameObject;
        m_weaponplayer1 = m_instance.transform.GetChild(6).gameObject;
        m_weaponplayer2 = m_instance.transform.GetChild(7).gameObject;


        //Set active UI
        m_pausemenu.SetActive(false);
        m_wavecontrol.SetActive(true);
        m_constructionpanel.SetActive(false);
        m_gameovermenu.SetActive(false);
        m_textplayer1.SetActive(true);

        if (gamemanager.m_amountofplayers > 1)
        {
            m_textplayer2.SetActive(true);
            m_weaponplayer2.SetActive(true);
        } else
        {
            m_textplayer2.SetActive(false);
            m_weaponplayer2.SetActive(false);
        }

    }

    // Update is called once per frame
    public void UpdateUI()
    {
        SetCurrencyText();
        SetKillsText();
        updateWeaponIcon();
    }

    // Changing UI back and fourth between phases
    // Wavephase = true : wavephase, Wavephase = false : build phase
    // First check gameover -> game is paused -> phase of game
    public void UIchange(bool gameover, bool wavephase, bool pause)
    {
        //Check for gameover
        if (gameover)
        {
            showGameoverMenu(true);
            showConstructonPanel(false);
            showWaveControl(false);
            showPauseMenu(false);
        } else
        {
            //Check for pause
            if (pause)
            {
                showConstructonPanel(false);
                showWaveControl(false);
                showPauseMenu(true);
                showGameoverMenu(false);
            }
            else
            {
                //Check wavephase
                if (wavephase)
                {
                    showWaveControl(true);
                    showConstructonPanel(false);
                    showPauseMenu(false);
                    showGameoverMenu(false);
                }
                else
                {
                    showWaveControl(true);
                    showConstructonPanel(true);
                    showPauseMenu(false);
                    showGameoverMenu(false);
                }
            }
        }
        
    }

    // sets the currencyText which is visible on the screen to the current Currency
    private void SetCurrencyText()
    {
        m_textplayer1.transform.GetChild(1).GetComponent<Text>().text = "Currency: " + m_usermanager.m_playerlist[0].m_stats.getCurrency().ToString();
        //m_textplayer2.transform.GetChild(1).GetComponent<Text>().text = "Currency: " + m_usermanager.m_playerlist[1].getCurrency().ToString();
    }

    // sets the KillsText which is visible on the screen to the current amount of kills
    private void SetKillsText()
    {
        m_textplayer1.transform.GetChild(2).GetComponent<Text>().text = "Kills: " + m_usermanager.m_playerlist[0].m_stats.getkills().ToString();
        //m_textplayer2.transform.GetChild(2).GetComponent<Text>().text = "Kills: " + m_usermanager.m_playerlist[1].getkills().ToString();
    }

    // Sets the icons of the guns
    private void updateWeaponIcon()
    {
        if(m_usermanager.m_playerlist[0].m_inventory.inventory.Count > 0)
        {
            m_weaponplayer1.transform.GetChild(0).GetComponent<Image>().sprite = getWeaponIcon(m_usermanager.m_playerlist[0], 0);
            m_weaponplayer1.transform.GetChild(1).GetComponent<Image>().sprite = getWeaponIcon(m_usermanager.m_playerlist[0], 1);
            m_weaponplayer1.transform.GetChild(2).GetComponent<Image>().sprite = getWeaponIcon(m_usermanager.m_playerlist[0], 2);
        }
    }

    // Gets the icon of the gun
    private Sprite getWeaponIcon(PlayerManager player, int index)
    {
        return player.m_inventory.inventory[index].itemicon;
    }

    // Show/hide Pause menu
    private void showPauseMenu(bool status)
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
    private void showWaveControl(bool status)
    {
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
    private void showConstructonPanel(bool status)
    {
        if (status)
        {
            m_constructionpanel.SetActive(true);

        }
        else
        {
            m_constructionpanel.SetActive(false);
        }
        
    }

    // Show/hide the gameover menu
    private void showGameoverMenu(bool status)
    {
        if (status)
        {
            m_gameovermenu.SetActive(true);
        } else
        {
            m_gameovermenu.SetActive(false);
        }
    }
}
