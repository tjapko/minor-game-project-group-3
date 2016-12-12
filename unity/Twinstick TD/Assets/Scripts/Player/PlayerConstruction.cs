using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerConstruction : MonoBehaviour {

    //Public variables
    public GameObject m_markerprefab;   //Reference to the suggestive marker prefab
    public GameObject m_wallprefab;   //Reference to (to be placed) turret
    public GameObject m_turretprefab;   //Reference to (to be placed) turret
    public GameObject m_carrotfieldprefab;  //Reference to (to be placed) carrot field prefab   
    public List<GameObject> m_placedobjects;    //List containing all the objects the user has placed  
    public PlayerManager m_player;     //Reference to the player manager (set by Usermanager)

    public int m_price_wall = 10;
    public int m_price_turret = 100;
    public int m_price_carrot = 200;

    //Private variables
    private Vector3 mouseposition;      // Position of mouse
    private Transform suggestedpos;     // Suggested position to place gameobject
    private bool constructing;          // Boolean : boolean if player is constructing
    private bool constructionphase;     // Boolean : (true = construction phase, false = wavephase), set by game manager

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
                StartCoroutine(ObjectPlacement("1", m_turretprefab));
            }
            
        }

        // Check if player has clicked
        // Player is in construction phase, not already constructing and pressing the construction button
        if (constructionphase && !constructing && Input.GetKeyDown("2"))
        {
            //Check if player has enough funds
            if (m_player.m_stats.getCurrency() >= m_price_carrot)
            {
                constructing = true;
                StartCoroutine(ObjectPlacement("2", m_carrotfieldprefab));

            }
        }
    }


    //Function to place objects
    public IEnumerator ObjectPlacement(string keyinput, GameObject prefab)
    {
        //First instantiate the object
        GameObject newinstance = GameObject.Instantiate(prefab) as GameObject;
        UserObjectStatistics instancestats = newinstance.GetComponent<UserObjectStatistics>();
        instancestats.setOwner(m_player.m_PlayerNumber);    //Give a reference to the player

        //Create the suggestive markers
        List<GameObject> markers = setConstructionMarker(keyinput);

        //While we're in the construction phase
        while (constructionphase)
        {
            //Set the location of the object to the mouse position or the suggested position
            if((suggestedpos != null) && Vector3.Distance(mouseposition, suggestedpos.position) < 1.5)
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
                        newinstance.tag = "PlayerCarrotField";
                        m_player.m_stats.substractCurrency(m_price_carrot);
                        break;
                    case "3":
                        newinstance.tag = "PlayerTurret";
                        m_player.m_stats.substractCurrency(m_price_turret);
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

    // Set the markers
    private List<GameObject> setConstructionMarker(string marker_type)
    {
        //Create list of markers
        List<GameObject> markers = new List<GameObject>();  //List of markers (gameobject)
        List<Vector3> marker_pos = new List<Vector3>();     //List of marker positions (vector3)
        List<Vector3> surrounding_vector = setVector(marker_type);

        Debug.Log("List has objects:" + m_placedobjects.Count);
        foreach (GameObject existing_obj in m_placedobjects)
        {
            foreach(Vector3 vector in surrounding_vector)
            {
                Vector3 spawnpos = existing_obj.transform.position + vector;
                Debug.Log(existing_obj.transform.position);
                //If the lists does not contain the marker add the marker to the list of markers
                //And instantiate the marker
                if (!marker_pos.Contains(spawnpos))
                {
                    marker_pos.Add(spawnpos);   //Add the position of the marker to the list

                    //Create a marker
                    GameObject newmarker = GameObject.Instantiate(m_markerprefab, spawnpos, Quaternion.identity) as GameObject;
                    m_markerprefab.transform.position = spawnpos;
                    markers.Add(newmarker);
                }
            }
        }

        return markers;
    }

    private void destroyMarkers(List<GameObject> markers)
    {
        foreach(GameObject marker in markers)
        {
            Destroy(marker);
        }
    }

    private List<Vector3> setVector(string marker_type)
    {
        List<Vector3> vec = new List<Vector3>();
        //Return a list of markers surrounding the existing object
        float amount = 0;
        switch (marker_type)
        {
            case "1":
                amount = 1.1f;
                break;
            case "2":
                amount = 2.1f;
                break;
            case "3":
                amount = 2.1f;
                break;
            default:
                break;
        }
        vec.Add(new Vector3(0f, 0f,  amount));
        vec.Add(new Vector3(0f, 0f, -amount));
        vec.Add(new Vector3( amount, 0f, 0f));
        vec.Add(new Vector3(-amount, 0f, 0f));

        return vec;
    }
}
