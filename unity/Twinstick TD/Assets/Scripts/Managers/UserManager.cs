using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserManager {

    //References
    public GameObject m_Playerprefab;       // Reference to prefab of player
    public GameObject m_playerobjects;      // Reference to the objects placed by the user
    public Transform m_playerspawnpoint;    // Spawnpoints of the player

    public int m_totalplayers;                  //Total amount of players
    public List<PlayerManager> m_playerlist;    //List of players

    //Private variables


    // Use this for initialization
    public UserManager(GameObject Playerprefab, GameObject Objectprefab, Transform playerspawnpoint, int totalplayers)
    {
        m_Playerprefab = Playerprefab;
        m_playerobjects = Objectprefab;
        m_playerspawnpoint = playerspawnpoint;
        m_totalplayers = totalplayers;

        m_playerlist = new List<PlayerManager>();
    }

    // Spawn all players
    public void spawnPlayers()
    {
        for (int i = 0; i < m_totalplayers; i++)
        {
            createplayer(m_Playerprefab, m_playerspawnpoint, i);
        }
    }

    // Destroy all players
    public void destroyPlayers()
    {
        foreach(PlayerManager player in m_playerlist)
        {
            GameObject.Destroy(player.m_Instance);
        }
        m_playerlist = new List<PlayerManager>();
    }

    // Spawn a player
    private void createplayer(GameObject prefab, Transform spawn, int playernumber)
    {
        //Create gameobject and create a PlayerManager
        
        GameObject newinstance = GameObject.Instantiate(prefab, spawn.position, spawn.rotation) as GameObject;
        PlayerManager newplayer = new PlayerManager(spawn, playernumber, newinstance);
        newplayer.m_construction.m_objectprefab = m_playerobjects;  //Set reference to objects that can be placed
        m_playerlist.Add(newplayer);    //Add player to list
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

    //Set construction phase for users
    public void setConstructionphase(bool status)
    {
        foreach(PlayerManager player in m_playerlist)
        {
            player.m_construction.setconstructionphase(status);
        }
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
