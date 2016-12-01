using UnityEngine;
using System.Collections;

/// <summary>
/// Class ObjectplacementManager
/// Has functions to take care of placing objects into the world
/// </summary>
public class ObjectplacementManager
{
    //Public variables
    public GameObject m_objectprefab;   //Reference to (to be placed) object prefab

    private UserManager m_usermanager;  //Reference to the user manager
    private bool constructionphase;     //Boolean if game is in construction phase
    private bool playerhasclicked;      //Boolean if player has clicked
    
    //Constructer
    public ObjectplacementManager(UserManager usermanager, GameObject objectprefab)
    {
        m_usermanager = usermanager;
        m_objectprefab = objectprefab;
        constructionphase = false;
        playerhasclicked = false;
    }

    //Function to place objects
    public IEnumerator ObjectPlacement()
    {
        //First instantiate the object
        GameObject newinstance = GameObject.Instantiate(m_objectprefab) as GameObject;

        //While we're in the construction phase
        while(constructionphase)
        {
            //get the location of the mouse in world coordinates
            Vector3 mouseposition = m_usermanager.m_playerlist[0].m_movement.mouseposition;
            //Set the location of the object to the mouse position
            newinstance.transform.position = mouseposition;
            //Return next frame
            yield return null;

            //Exit 
            if(playerhasclicked)
            {
                break;
            }
        }

        //If player hasn't clicked (and construction timer has expired)
        //Remove the instance
        if (!playerhasclicked)
        {
            GameObject.Destroy(newinstance);
        }

    }

    //Sets the construction phase
    public void setconstructionphase(bool status)
    {
        constructionphase = status;
    }

    //Detects if player has clicked the button
    public void setplayerclick(bool status)
    {
        playerhasclicked = status;
    }


}
