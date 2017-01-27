using UnityEngine;
using System.Collections;

/// <summary>
/// UI Manager
/// </summary>
public class UIManagerV2 : MonoBehaviour
{

    //Prefabs
    [Header("Prefabs:")]
    public GameObject m_CanvasConstructionPrefab;//GameObject Canvas Construction
    public GameObject m_CanvasGameOverPrefab;   //GameObject Canvas Game Over
    public GameObject m_CanvasPauseMenuPrefab;  //GameObject Canvas Pause Menu
    public GameObject m_CanvasPlayerUIPrefab;   //GameObject Canas Player UI

    //References
    [HideInInspector] public GameObject go_CanvasConstruction;//GameObject Canvas Construction
    [HideInInspector] public GameObject go_CanvasGameOver;    //GameObject Canvas Game Over
    [HideInInspector] public GameObject go_CanvasPauseMenu;   //GameObject Canvas Pause Menu
    [HideInInspector] public GameObject go_CanvasPlayerUI;    //GameObject Canas Player UI
    [HideInInspector] public GameObject go_Shop;              //GameObject Shop
    [HideInInspector] public GameObject go_Base;              //GameObject Shop

    private GameManager m_gamemanager;  //Reference to GameManager
    private UserManager m_usermanager;  //Reference to UserManager
    private CanvasConstructionScriptV2 m_ConstructionScript;  //Reference to CanvasConstructionScriptV2
    private CanvasGameOverScript m_GameOverScript;      //Reference to CanvasGameOverScript
    private CanvasPauseMenuScript m_PauseMenuScript;    //Reference to CanvasPauseMenuScript
    private CanvasPlayerUIScriptV2 m_PlayerUIScript;      //Reference to CanvasPlayerUIScriptV2


    //Constructer
    public void StartInitialization()
    {
        //Find Game manager
        m_gamemanager = GameObject.FindWithTag("Gamemanager").GetComponent<GameManager>();
        m_usermanager = m_gamemanager.getUserManager();

        //Instantiate and set references to canvasses (and shop)
        //GameObject.Instantiate(m_CanvasHelpScreen, Vector3.zero, Quaternion.identity);
        go_CanvasConstruction = GameObject.Instantiate(m_CanvasConstructionPrefab) as GameObject;
        go_CanvasGameOver = GameObject.Instantiate(m_CanvasGameOverPrefab) as GameObject;
        go_CanvasPauseMenu = GameObject.Instantiate(m_CanvasPauseMenuPrefab) as GameObject;
        go_CanvasPlayerUI = GameObject.Instantiate(m_CanvasPlayerUIPrefab) as GameObject;
        go_Shop = GameObject.FindWithTag("Shop");
        go_Base = m_gamemanager.getBaseManager().m_Instance;

        //Get scripts of canvasses
        m_ConstructionScript = go_CanvasConstruction.GetComponent<CanvasConstructionScriptV2>();
        m_GameOverScript = go_CanvasGameOver.GetComponent<CanvasGameOverScript>();
        m_PauseMenuScript = go_CanvasPauseMenu.GetComponent<CanvasPauseMenuScript>();
        m_PlayerUIScript = go_CanvasPlayerUI.GetComponent<CanvasPlayerUIScriptV2>();

        //Initialize Canvas
        m_ConstructionScript.StartInitialization();
        m_GameOverScript.StartInitialization();
        m_PauseMenuScript.StartInitialization();
        m_PlayerUIScript.StartInitialization();

        //Find shop and set variable
        ShopScriptV2 m_shopscript = go_Shop.GetComponent<ShopScriptV2>();
        m_shopscript.Start();

        //Find base and set reference

        //Set visibility of canvas
        go_CanvasConstruction.SetActive(false);
        go_CanvasGameOver.SetActive(false);
        go_CanvasPauseMenu.SetActive(false);
        go_CanvasPlayerUI.SetActive(true);

    }

    // Changing UI back and fourth between phases
    // Wavephase = true : wavephase, Wavephase = false : build phase
    // First check gameover -> game is paused -> phase of game
    public void UIchange(bool gameover, bool wavephase, bool pause)
    {
        //Check for gameover
        if (gameover)
        {
            go_CanvasGameOver.SetActive(true);
            go_CanvasConstruction.SetActive(false);
            go_CanvasPauseMenu.SetActive(false);
            go_CanvasPlayerUI.SetActive(false);
        }
        else
        {
            //Check for pause
            if (pause)
            {
                go_CanvasGameOver.SetActive(false);
                go_CanvasConstruction.SetActive(false);
                go_CanvasPauseMenu.SetActive(true);
                go_CanvasPlayerUI.SetActive(false);
            }
            else
            {
                //Check wavephase
                if (wavephase)
                {
                    go_CanvasGameOver.SetActive(false);
                    go_CanvasConstruction.SetActive(false);
                    go_CanvasPauseMenu.SetActive(false);
                    go_CanvasPlayerUI.SetActive(true);

                }
                else
                {
                    go_CanvasGameOver.SetActive(false);
                    go_CanvasConstruction.SetActive(true);
                    go_CanvasPauseMenu.SetActive(false);
                    go_CanvasPlayerUI.SetActive(false);
                }
            }
        }

    }

    //public IEnumerator showWaveStatsUI()
    //{
    //    yield return StartCoroutine(m_PlayerUIScript.showWaveStatsUI());
    //}

    //Set score in game over canvas
    public void setScore()
    {
        m_GameOverScript.setScore();
    }

    //Show reward in playerUi
    public void showWaveReward()
    {
        StartCoroutine(m_ConstructionScript.showWaveReward());
    }

    //Set wavenumber in player ui
    public void setWaveNumber()
    {
        m_PlayerUIScript.setWaveNumber();
    }

}
