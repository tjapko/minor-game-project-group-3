using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerConstruction : MonoBehaviour {

    //Public variables
    public GameObject m_turretprefab;   //Reference to (to be placed) turret
    public GameObject m_carrotfieldprefab;  //Reference to (to be placed) carrot field prefab   
    public List<GameObject> m_placedobjects;    //List containing all the objects the user has placed  
    public PlayerManager m_player;     //Reference to the player manager (set by Usermanager)

    public int m_price_wall = 200;
    public int m_price_turret = 200;
    public int m_price_carrot = 200;

    //Private variables
    Vector3 mouseposition;              // Position of mouse
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

        //While we're in the construction phase
        while (constructionphase)
        {
            //Set the location of the object to the mouse position
            newinstance.transform.position = mouseposition;
            //Return next frame
            yield return null;

            //Exit 
            if (Input.GetKeyDown(keyinput) && instancestats.getGroundClear())
            {   
                instancestats.setPlacement(true);    //The object is now placed onto the ground
                instancestats.setOwner(m_player.m_PlayerNumber);    //Give a reference to the player
                newinstance.AddComponent<Rigidbody>();  //Create a rigid body, for OnTriggerEnter to work properly
                newinstance.GetComponent<Rigidbody>().isKinematic = true;   //Make the rigidbody kinematic, such that it's not affected by physics
                m_placedobjects.Add(newinstance);   //Add instance to list
                constructing = false;   //Building has finished

                //Reduce funds
                switch (keyinput)
                {
                    case "1":
                        m_player.m_stats.substractCurrency(m_price_wall);
                        break;
                    case "2":
                        m_player.m_stats.substractCurrency(m_price_carrot);
                        break;
                    case "3":
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
}
