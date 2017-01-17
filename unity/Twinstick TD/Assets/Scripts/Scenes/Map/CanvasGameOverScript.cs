using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasGameOverScript : MonoBehaviour {

    //References to managers
    private GameManager m_gamemanager;      //Reference to game manager (used to invoke next wave)
    private UserManager m_usermanager;      //Reference to the usermanager in the game manager
    private WaveManager m_wavemanager;
    private List<PlayerManager> m_players;  //Reference to the players in the game 

    //References to GameObject
    private GameObject m_gameovermenu;  //Reference to the game over menu
    private GameObject m_restartbutton; //Reference to the restart button

    public void StartInitialization()
    {
        //Find gamemanager
        m_gamemanager = GameObject.FindWithTag("Gamemanager").GetComponent<GameManager>();
        m_usermanager = m_gamemanager.getUserManager();
        m_players = m_usermanager.m_playerlist;
        m_wavemanager = m_gamemanager.getWaveManager();

        //Set references to children
        m_gameovermenu = gameObject.transform.GetChild(0).gameObject;
        m_restartbutton = m_gameovermenu.transform.GetChild(2).gameObject;
        //Activate game over menu
        m_gameovermenu.SetActive(true);

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

        for (int i = 0; i < amountofplayers; i++)
        {
            PlayerManager player = playerlist[i];
            score[i] = player.m_stats.getkills() * 10 + player.m_stats.getCurrency();
            scoretext.text += "Player" + (i + 1) + " : " + score[i];
        }
    }

    // Restart Scee
    public void RestartScene(int sceneindex)
    {
        m_restartbutton.GetComponent<Button>().interactable = false;
        m_restartbutton.transform.GetChild(0).GetComponent<Text>().text = "Restarting Game";
        NextScene(sceneindex);
    }

    // Load next scene
    public void NextScene(int sceneindex)
    {
        SceneManager.LoadScene(sceneindex);
    }
}
