using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// Class UI script
/// Functions for the GameManager to implement UI 
/// </summary>
public class MapUIScript {

    //Private variables
    private GameObject m_instance;
    private UserManager m_usermanager;
    private GameObject m_pausemenu;
    private GameObject m_wavecontrol;
    private GameObject m_constructionpanel;
    private GameObject m_textplayer1;
    private GameObject m_textplayer2;


    //private variable 
    private Text m_currencyText;    // for holding the Currency text which will be visible on the screen
    private Text m_killsText;		// for holding the Kills text which will be visible on the screen

    //Constructer
    public MapUIScript(GameObject ui_prefab, UserManager usermanager)
    {
        m_instance = GameObject.Instantiate(ui_prefab); ;
        m_usermanager = usermanager;
        m_pausemenu = m_instance.transform.GetChild(0).gameObject;
        m_wavecontrol = m_instance.transform.GetChild(1).gameObject;
        m_constructionpanel = m_instance.transform.GetChild(2).gameObject;
        m_textplayer1 = m_instance.transform.GetChild(3).gameObject;
        m_textplayer2 = m_instance.transform.GetChild(4).gameObject;

        m_pausemenu.SetActive(false);
        m_wavecontrol.SetActive(false);
        m_constructionpanel.SetActive(false);

    }

    // Update is called once per frame
    public void Update()
    {
        m_usermanager.Update();
        SetCurrencyText();
        SetKillsText();
    }

    // sets the currencyText which is visible on the screen to the current Currency
    public void SetCurrencyText()
    {
        m_textplayer1.transform.GetChild(1).GetComponent<Text>().text = "Currency: " + m_usermanager.m_playerlist[0].getCurrency().ToString();
        //m_textplayer2.transform.GetChild(1).GetComponent<Text>().text = "Currency: " + m_usermanager.m_playerlist[1].getCurrency().ToString();
    }

    // sets the KillsText which is visible on the screen to the current amount of kills
    public void SetKillsText()
    {
        m_textplayer1.transform.GetChild(2).GetComponent<Text>().text = "Kills: " + m_usermanager.m_playerlist[0].getkills().ToString();
        //m_textplayer2.transform.GetChild(2).GetComponent<Text>().text = "Kills: " + m_usermanager.m_playerlist[1].getkills().ToString();
    }

    // Show Pause menu
    public void showPauseMenu()
    {
        m_pausemenu.SetActive(true);
    }

    // Show Pause menu
    public void hidePauseMenu()
    {
        m_pausemenu.SetActive(false);
    }

    // Show Pause menu
    public void showWaveControl()
    {
        m_wavecontrol.SetActive(true);
    }

    // Show Pause menu
    public void hideWaveControl()
    {
        m_wavecontrol.SetActive(false);
    }

    // Show Pause menu
    public void showConstructonPanel()
    {
        m_constructionpanel.SetActive(true);
    }

    // Show Pause menu
    public void hideConstructonPanel()
    {
        m_constructionpanel.SetActive(false);
    }

}
