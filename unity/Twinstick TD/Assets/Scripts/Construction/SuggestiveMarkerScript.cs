using UnityEngine;
using System.Collections;

public class SuggestiveMarkerScript : MonoBehaviour {

    //Private variables
    private Transform m_position;

	// Use this for initialization
	void Start () {
        m_position = gameObject.transform;

    }

    //Object is intersecting with the to be placed object
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerObject"))
        {
            PlayerManager player = other.gameObject.GetComponent<UserObjectStatistics>().m_owner;
            player.m_Instance.GetComponent<PlayerConstruction>().setSuggestedPosition(m_position);
        }
        //If another marker is detected at the same location, destroy the new marker
        if(other.gameObject.CompareTag("PlayerWall")||
           other.gameObject.CompareTag("PlayerCarrotField") ||
           other.gameObject.CompareTag("PlayerTurret"))
        {
            Destroy(gameObject);
        }
    }

}
