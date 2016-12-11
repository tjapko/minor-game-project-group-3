using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserManager {

    //References
    public GameObject m_Playerprefab;       // Reference to prefab of player
    public Transform m_playerspawnpoint;    // Spawnpoints of the player

    public int m_totalplayers;                  //Total amount of players
    public List<PlayerManager> m_playerlist;    //List of players

    //Private variables
    private int current_wave;

    // Use this for initialization
    public UserManager(GameObject Playerprefab, Transform playerspawnpoint, int totalplayers)
    {
        m_Playerprefab = Playerprefab;
        m_playerspawnpoint = playerspawnpoint;
        m_totalplayers = totalplayers;

        m_playerlist = new List<PlayerManager>();
        current_wave = 0;
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
        newinstance.GetComponent<PlayerConstruction>().m_player = newplayer;
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

    //Give player currency for completion of wave
    public void rewardPlayer()
    {
        foreach(PlayerManager player in m_playerlist)
        {
            //Reward from clearing wave
            player.m_stats.addCurrency(waveCurrency());
            player.m_stats.addCurrency(player.m_construction.getCarrots());
        }
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

    //Function to determine currency per wave
    private int waveCurrency()
    {
        current_wave++;
        return current_wave * 1000;
    }
}
