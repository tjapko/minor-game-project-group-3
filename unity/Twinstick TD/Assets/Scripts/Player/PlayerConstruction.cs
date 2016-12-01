using UnityEngine;
using System.Collections;

public class PlayerConstruction : MonoBehaviour {

    //Public variables
    public GameObject m_objectprefab;   //Reference to (to be placed) object prefab (set by Usermanager)  

    //Private variables
    private PlayerManager m_player;     //Reference to the player manager (set by Usermanager)
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
            constructing = true;
            StartCoroutine(ObjectPlacement());
        }
    }

    //Function to place objects
    public IEnumerator ObjectPlacement()
    {
        //First instantiate the object
        GameObject newinstance = GameObject.Instantiate(m_objectprefab) as GameObject;

        //While we're in the construction phase
        while (constructionphase)
        {
            //Set the location of the object to the mouse position
            newinstance.transform.position = mouseposition;
            //Return next frame
            yield return null;

            //Exit 
            if (Input.GetKeyDown("1"))
            {
                constructing = false;
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

    //Sets the construction phase
    public void setconstructionphase(bool status)
    {
        constructionphase = status;
    }
}
