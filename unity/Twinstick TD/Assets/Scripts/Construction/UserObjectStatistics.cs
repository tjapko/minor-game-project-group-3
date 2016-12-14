using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class contains all the basic function that all objects have
/// </summary>
public class UserObjectStatistics : MonoBehaviour {
    //Public variables
    public int m_maxhealth;     //Max health of the carrot field
   
    //References
    [HideInInspector]public PlayerManager m_owner;   //Instantiated by the player in PlayerConstruction

    //Private variables
    private int m_health;        //Current health of the object
    private PlayerConstruction.PlayerObjectType m_object_type;   //Type of object (set by PlayerConstruction (script))
    private bool object_placed; //Boolean if object is placed
    private List<GameObject> intersecting_objects;  //List of objects intersecting with this object
    private List<string> m_allowedtags;  //Tags of objects that are allowed to intersect

    // Use this for initialization
    void Start()
    {
        m_health = m_maxhealth;
        object_placed = false;
        intersecting_objects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Object is intersecting with the to be placed object
    public void OnTriggerEnter(Collider other)
    {
        bool intersection = true;
        foreach(string tag in m_allowedtags)
        {
            if (other.gameObject.CompareTag(tag))
            {
                intersection = false;
                break;
            }
        }

        if (intersection)
        {
            intersecting_objects.Add(other.gameObject);
        }

    }

    //Remove objects that're not blocking anymore
    public void OnTriggerExit(Collider other)
    {
        GameObject obj = other.gameObject;
        
        for(int i = 0; i < intersecting_objects.Count; i++)
        {
            if(obj.GetInstanceID() == intersecting_objects[i].GetInstanceID())
            {
                intersecting_objects.RemoveAt(i);
                break;
            }
        }

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
        m_owner.m_Instance.GetComponent<PlayerConstruction>().removeObject(gameObject);
    }


    //Get ground clear
    public bool getGroundClear()
    {
        return intersecting_objects.Count == 0;
    }

    // Set owner of object (executed by the PlayerConstructon script)
    public void setOwner(int playernumber)
    {
        GameObject m_root = GameObject.FindWithTag("Gamemanager");
        GameManager m_gamemanager = m_root.GetComponent<GameManager>();
        m_owner = m_gamemanager.getUserManager().m_playerlist[playernumber];

    }

    //Sets the object type and allowed tags
    public void setObjectType(PlayerConstruction.PlayerObjectType object_type)
    {
        m_allowedtags = new List<string>(); //Create new list
        m_object_type = object_type;    //Set object type

        //Set the tags that are allowed to intersect
        switch (m_object_type)
        {
            case PlayerConstruction.PlayerObjectType.PlayerWall:
                m_allowedtags.Add("PlayerShell");
                m_allowedtags.Add("EnemyShell");
                m_allowedtags.Add("PlayerConstructionMarker");
                m_allowedtags.Add("PlayerTurret");
                m_allowedtags.Add("PlayerCarrotField");
                m_allowedtags.Add("PlayerMud"); 
                break;
            case PlayerConstruction.PlayerObjectType.PlayerTurret:
                m_allowedtags.Add("PlayerShell");
                m_allowedtags.Add("EnemyShell");
                m_allowedtags.Add("PlayerConstructionMarker");
                m_allowedtags.Add("PlayerWall");
                break;
            case PlayerConstruction.PlayerObjectType.PlayerCarrotField:
                m_allowedtags.Add("PlayerShell");
                m_allowedtags.Add("EnemyShell");
                m_allowedtags.Add("PlayerConstructionMarker");
                m_allowedtags.Add("PlayerWall");
                break;
            case PlayerConstruction.PlayerObjectType.PlayerMud:
                m_allowedtags.Add("PlayerShell");
                m_allowedtags.Add("EnemyShell");
                m_allowedtags.Add("PlayerConstructionMarker");
                m_allowedtags.Add("PlayerWall");
                break;
            default:
                break;
        }
    }

    //Setter of object_placed
    public void setPlacement(bool status)
    {
        object_placed = status;
    }

    //Get the object type
    public PlayerConstruction.PlayerObjectType getObjectType()
    {
        return m_object_type;
    }
}
