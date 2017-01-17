using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasePlayerPresentScript : MonoBehaviour {

    //References
    private BaseUpgradeScript m_baseupgradescript;  //Reference to base

    //Private variables
    private List<GameObject> players_present;   //List of players that are near the base
	
    // Use this for initialization
	public void StartInitialization () {
        players_present = new List<GameObject>();
        m_baseupgradescript = gameObject.transform.parent.GetComponent<BaseUpgradeScript>();
    }

    //On trigger fuction
    private void OnTriggerEnter(Collider other)
    {
        //Look if a player has joined
        if (other.gameObject.CompareTag("Player"))
        {
            //Check if this script has been initialized 
            if (players_present == null)
            {
                StartInitialization();
            }

            //Check if player is not in the list
            if (!playerAlreadyPresent(other.gameObject))
            {
                players_present.Add(other.gameObject);
            }
        }
    }

    //On exit function
    public void OnTriggerExit(Collider other)
    {
        //Check if this script has been initialized 
        if (players_present == null)
        {
            StartInitialization();
        }

        for (int i = 0; i < players_present.Count; i++)
        {
            if (other.gameObject.GetInstanceID() == players_present[i].GetInstanceID())
            {
                m_baseupgradescript.showUICanvas(false);
                players_present.RemoveAt(i);
                break;
            }
        }
    }

    //Returns if player is present in the list
    private bool playerAlreadyPresent(GameObject player)
    {

        foreach (GameObject list_player in players_present)
        {
            if (player.GetInstanceID() == list_player.GetInstanceID())
            {
                return true;
            }
        }
        return false;
    }

    //Getter for players_present
    public List<GameObject> getPlayersPresent()
    {
        return players_present;
    }
}
