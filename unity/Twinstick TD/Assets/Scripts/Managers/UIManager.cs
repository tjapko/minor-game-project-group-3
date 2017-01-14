﻿using UnityEngine;
using System.Collections;

/// <summary>
/// UI Manager
/// </summary>
public class UIManager : MonoBehaviour {

    //Prefabs
    public GameObject m_CanvasGameOverPrefab;   //GameObject Canvas Game Over
    public GameObject m_CanvasHelpScreen;       //GameObject Canvas Help Screen
    public GameObject m_CanvasPauseMenuPrefab;  //GameObject Canvas Pause Menu
    public GameObject m_CanvasPlayerUIPrefab;   //GameObject Canas Player UI
    public GameObject m_CanvasShopPrefab;       //GameObject Canvas Shop

    //References
    [HideInInspector] public GameObject go_CanvasGameOver;    //GameObject Canvas Game Over
    [HideInInspector] public GameObject go_CanvasPauseMenu;   //GameObject Canvas Pause Menu
    [HideInInspector] public GameObject go_CanvasPlayerUI;    //GameObject Canas Player UI
    [HideInInspector] public GameObject go_CanvasShop;        //GameObject Canvas Shop
    [HideInInspector] public GameObject go_Shop;              //GameObject Shop

    private GameManager m_gamemanager;  //Reference to GameManager
    private UserManager m_usermanager;  //Reference to UserManager
    private CanvasGameOverScript m_GameOverScript;      //Reference to CanvasGameOverScript
    private CanvasPauseMenuScript m_PauseMenuScript;    //Reference to CanvasPauseMenuScript
    private CanvasPlayerUIScript m_PlayerUIScript;      //Reference to CanvasPlayerUIScript
    private ShopUIScript m_ShopUIScript;    //Reference to ShopUIScript


    //Constructer
    public void Start()
    {
        //Find Game manager
        m_gamemanager = GameObject.FindWithTag("Gamemanager").GetComponent<GameManager>();
        m_usermanager = m_gamemanager.getUserManager();

        //Instantiate and set references to canvasses (and shop)
        GameObject.Instantiate(m_CanvasHelpScreen, Vector3.zero, Quaternion.identity);
        go_CanvasGameOver   = GameObject.Instantiate(m_CanvasGameOverPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        go_CanvasPauseMenu  = GameObject.Instantiate(m_CanvasPauseMenuPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        go_CanvasPlayerUI   = GameObject.Instantiate(m_CanvasPlayerUIPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        go_CanvasShop       = GameObject.Instantiate(m_CanvasShopPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        go_Shop = GameObject.FindWithTag("Shop");

        //Get scripts of canvasses
        m_GameOverScript = go_CanvasGameOver.GetComponent<CanvasGameOverScript>();
        m_PauseMenuScript = go_CanvasPauseMenu.GetComponent<CanvasPauseMenuScript>();
        m_PlayerUIScript = go_CanvasPlayerUI.GetComponent<CanvasPlayerUIScript>();
        m_ShopUIScript = go_CanvasShop.GetComponent<ShopUIScript>();

        //Find shop and set variable
        ShopScript m_shopscript = go_Shop.GetComponent<ShopScript>();
        m_shopscript.m_instance_UI = go_CanvasShop;

        //Set visibility of canvas
        go_CanvasGameOver.SetActive(false);
        go_CanvasPauseMenu.SetActive(false);
        go_CanvasPlayerUI.SetActive(true);
        go_CanvasShop.SetActive(false);

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
            go_CanvasPauseMenu.SetActive(false);
            go_CanvasPlayerUI.SetActive(false);
            go_CanvasShop.SetActive(false);
        }
        else
        {
            //Check for pause
            if (pause)
            {
                go_CanvasGameOver.SetActive(false);
                go_CanvasPauseMenu.SetActive(true);
                go_CanvasPlayerUI.SetActive(false);
                go_CanvasShop.SetActive(false);
            }
            else
            {
                //Check wavephase
                if (wavephase)
                {
                    go_CanvasGameOver.SetActive(false);
                    go_CanvasPauseMenu.SetActive(false);
                    go_CanvasPlayerUI.SetActive(true);
                    m_PlayerUIScript.showWaveRemaining(true);

                }
                else
                {
                    go_CanvasGameOver.SetActive(false);
                    go_CanvasPauseMenu.SetActive(false);
                    go_CanvasPlayerUI.SetActive(true);
                    m_PlayerUIScript.showWaveRemaining(false);
                }
            }
        }

    }

    public IEnumerator showWaveStatsUI()
    {
        yield return StartCoroutine(m_PlayerUIScript.showWaveStatsUI());
    }

    public void setScore()
    {
        m_GameOverScript.setScore();
    }
   

}
