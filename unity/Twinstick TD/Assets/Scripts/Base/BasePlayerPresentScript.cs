using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasePlayerPresentScript : MonoBehaviour {
    //Fix
    private char use_button = 'f';

    //References
    public GameObject helpbox_prefab;     //Reference to the help canvas
    private BaseUpgradeScript m_baseupgradescript;  //Reference to base

    //Private variables
    private List<GameObject> players_present;   //List of players that are near the base
    private bool box_shown;
	
    // Use this for initialization
	public void StartInitialization () {
        players_present = new List<GameObject>();
        m_baseupgradescript = gameObject.transform.parent.GetComponent<BaseUpgradeScript>();
        box_shown = false;
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

            if (!box_shown)
            {
                GameObject instance = GameObject.Instantiate(helpbox_prefab, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
                SpeechBubbleScript instance_script = instance.GetComponent<SpeechBubbleScript>();
                instance_script.m_offset = new Vector3(-10, 6, 0);
                instance_script.setText("Press '" + use_button + "' to open the upgrade menu");
                box_shown = true;
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
