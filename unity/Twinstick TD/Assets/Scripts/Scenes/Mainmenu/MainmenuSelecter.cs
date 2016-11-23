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

    // Update is called once per frame
    void Update()
    {
        // Check for input of user
        // Not a single button has been selected yet
        if (Input.GetAxisRaw("Vertical") != 0 && buttonSelected == false)
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
}
