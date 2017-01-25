using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// Class MainmenuSelecter
/// https://unity3d.com/learn/tutorials/topics/user-interface-ui/creating-main-menu
/// </summary>
public class MainmenuSelecter : MonoBehaviour {
    //Public variables
    public EventSystem eventSystem;     // Event system of UI
    public GameObject selectedObject;   // Selected object (button)
    private bool buttonSelected;        // Boolean if button has been selected
	private GameObject m_menu;  
	private GameObject m_HighScoreCanvas;
	private HSController m_HSController; 


    // Update is called once per frame
    void Update()
    {
        // Check for input of user
        // Not a single button has been selected yet
        if (Input.GetAxisRaw("Vertical_1") != 0 && buttonSelected == false)
        {
            eventSystem.SetSelectedGameObject(selectedObject);  //Select a button 
            buttonSelected = true;
        }
    }

    // When gameobject is disabled
    private void OnDisable()
    {
        buttonSelected = false;
    }

	public void showHighScore()
	{
		m_menu = gameObject.transform.GetChild (0).gameObject;
		m_HighScoreCanvas = gameObject.transform.GetChild (4).gameObject;
		m_HSController = m_HighScoreCanvas.GetComponent<HSController> ();
		m_menu.SetActive (false);
		m_HighScoreCanvas.SetActive (true);
		m_HSController.StartInitialization ();
	}

	public void Menu()
	{
		m_menu = gameObject.transform.GetChild(0).gameObject;
		m_HighScoreCanvas = gameObject.transform.GetChild(4).gameObject;
		m_menu.SetActive(true);
		m_HighScoreCanvas.SetActive (false);
	}
}


