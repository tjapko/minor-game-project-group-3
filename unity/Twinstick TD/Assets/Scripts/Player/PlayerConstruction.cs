﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerConstruction : MonoBehaviour {

    //Public variables
    public GameObject m_markerprefab;   //Reference to the suggestive marker prefab
    public GameObject m_wallprefab;   //Reference to (to be placed) turret
    public GameObject m_turretprefab;   //Reference to (to be placed) turret
    public GameObject m_carrotfieldprefab;  //Reference to (to be placed) carrot field prefab
    public GameObject m_mudprefab;   
    public List<GameObject> m_placedobjects;    //List containing all the objects the user has placed  
    public PlayerManager m_player;     //Reference to the player manager (set by Usermanager)

    public int m_price_wall = 10;       //Price of wall
    public int m_price_turret = 100;    //Price of turret
    public int m_price_carrot = 200;    //Price of Carrot field
    public int m_price_mud = 200;       //Price of the mud field

    //Private variables
    private Vector3 mouseposition;      // Position of mouse
    private Transform suggestedpos;     // Suggested position to place gameobject
    private bool constructing;          // Boolean : boolean if player is constructing
    private bool constructionphase;     // Boolean : (true = construction phase, false = wavephase), set by game manager

    //Enum of playerobject
    public enum PlayerObjectType
    {
        PlayerWall,
        PlayerTurret,
        PlayerCarrotField,
        PlayerMud
    }

    // Use this for initialization
    void Start () {
        constructing = false;
        constructionphase = false;
    }
	
	// Update is called once per frame
	void Update () {

        // Get the location of the mouse in world coordinates
        mouseposition = GetComponent<PlayerMovement>().mouseposition;

        // Check if player has clicked
        // Player is in construction phase, not already constructing and pressing the construction button
        if(constructionphase && !constructing && Input.GetKeyDown("1"))
        {
            //Check if player has enough funds
            if(m_player.m_stats.getCurrency() >= m_price_wall)
            {
                constructing = true;
                StartCoroutine(ObjectPlacement("1", m_wallprefab, PlayerObjectType.PlayerWall));
            }
            
        }

        // Check if player has clicked
        // Player is in construction phase, not already constructing and pressing the construction button
        if (constructionphase && !constructing && Input.GetKeyDown("2"))
        {
            //Check if player has enough funds
            if (m_player.m_stats.getCurrency() >= m_price_turret)
            {
                constructing = true;
                StartCoroutine(ObjectPlacement("2", m_turretprefab, PlayerObjectType.PlayerTurret));

            }
        }

        // Check if player has clicked
        // Player is in construction phase, not already constructing and pressing the construction button
        if (constructionphase && !constructing && Input.GetKeyDown("3"))
        {
            //Check if player has enough funds
            if (m_player.m_stats.getCurrency() >= m_price_carrot)
            {
                constructing = true;
                StartCoroutine(ObjectPlacement("3", m_carrotfieldprefab, PlayerObjectType.PlayerCarrotField));

            }
        }

        // Check if player has clicked
        // Player is in construction phase, not already constructing and pressing the construction button
        if (constructionphase && !constructing && Input.GetKeyDown("4"))
        {
            //Check if player has enough funds
            if (m_player.m_stats.getCurrency() >= m_price_mud)
            {
                constructing = true;
                StartCoroutine(ObjectPlacement("4", m_mudprefab, PlayerObjectType.PlayerMud));

            }
        }
    }


    //Function to place objects
    public IEnumerator ObjectPlacement(string keyinput, GameObject prefab, PlayerObjectType objecttype)
    {
        //First instantiate the object
        GameObject newinstance = GameObject.Instantiate(prefab) as GameObject;
        UserObjectStatistics instancestats = newinstance.GetComponent<UserObjectStatistics>();
        instancestats.setOwner(m_player.m_PlayerNumber);    //Give a reference to the player
        instancestats.setObjectType(objecttype);

        //Create the suggestive markers
        List<GameObject> markers = setConstructionMarker(objecttype);

        //While we're in the construction phase
        while (constructionphase)
        {
            //Set the location of the object to the mouse position or the suggested position
            if((suggestedpos != null) && Vector3.Distance(mouseposition, suggestedpos.position) < 1.0)
            {
                newinstance.transform.position = suggestedpos.position;
            } else
            {
                newinstance.transform.position = mouseposition;
            }
            
            //Return next frame
            yield return null;

            //Exit 
            if (Input.GetKeyDown(keyinput) && instancestats.getGroundClear())
            {   
                instancestats.setPlacement(true);    //The object is now placed onto the ground
                newinstance.AddComponent<Rigidbody>();  //Create a rigid body, for OnTriggerEnter to work properly
                newinstance.GetComponent<Rigidbody>().isKinematic = true;   //Make the rigidbody kinematic, such that it's not affected by physics
                m_placedobjects.Add(newinstance);   //Add instance to list
                constructing = false;   //Building has finished

                //Reduce funds and set tag
                switch (keyinput)
                {
                    case "1":
                        newinstance.tag = "PlayerWall";
                        m_player.m_stats.substractCurrency(m_price_wall);
                        break;
                    case "2":
                        newinstance.tag = "PlayerTurret";
                        m_player.m_stats.substractCurrency(m_price_turret);
                        break;
                    case "3":
                        newinstance.tag = "PlayerCarrotField";
                        m_player.m_stats.substractCurrency(m_price_carrot);
                        break;
                    case "4":
                        newinstance.tag = "PlayerMud";
                        m_player.m_stats.substractCurrency(m_price_mud);
                        break;
                    default:
                        break;
                }
                break;
            }
        }

        //If player hasn't clicked (and construction timer has expired) remove the instance
        if (constructing)
        {
            constructing = false;
            GameObject.Destroy(newinstance);
        }

        destroyMarkers(markers);

    }

    //Function to remove objects
    public void removeObject(GameObject remove_object)
    {
        int remove_id = remove_object.GetInstanceID();
        for(int i = 0; i < m_placedobjects.Count; i++)
        {
            if(m_placedobjects[i].GetInstanceID()  == remove_id)
            {
                Destroy(m_placedobjects[i]);
                m_placedobjects.RemoveAt(i);
                break;
            }
        }
    }

    //Sets the construction phase
    public void setconstructionphase(bool status)
    {
        constructionphase = status;
    }

    //Get the reward of the farms
    public int getCarrots()
    {
        int amount = 0;
        foreach(GameObject placedobject in m_placedobjects)
        {
            if(placedobject.GetComponent<CarrotFieldScript>() != null)
            {
                amount += placedobject.GetComponent<CarrotFieldScript>().waveYield();
            }
        }
        return amount;
    }

    //Set the Suggested position to place the object
    public void setSuggestedPosition(Transform pos)
    {
        suggestedpos = pos;
    }

    //Remove constructions of player
    public void removePlayerConstructions()
    {
        int n = m_placedobjects.Count;
        for (int i = 0; i < n; i++)
        {
            Destroy(m_placedobjects[0]);
            m_placedobjects.RemoveAt(0);
        }
    }

    // Set the markers
    private List<GameObject> setConstructionMarker(PlayerObjectType new_object_type)
    {
        //Create list of markers
        List<GameObject> markers = new List<GameObject>();  //List of markers (gameobject)
        List<Vector3> marker_pos = new List<Vector3>();     //List of marker positions (vector3)

        foreach (GameObject existing_obj in m_placedobjects)
        {
            PlayerObjectType existing_obj_type = existing_obj.GetComponent<UserObjectStatistics>().getObjectType();
            List<Vector3> surrounding_vector = setVector(existing_obj_type, new_object_type);
            if(surrounding_vector == null)
            {
                continue;
            }

            foreach (Vector3 vector in surrounding_vector)
            {
                Vector3 spawnpos = existing_obj.transform.position + vector;
                //If the lists does not contain the marker add the marker to the list of markers
                //And instantiate the marker
                if (!marker_pos.Contains(spawnpos))
                {
                    marker_pos.Add(spawnpos);   //Add the position of the marker to the list

                    //Create a marker
                    GameObject newmarker = GameObject.Instantiate(m_markerprefab, spawnpos, Quaternion.identity) as GameObject;
                    m_markerprefab.transform.position = spawnpos;
                    newmarker.GetComponent<SuggestiveMarkerScript>().setMarker(new_object_type);
                    markers.Add(newmarker);
                }
            }
        }

        return markers;
    }

    //Destroy the markers
    private void destroyMarkers(List<GameObject> markers)
    {
        foreach(GameObject marker in markers)
        {
            Destroy(marker);
        }
    }

    //Create vectors for the markers
    private List<Vector3> setVector(PlayerObjectType existing_object_type, PlayerObjectType new_object_type)
    {
        List<Vector3> vec = null;
        //Return a list of markers surrounding the existing object
        float amount = 0;
        //Determines the distance between the existing object and the marker
        switch (existing_object_type)
        {   
            //Markers for a wall
            case PlayerObjectType.PlayerWall:
                amount = 1.1f;
                //Places the marker
                switch (new_object_type)
                {
                    //Placing a wall next to a  wall
                    case PlayerObjectType.PlayerWall:
                        vec = markerHorizontalVertical(amount);
                        break;
                    //Placing a carrot field next to a wall
                    case PlayerObjectType.PlayerCarrotField:
                        vec = markerDiagonal(amount);
                        break;
                    //Placing a turret next to a wall
                    case PlayerObjectType.PlayerTurret:
                        vec = markerHorizontalVertical(amount);
                        break;
                    //Placing mud next to a wall
                    case PlayerObjectType.PlayerMud:
                        vec = markerDiagonal(amount);
                        break;
                }
                break;
                //Placing a turret
            case PlayerObjectType.PlayerCarrotField:
                amount = 2.01f;
                //Places the marker
                switch (new_object_type)
                {
                    //Placing a wall next to a carrot field
                    case PlayerObjectType.PlayerWall:
                        vec = markerDiagonal(amount/2);
                        break;
                    //Placing a carrot field next to a carrot field
                    case PlayerObjectType.PlayerCarrotField:
                        vec = markerHorizontalVertical(amount);
                        break;
                    //Placing a turret next to a carrot field
                    case PlayerObjectType.PlayerTurret:
                        vec = markerHorizontalVertical(amount);
                        break;
                    //Placing mud next to a carrot field
                    case PlayerObjectType.PlayerMud:
                        vec = markerHorizontalVertical(amount);
                        break;
                }
                break;
            case PlayerObjectType.PlayerTurret:
                amount = 2.01f;
                //Places the marker
                switch (new_object_type)
                {
                    //Placing a wall next to a turret
                    case PlayerObjectType.PlayerWall:
                        vec = markerHorizontalVertical(amount/2);
                        break;
                    //Placing a carrot field next to a turret
                    case PlayerObjectType.PlayerCarrotField:
                        vec = markerHorizontalVertical(amount);
                        break;
                    //Placing a turret next to a turret
                    case PlayerObjectType.PlayerTurret:
                        vec = markerHorizontalVertical(amount);
                        break;
                    //Placing mud next to a turret
                    case PlayerObjectType.PlayerMud:
                        vec = markerHorizontalVertical(amount);
                        break;
                }
                break;
            case PlayerObjectType.PlayerMud:
                amount = 2.01f;
                //Places the marker
                switch (new_object_type)
                {
                    //Placing a wall next to mud
                    case PlayerObjectType.PlayerWall:
                        vec = markerDiagonal(amount / 2);
                        break;
                    //Placing a carrot field next to mud
                    case PlayerObjectType.PlayerCarrotField:
                        vec = markerHorizontalVertical(amount);
                        break;
                    //Placing a turret next to mud
                    case PlayerObjectType.PlayerTurret:
                        vec = markerHorizontalVertical(amount);
                        break;
                    //Placing mud next to mud
                    case PlayerObjectType.PlayerMud:
                        vec = markerHorizontalVertical(amount);
                        break;
                }
                break;
        }
        
        return vec;
    }

    //Diagonal markers
    private List<Vector3> markerDiagonal(float amount)
    {
        List<Vector3> ans = new List<Vector3>();
        ans.Add(new Vector3(amount, 0f, amount));
        ans.Add(new Vector3(amount, 0f, -amount));
        ans.Add(new Vector3(-amount, 0f, amount));
        ans.Add(new Vector3(-amount, 0f, -amount));
        return ans;
    }

    //Vertical markers
    private List<Vector3> markerHorizontalVertical(float amount)
    {
        List<Vector3> ans = new List<Vector3>();
        ans.Add(new Vector3(0f, 0f, amount));
        ans.Add(new Vector3(0f, 0f, -amount));
        ans.Add(new Vector3(amount, 0f, 0f));
        ans.Add(new Vector3(-amount, 0f, 0f));
        return ans;
    }
}
