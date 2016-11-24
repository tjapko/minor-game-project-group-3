using UnityEngine;
using System.Collections;

/// <summary>
/// Class UI script
/// Functions for the GameManager to implement UI 
/// </summary>
public class MapUIScript : MonoBehaviour {
    private GameObject m_pausemenu;
    private GameObject m_wavecontrol;
    private GameObject m_constructionpanel;

    // Initialization
    void Start()
    {
        m_pausemenu = gameObject.transform.Find("PauseMenu").gameObject;
        m_wavecontrol = gameObject.transform.Find("WaveControl").gameObject;
        m_constructionpanel = gameObject.transform.Find("ConstructionPanel").gameObject;

        m_pausemenu.SetActive(false);
        m_wavecontrol.SetActive(false);
        m_constructionpanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Show Pause menu
    public void showPauseMenu()
    {
        m_pausemenu.SetActive(true);
    }

    // Show Pause menu
    public void hidePauseMenu()
    {
        m_pausemenu.SetActive(false);
    }

    // Show Pause menu
    public void showWaveControl()
    {
        m_wavecontrol.SetActive(true);
    }

    // Show Pause menu
    public void hideWaveControl()
    {
        m_wavecontrol.SetActive(false);
    }

    // Show Pause menu
    public void showConstructonPanel()
    {
        m_constructionpanel.SetActive(true);
    }

    // Show Pause menu
    public void hideConstructonPanel()
    {
        m_constructionpanel.SetActive(false);
    }

}
