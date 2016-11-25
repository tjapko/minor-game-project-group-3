using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserManager {

    //Public variables
    public GameObject m_Playerprefab;       //Reference to prefab of player
    public Transform m_playerspawnpoint;    //Spawnpoints of enemies
    public int m_totalplayers;              //Total amount of players
    public List<PlayerManager> m_playerlist;   //List of players

    //Private variables


    // Update is called for every player to update their statistics
    public void Update()
    {
        foreach(PlayerManager player in m_playerlist)
        {
            player.Update();
        }
    }

    // Use this for initialization
    public UserManager(GameObject Playerprefab, Transform playerspawnpoint, int totalplayers)
    {
        m_Playerprefab = Playerprefab;
        m_playerspawnpoint = playerspawnpoint;
        m_totalplayers = totalplayers;

        m_playerlist = new List<PlayerManager>();
    }

    // Spawn all players
    public void spawnPlayers()
    {
        for (int i = 0; i < m_totalplayers; i++)
        {
            GameObject newinstance = GameObject.Instantiate(m_Playerprefab, m_playerspawnpoint.position, m_playerspawnpoint.rotation) as GameObject;
            m_playerlist.Add(new PlayerManager(m_playerspawnpoint, i, newinstance));
        }
    }

    // Determine if players are dead (hasn't been tested)
    public bool playerDead()
    {
        foreach (PlayerManager player in m_playerlist)
        {
            if(player.m_Instance.activeSelf)
            {
                return false;
            }
        }
        return true;
    }

    // Enable player control
    public void enablePlayersControl()
    {
        foreach(PlayerManager player in m_playerlist)
        {
            player.EnableControl();
        }
    }

    //Disable player control
    public void disablePlayersControl()
    {
        foreach (PlayerManager player in m_playerlist)
        {
            player.DisableControl();
        }
    }

    //Reset all players
    public void resetAllPlayers()
    {
        foreach (PlayerManager player in m_playerlist)
        {
            player.Reset();
        }
    }
}
