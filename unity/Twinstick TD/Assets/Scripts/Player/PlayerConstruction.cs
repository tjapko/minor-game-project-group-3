using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerConstruction : MonoBehaviour {

    //Fix
    const string build_1 = "1";
    const string build_2 = "2";
    const string build_3 = "3";
    const string build_4 = "4";

    //References
    private GameManager m_gamemanager;          //Reference to game manager script
    private PlayerMovement m_playerMovement;    //Reference to player movement script
    private PlayerStatistics m_playerStats;     //Reference to player stats script
    private GridManager m_gridmanager;          //Reference to grid manager
    private Grid m_grid;                        //Reference to grid in grid manager

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
        //Set referenes
        m_gamemanager = GameObject.FindWithTag("Gamemanager").GetComponent<GameManager>();
        m_playerMovement = GetComponent<PlayerMovement>();
        m_playerStats = GetComponent<PlayerStatistics>();

        //Set variables
        constructing = false;
        constructionphase = false;
    }
	
	// Update is called once per frame
	void Update () {

        // Get the location of the mouse in world coordinates
        if (constructing)
        {
            mouseposition = m_playerMovement.mouseposition;
        }

        // Check if player has clicked
        // Player is in construction phase, not already constructing and pressing the construction button
        if(Input.GetKeyDown(build_1) && constructionphase && !constructing)
        {
            //Check if player has enough funds
            if(m_playerStats.getCurrency() >= m_price_wall)
            {
                constructing = true;
                StartCoroutine(ObjectPlacement(build_1, m_wallprefab, PlayerObjectType.PlayerWall));
            }
            
        }

        // Check if player has clicked
        // Player is in construction phase, not already constructing and pressing the construction button
        if (Input.GetKeyDown(build_2) && constructionphase && !constructing)
        {
            //Check if player has enough funds
            if (m_playerStats.getCurrency() >= m_price_turret)
            {
                constructing = true;
                StartCoroutine(ObjectPlacement(build_2, m_turretprefab, PlayerObjectType.PlayerTurret));
            }
        }

        // Check if player has clicked
        // Player is in construction phase, not already constructing and pressing the construction button
        if (Input.GetKeyDown(build_3) && constructionphase && !constructing)
        {
            //Check if player has enough funds
            if (m_playerStats.getCurrency() >= m_price_carrot)
            {
                constructing = true;
                StartCoroutine(ObjectPlacement(build_3, m_carrotfieldprefab, PlayerObjectType.PlayerCarrotField));
            }
        }

        // Check if player has clicked
        // Player is in construction phase, not already constructing and pressing the construction button
        if (Input.GetKeyDown(build_4) && constructionphase && !constructing)
        {
            //Check if player has enough funds
            if (m_playerStats.getCurrency() >= m_price_mud)
            {
                constructing = true;
                StartCoroutine(ObjectPlacement(build_4, m_mudprefab, PlayerObjectType.PlayerMud));
            }
        }
        
    }


    //Function to place objects
    public IEnumerator ObjectPlacement(string keyinput, GameObject prefab, PlayerObjectType objecttype)
    {
        //First instantiate the object
        GameObject newinstance = GameObject.Instantiate(prefab) as GameObject;  //Create object
        UserObjectStatistics instancestats = newinstance.GetComponent<UserObjectStatistics>();  //Get stats script
        instancestats.setOwner(m_player.m_PlayerNumber);    //Give a reference to the player
        instancestats.setObjectType(objecttype);            //Set the object type
        newinstance.layer = 10; //Set layer of new instance such that the object doesnt collide with he player

        //Create a new grid
        m_gridmanager = new GridManager(m_gamemanager.m_gridPrefab);
        m_grid = m_gridmanager.m_grid;

        //Create the suggestive markers
        List<GameObject> markers = setConstructionMarker(objecttype);

        //While we're in the construction phase
        while (constructionphase)
        {
            //Place the instance onto a node
            Node selected_node = m_grid.NodeFromWorldPoint(mouseposition);
            newinstance.transform.position = selected_node.worldPosition;
            instancestats.setMesh(selected_node.walkable);


            //Return next frame
            yield return null;

            //Exit 
            if (Input.GetKeyDown(keyinput) && instancestats.getGroundClear() && m_grid.NodeFromWorldPoint(newinstance.transform.position).walkable)
            {
                instancestats.setPlacement(true);    //The object is now placed onto the ground
                newinstance.AddComponent<Rigidbody>();  //Create a rigid body, for OnTriggerEnter to work properly
                newinstance.GetComponent<Rigidbody>().isKinematic = true;   //Make the rigidbody kinematic, such that it's not affected by physics
                m_placedobjects.Add(newinstance);   //Add instance to list
                constructing = false;   //Building has finished
                newinstance.layer = 9;  //Set layer of the object as unwalkable

                //Reduce funds and set tag
                switch (keyinput)
                {
                    case build_1:
                        newinstance.tag = "PlayerWall";
                        m_player.m_stats.substractCurrency(m_price_wall);
                        break;
                    case build_2:
                        newinstance.tag = "PlayerTurret";
                        m_player.m_stats.substractCurrency(m_price_turret);
                        break;
                    case build_3:
                        newinstance.tag = "PlayerCarrotField";
                        m_player.m_stats.substractCurrency(m_price_carrot);
                        break;
                    case build_4:
                        newinstance.tag = "PlayerMud";
                        m_player.m_stats.substractCurrency(m_price_mud);
                        break;
                    default:
                        break;
                }
                break;
            }

            //Escape button
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                constructing = true;
                break;
            }
        }

        //If player hasn't clicked (and construction timer has expired) remove the instance
        if (constructing)
        {
            constructing = false;
            GameObject.Destroy(newinstance);
        }
        //destroyMarkers(markers);

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
            CarrotFieldScript carrot_script = placedobject.GetComponent<CarrotFieldScript>();
            if (carrot_script != null)
            {
                amount += carrot_script.waveYield();
            }
        }
        return amount;
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
    {/*
        //Create list of markers
        List<GameObject> markers = new List<GameObject>();  //List of markers (gameobject)

        //Load grid and path

        m_gridmanager = new GridManager(gamemanager.m_gridPrefab);
        m_grid = m_gridmanager.m_grid;

        //Node[,] grid = m_grid.getGrid();
        //int length = grid.Length;
        //Debug.Log(length);

        //GameObject.Destroy(m_gridmanager.m_instance);
        */
        return new List<GameObject>();
        /*
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
        */
    }

    //Destroy the markers
    private void destroyMarkers(List<GameObject> markers)
    {
        foreach(GameObject marker in markers)
        {
            Destroy(marker);
        }
    }

    //Check if position is walkable()
    private Vector3 placeObject(Vector3 position)
    {
        GridManager m_gridmanager = new GridManager(m_gamemanager.m_gridPrefab);
        Grid m_grid = m_gridmanager.m_grid;
        Node node = m_grid.NodeFromWorldPoint(position);

        return node.walkable?node.worldPosition:Vector3.zero;
    }

    //Getter for boolean constructing: player is constucting
    public bool getPlayerisConstructing()
    {
        return constructing;
    }
}
