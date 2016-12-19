﻿using UnityEngine;
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
    private List<GameObject> colliding_objects;  //List of objects colliding with this object
    private List<GameObject> colliding_markers;  //List of markers colliding with this object
    private List<string> m_allowedtags;  //Tags of objects that are allowed to intersect

    // Use this for initialization
    void Start()
    {
        m_health = m_maxhealth;
        object_placed = false;
        colliding_objects = new List<GameObject>();
        colliding_markers = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Object is intersecting with the to be placed object
    public void OnTriggerEnter(Collider other)
    {
        //Check if this object is colliding with another object
        checkEnterExistingObject(other);

        //Check for markers
        checkEnterMarkers(other);

    }

    //Remove objects that're not blocking anymore
    public void OnTriggerExit(Collider other)
    {
        //Check for objects leaving this object
        checkExitExistingObject(other);

        //Check for markers leaving this object
        checkExitMarkers(other);

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
        return colliding_objects.Count == 0;
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

    //OnTriggerEnter check for illegal tags
    private void checkEnterExistingObject(Collider other)
    {
        bool collide = true;
        foreach (string tag in m_allowedtags)
        {
            if (other.gameObject.CompareTag(tag))
            {
                collide = false;
                break;
            }
        }

        if (collide)
        {
            colliding_objects.Add(other.gameObject);
        }
    }

    //OnTriggerEnter check for markers
    private void checkEnterMarkers(Collider other)
    {
        GameObject new_marker = other.gameObject;
        if (other.gameObject.CompareTag("PlayerConstructionMarker"))
        {
            bool unique = true;
            foreach(GameObject marker in colliding_markers)
            {
                if(new_marker.GetInstanceID() == marker.GetInstanceID())
                {
                    unique = false;
                    break;
                }
            }

            if (unique)
            {
                colliding_markers.Add(new_marker);
            }
        }
    }

    //OnTriggerExit check for illegal tags
    private void checkExitExistingObject(Collider other)
    {
        GameObject obj = other.gameObject;

        for (int i = 0; i < colliding_objects.Count; i++)
        {
            if (obj.GetInstanceID() == colliding_objects[i].GetInstanceID())
            {
                colliding_objects.RemoveAt(i);
                break;
            }
        }
    }

    //OnTriggerExit check for markers
    private void checkExitMarkers(Collider other)
    {
        GameObject obj = other.gameObject;

        for (int i = 0; i < colliding_markers.Count; i++)
        {
            if (obj.GetInstanceID() == colliding_markers[i].GetInstanceID())
            {
                colliding_markers.RemoveAt(i);
                break;
            }
        }
    }

    //Getter for markers
    public List<GameObject> getMarkers()
    {
        return colliding_markers != null? colliding_markers:null;
    }
}