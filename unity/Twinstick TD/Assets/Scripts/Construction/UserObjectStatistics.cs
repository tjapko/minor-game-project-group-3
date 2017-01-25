using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class contains all the basic function that all objects have
/// </summary>
public class UserObjectStatistics : MonoBehaviour {
    //Public variables
    private int m_maxhealth;     //Max health of the carrot field
   
    //References
    [HideInInspector]public PlayerManager m_owner;   //Instantiated by the player in PlayerConstruction
    public MeshRenderer[] m_meshes;     //Reference to the meshes of this object (set in Inspector)

    //Private variables
    private int m_health;        //Current health of the object
    private PlayerConstruction.PlayerObjectType m_object_type;   //Type of object (set by PlayerConstruction (script))
    private bool object_placed; //Boolean if object is placed
    private bool player_present;    //Boolean if player is standing near the object
    private List<GameObject> colliding_markers;  //List of markers colliding with this object
    private List<string> m_allowedtags;  //Tags of objects that are allowed to intersect

    // Use this for initialization
    void Start()
    {
        //m_health = m_maxhealth;
        object_placed = false;
        player_present = false;
        colliding_markers = new List<GameObject>();

        //InvokeRepeating("checkforPlayer", 0f, 0.1f);
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    //Object is intersecting with the to be placed object
    /*public void OnTriggerEnter(Collider other)
    {

    }

    //Remove objects that're not blocking anymore
    public void OnTriggerExit(Collider other)
    {

    }*/

    // Check if player is standing near turret
	private void checkforPlayer()
    {
        bool playerisPresent = false;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
        {
            if(Vector3.Distance(player.transform.position, gameObject.transform.position) < 2)
            {
                playerisPresent = true;
                break;
            }
        }
        //Player_present=false may be set before the algorithm has finished
        player_present = playerisPresent;   
    }

    // Function add health
    public void changeHealth(int amount)
    {
        if (m_health + amount > m_maxhealth)
        {
            m_health = m_maxhealth;
        }
        else if (m_health + amount <= 0)
        {
            m_health = 0;
            onDeath();
        }
        else
        {
            m_health += amount;
        }
    }

    //Function on death
    public void onDeath()
    {
		if (m_owner != null && m_owner.m_construction != null) {
			m_owner.m_construction.removeObject (gameObject);
		}
    }


    //Get ground clear
    public bool getGroundClear()
    {
        return !player_present;
    }

    // Set owner of object (executed by the PlayerConstructon script)
    public void setOwner(int playernumber)
    {
        GameObject m_root = GameObject.FindWithTag("Gamemanager");
        GameManager m_gamemanager = m_root.GetComponent<GameManager>();
        m_owner = m_gamemanager.getUserManager().m_playerlist[playernumber];

        TurretScript ts = gameObject.GetComponent<TurretScript>();
        if(ts != null)
        {
            ts.setPlayerNumber(playernumber);
        }
    }

    //Setter of object_placed
    public void setPlacement(bool status)
    {
        object_placed = status;

        if (status)
        {
            CancelInvoke("checkforPlayer");
        }
    }

    //Get the object type
    public PlayerConstruction.PlayerObjectType getObjectType()
    {
        return m_object_type;
    }

    //Setter of the object type
    public void setObjectType(PlayerConstruction.PlayerObjectType object_type)
    {
        m_object_type = object_type;
    }

    //Set the texture of the object based on walkable
    public void setMesh(bool walkable)
    {
        if (walkable)
        {
            foreach (MeshRenderer mesh in m_meshes)
            {
                mesh.material.color = Color.white;
            }
        } else
        {
            foreach (MeshRenderer mesh in m_meshes)
            {
                mesh.material.color = Color.red;
            }
        }
    }
}
