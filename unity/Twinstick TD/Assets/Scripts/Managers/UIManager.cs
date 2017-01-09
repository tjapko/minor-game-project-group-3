using UnityEngine;
using System.Collections;

/// <summary>
/// UI Manager
/// </summary>
public class UIManager {

    //References
    public GameObject go_mapUI;     //Reference to mapUI (gameobject)
    public GameObject go_mapShopUI; //Reference to shopUI (gameobject)
    
    private GameManager m_gamemanager;  //Reference to GameManager
    private UserManager m_usermanager;  //Reference to UserManager
    private MapUIScript m_mapui;        //Reference to MapUIScript
    private ShopUIScript m_shopUI;      //Referene to ShopUIScript


    //Constructer
    public UIManager(GameManager gamemanager, GameObject mapuiPrefab, GameObject mapshopuiPrefab)
    {
        m_gamemanager = gamemanager;
        m_usermanager = m_gamemanager.getUserManager();

        go_mapUI = GameObject.Instantiate(mapuiPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        go_mapShopUI = GameObject.Instantiate(mapshopuiPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        m_mapui = go_mapUI.GetComponent<MapUIScript>();
        m_shopUI = go_mapShopUI.GetComponent<ShopUIScript>();

        ShopScript m_shopscript = GameObject.FindWithTag("Shop").GetComponent<ShopScript>();
        m_shopscript.m_instance_UI = go_mapShopUI;

    }

    // Changing UI back and fourth between phases
    // Wavephase = true : wavephase, Wavephase = false : build phase
    // First check gameover -> game is paused -> phase of game
    public void UIchange(bool gameover, bool wavephase, bool pause)
    {
        //Check for gameover
        if (gameover)
        {
            m_mapui.showGameoverMenu(true);
            m_mapui.showConstructonPanel(false);
            m_mapui.showWaveControl(false);
            m_mapui.showPauseMenu(false);
        }
        else
        {
            //Check for pause
            if (pause)
            {
                m_mapui.showConstructonPanel(false);
                m_mapui.showWaveControl(false);
                m_mapui.showPauseMenu(true);
                m_mapui.showGameoverMenu(false);
            }
            else
            {
                //Check wavephase
                if (wavephase)
                {
                    m_mapui.showWaveControl(true);
                    m_mapui.showConstructonPanel(false);
                    m_mapui.showPauseMenu(false);
                    m_mapui.showGameoverMenu(false);
                }
                else
                {
                    m_mapui.showWaveControl(true);
                    m_mapui.showConstructonPanel(true);
                    m_mapui.showPauseMenu(false);
                    m_mapui.showGameoverMenu(false);
                }
            }
        }

    }

    public IEnumerator showWaveStatsUI()
    {
        yield return m_mapui.showWaveStatsUI();
    }

    public void setScore()
    {
        m_mapui.setScore();
    }
   

}
