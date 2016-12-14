using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SuggestiveMarkerScript : MonoBehaviour {

    //Private variables
    private Transform m_position;   //Position of the marker
    private PlayerConstruction.PlayerObjectType m_marker_type;  // Type of the marker
    private List<string> listallowedtags;  //listallowedtags on which the marker responds 

    // Use this for initialization
    void Start () {
        m_position = gameObject.transform; //Set position
    }

    //Object is intersecting with the to be placed object
    public void OnTriggerEnter(Collider other)
    {
        //Check if player is trying to access the marker
        if (other.gameObject.CompareTag("PlayerObject"))
        {
            //Set the suggested location
            PlayerManager player = other.gameObject.GetComponent<UserObjectStatistics>().m_owner;
            player.m_Instance.GetComponent<PlayerConstruction>().setSuggestedPosition(m_position);
        }

        //Check if marker is intersecting with existing object
        if(other.GetComponent<Rigidbody>() != null)
        {
            checkIntersecting(other);
        }
        
    }

    //Function to check if the marker is on an illegal tile
    private void checkIntersecting(Collider other)
    {
        bool destroy_object = true;
        foreach (string tag in listallowedtags)
        {
            //Destroy marker if it's intersecting 
            if (other.gameObject.CompareTag(tag))
            {
                destroy_object = false;
                break;
            }
        }

        if (destroy_object)
        {
            Destroy(gameObject);
        }
    }

    public void setMarker(PlayerConstruction.PlayerObjectType marker_type)
    {
        //Set listallowedtags
        listallowedtags = new List<string>();
        switch (marker_type)
        {
            //The marker is set by a PlayerWall
            case PlayerConstruction.PlayerObjectType.PlayerWall:
                listallowedtags.Add("PlayerShell");
                listallowedtags.Add("EnemyShell");
                listallowedtags.Add("PlayerConstructionMarker");
                listallowedtags.Add("PlayerWall"); //Walls may not intersect with walls
                listallowedtags.Add("PlayerTurret"); //Carrot fields may not be placed on top of eachother
                listallowedtags.Add("PlayerCarrotField"); //Carrot fields may not be placed upon Carrot Fields
                listallowedtags.Add("PlayerMud");  //Carrot fields may not be placed upon Mud
                break;
            //The marker is set by a Turret
            case PlayerConstruction.PlayerObjectType.PlayerTurret:
                listallowedtags.Add("PlayerShell");
                listallowedtags.Add("EnemyShell");
                listallowedtags.Add("PlayerConstructionMarker");
                listallowedtags.Add("PlayerWall"); //Walls may not intersect with walls
                break;
            case PlayerConstruction.PlayerObjectType.PlayerCarrotField:
                listallowedtags.Add("PlayerShell");
                listallowedtags.Add("EnemyShell");
                listallowedtags.Add("PlayerConstructionMarker");
                listallowedtags.Add("PlayerWall"); //Walls may not intersect with walls
                break;
            case PlayerConstruction.PlayerObjectType.PlayerMud:
                listallowedtags.Add("PlayerShell");
                listallowedtags.Add("EnemyShell");
                listallowedtags.Add("PlayerConstructionMarker");
                listallowedtags.Add("PlayerWall"); //Walls may not intersect with walls
                break;
            default:
                break;
        }


    }

}
