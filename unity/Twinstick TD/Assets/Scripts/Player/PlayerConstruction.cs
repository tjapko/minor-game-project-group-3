using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerConstruction : MonoBehaviour {

    //Start price objects
    public static int m_price_wall = 100;      //Price of wall
	public static int m_price_turret = 1000;   //Price of turret
	public static int m_price_carrot = 2500;   //Price of Carrot field
	public static int m_price_mud = 500;       //Price of the mud field

    //Fix
    const string build_1 = "1";         //Button to place walls
    const string build_2 = "2";         //Button to place turrets
    const string build_3 = "3";         //Button to place farms
    const string build_4 = "4";         //Button to place mud

    //References
    private GameManager m_gamemanager;          //Reference to game manager script
    private BaseManager m_basemanager;          //Reference to base manager
    private PlayerMovement m_playerMovement;    //Reference to player movement script
    private PlayerStatistics m_playerStats;     //Reference to player stats script
    private GridManager m_gridmanager;          //Reference to grid manager
    private Grid m_grid;                        //Reference to grid in grid manager
    private Camera m_constructionCamera;        //Reference to the construction camera
    private int Floor;                          //Reference to floor layer


    //Public variables
    public GameObject m_markerprefab;   //Reference to the suggestive marker prefab
    public GameObject m_wallprefab;   //Reference to (to be placed) turret
    public GameObject m_turretprefab;   //Reference to (to be placed) turret
    public GameObject m_carrotfieldprefab;  //Reference to (to be placed) carrot field prefab
    public GameObject m_mudprefab;   
    public List<GameObject> m_placedobjects;    //List containing all the objects the user has placed  
    public PlayerManager m_player;     //Reference to the player manager (set by Usermanager)

    // turret variables
    public static int maxTurrets = 3; // maximum amount of turrets
    public int  currentTurrets=0; // current amount of active turrets 

    //Private variables
    private Vector3 mouseposition;      // Position of mouse
    private bool constructing;          // Boolean : boolean if player is constructing
    private bool constructionphase;     // Boolean : (true = construction phase, false = wavephase), set by game manager
    private float camRayLength = 100f;

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
        m_basemanager = m_gamemanager.getBaseManager();
        m_playerMovement = GetComponent<PlayerMovement>();
        m_playerStats = GetComponent<PlayerStatistics>();
        m_constructionCamera = GameObject.FindWithTag("constructionCam").GetComponent<Camera>();
        Floor = LayerMask.GetMask("mouseFloor");

        //Set variables
        constructing = false;
        constructionphase = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (constructionphase && !constructing && m_basemanager.m_Instance.activeSelf)
        {
            // Check if player has clicked
            // Player is in construction phase, not already constructing and pressing the construction button
            if (Input.GetKeyDown(build_1))
            {
                //Check if player has enough funds
                if (m_playerStats.getCurrency() >= m_price_wall)
                {
                    constructing = true;
                    StartCoroutine(ObjectPlacement(build_1, m_wallprefab, PlayerObjectType.PlayerWall));
                }

            }

            // Check if player has clicked
            // Player is in construction phase, not already constructing and pressing the construction button
            if (Input.GetKeyDown(build_2))
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
            if (Input.GetKeyDown(build_3))
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
            if (Input.GetKeyDown(build_4))
            {
                //Check if player has enough funds
                if (m_playerStats.getCurrency() >= m_price_mud)
                {
                    constructing = true;
                    StartCoroutine(ObjectPlacement(build_4, m_mudprefab, PlayerObjectType.PlayerMud));
                }
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
        //List<GameObject> markers = setConstructionMarker(objecttype);

        //While we're in the construction phase
        while (constructionphase)
        {
            //Check mouse position
            mouseposition = mousePos();

            //Place the instance onto a node
            Node selected_node = m_grid.NodeFromWorldPoint(mouseposition);
            newinstance.transform.position = selected_node.worldPosition;
            instancestats.setMesh(selected_node.walkable);

            //Return next frame
            yield return null;

            //Exit
            //Check if button has been pressed
            //Check if player isn't standing onto the object
            //Check if node is walkable
            if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(keyinput) && (instancestats.getGroundClear() && m_grid.NodeFromWorldPoint(newinstance.transform.position).walkable))
            {
                instancestats.setPlacement(true);    //The object is now placed onto the ground
                //newinstance.AddComponent<Rigidbody>();  //Create a rigid body, for OnTriggerEnter to work properly
                newinstance.GetComponent<Rigidbody>().isKinematic = true;   //Make the rigidbody kinematic, such that it's not affected by physics
                setTag(objecttype, newinstance);    //Set tag onto game object
				newinstance.GetComponent<UserObjectStatistics>().m_owner = m_player;
				m_placedobjects.Add(newinstance);   //Add instance to list
                constructing = false;   //Building has finished
               	newinstance.layer = 15;  //Set layer of the object as unwalkable

                //Reduce funds
                reduceFunds(objecttype, countObjects(objecttype));
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
		if (remove_object != null) {
			int remove_id = remove_object.GetInstanceID ();
			for (int i = 0; i < m_placedobjects.Count; i++) {
				if (m_placedobjects [i].GetInstanceID () == remove_id) {
					Destroy (m_placedobjects [i]);
					m_placedobjects.RemoveAt (i);
					break;
				}
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

    //Destroy the markers
    private void destroyMarkers(List<GameObject> markers)
    {
        foreach(GameObject marker in markers)
        {
            Destroy(marker);
        }
    }

    //Getter for boolean constructing: player is constucting
    public bool getPlayerisConstructing()
    {
        return constructing;
    }

    // Returns the price of a new piece of wall
    private void reduceFunds(PlayerObjectType objecttype, int exisiting_walls)
    {
        float factor = 1.5f + (0.1f * (float)exisiting_walls);

        switch (objecttype)
        {
            case PlayerObjectType.PlayerWall:
                m_player.m_stats.substractCurrency(m_price_wall);
                m_price_wall = (int)((float) m_price_wall * factor);
                break;
            case PlayerObjectType.PlayerTurret:
                m_player.m_stats.substractCurrency(m_price_turret);
                m_price_turret = (int)((float)m_price_turret * factor);
                break;
            case PlayerObjectType.PlayerCarrotField:
                m_player.m_stats.substractCurrency(m_price_carrot);
                m_price_carrot = (int)((float)m_price_carrot * factor);
                break;
            case PlayerObjectType.PlayerMud:
                m_player.m_stats.substractCurrency(m_price_mud);
                m_price_mud = (int)((float)m_price_mud * factor);
                break;
            default:
                break;

        }
    }

    //Counts amount of exisiting PlayerObjects
    private int countObjects(PlayerObjectType objecttype)
    {
        int ans = 0;
        foreach(GameObject obj in m_placedobjects)
        {
            UserObjectStatistics stats = obj.GetComponent<UserObjectStatistics>();
            if(stats.getObjectType() == objecttype)
            {
                ans++;
            }
        }
        return ans;
    }

    private void setTag(PlayerObjectType obj_type, GameObject obj)
    {
        switch (obj_type)
        {
            case PlayerObjectType.PlayerWall:
                obj.tag = "PlayerWall";
                break;
            case PlayerObjectType.PlayerTurret:
                obj.tag = "PlayerTurret";
                break;
            case PlayerObjectType.PlayerCarrotField:
                obj.tag = "PlayerCarrotField";
                break;
            case PlayerObjectType.PlayerMud:
                obj.tag = "PlayerMud";
                break;
            default:
                break;

        }
    }

    // Adjust the rotation of the player based on the mousePosition input.
    private Vector3 mousePos()
    {
        // creating a ray from the camera to the mouseposition
        Ray camRay = m_constructionCamera.ScreenPointToRay(Input.mousePosition);

        // a variable, which is true when the ray hits the floor 
        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLength, Floor)) // checking if the ray hits the floor 
        {
            Vector3 playerToMouse = floorHit.point - transform.position; // creating a vector from the player to the mousePosition
            playerToMouse.y = 0f; // because it's a projection on the x-z-plane
            playerToMouse = transform.position + playerToMouse;  //Used for dropping objects
            return playerToMouse;
            //Line from Main Camera to the point selected with the mouse (for debugging purposes)
            //Debug.DrawLine(m_constructionCamera.transform.position, floorHit.point, Color.yellow);
        }
        return Vector3.zero;
    }
}
