using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainmenuCanvasScript : MonoBehaviour {

    //References
    [Header("References:")]
    public GameObject m_MainMenuPanel;  //Reference to the main menu panel
    public GameObject m_SettingsPanel;  //Reference to the settings panel
    public GameObject m_HelpmenuPanel;  //Reference to the help menu panel
    public GameObject m_creditsPanel;   //Reference to the credits panel
    public GameObject m_controlspanel;  //Reference to the controls panel
    public GameObject m_highscorepanel; //Reference to the high score panel
    public GameObject m_helpMenu;       //Reference to the help Menu (after clicking start)

    // Use this for initialization
    void Start () {
        //Initialize menus : Assume it's right
        //m_MainMenuPanel.SetActive(true);
        //m_SettingsPanel.SetActive(false);
        //m_HelpmenuPanel.SetActive(false);
        //m_creditsPanel.SetActive(false);
        //m_controlspanel.SetActive(false);
        //m_highscorepanel.SetActive(false);
        //m_helpMenu.SetActive(false);

    }
	
    //Start game function
    public void StartGame(int sceneindex)
    {
        //Option has been checked or ...
        if(PlayerPrefs.GetInt("option_helpscreen", 1) == 1)
        {
            m_MainMenuPanel.SetActive(false);
            m_SettingsPanel.SetActive(false);
            m_HelpmenuPanel.SetActive(false);
            m_creditsPanel.SetActive(false);
            m_controlspanel.SetActive(false);
            m_highscorepanel.SetActive(false);
            m_helpMenu.SetActive(true);
        } else
        {
            SceneManager.LoadScene(sceneindex);
        }
    }

    //Setter help menu
    public void setHelpscreen(GameObject gameobject)
    {
        Toggle toggle = gameobject.GetComponent<Toggle>();
        if(toggle != null)
        {
            PlayerPrefs.SetInt("option_helpscreen", toggle.isOn ? 0 : 1);
        }
    }
}
