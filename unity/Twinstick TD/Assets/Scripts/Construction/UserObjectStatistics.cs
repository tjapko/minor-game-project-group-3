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
    public PlayerManager m_owner;   //Instantiated by the player in PlayerConstruction

    //Private variables
    private int m_health;           //Current health of the object
    private bool object_placed;
    private List<GameObject> intersecting_objects;


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

    // Set owner of object (executed by the PlayerConstructon script)
    public void setOwner(int playernumber)
    {
        GameObject m_root = GameObject.FindWithTag("Gamemanager");
        GameManager m_gamemanager = m_root.GetComponent<GameManager>();
        m_owner = m_gamemanager.getUserManager().m_playerlist[playernumber];

    }

    //Object is intersecting with the to be placed object
    public void OnTriggerEnter(Collider other)
    {
        intersecting_objects.Add(other.gameObject);

        /*
        //Only perform this when the object hasn't been placed
        if (!object_placed)
        {
            Debug.Log("Colliding");
            //Check tags
            if (other.gameObject.CompareTag("Player") ||
                other.gameObject.CompareTag("PlayerObject") ||
                other.gameObject.CompareTag("Enemy") ||
                other.gameObject.CompareTag("Base") ||
                other.gameObject.CompareTag("Wall1") ||
                other.gameObject.CompareTag("Wall2") ||
                other.gameObject.CompareTag("Wall3") ||
                other.gameObject.CompareTag("Wall4"))
            {
                
            }
        }
        */
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

    public void setPlacement(bool status)
    {
        object_placed = status;
    }
}
