using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CanvasBaseUpgrade : MonoBehaviour {

    //References
    private GameManager m_gamemanager;  //Reference to game manager
    private UserManager m_usermanager;  //Reference to user manager
    private GameObject m_base;          //Reference to the base
    private BaseUpgradeScript m_baseupgradescript;  //Reference to the base upgrade script
    private List<BaseUpgrade> m_listBaseUpgrades;   //Reference to the list of base upgrades
    private List<BaseUpgrade> m_listPlayerUpgrades; //Reference to the list of player upgrades

    //References to children
    private GameObject m_baseupgrademenu;   //Reference to the base upgrade menu
    private Text m_playerupgradeText;               //Reference to the base upgrade menu's first line of text
    private List<GameObject> go_playerupgradeslist; //List containing references to the player upgrades in the canvas
    private List<Image> img_playerupgradelist;      //List containing references of player upgrade icons
    private List<Text> txt_playerupgradelist;       //List containing references of player upgrade text
    private List<Button> btn_playerupgradelist_1;   //List containing references of upgrade buttons
    private List<Button> btn_playerupgradelist_2;   //List containing references of restore buttons
    private List<Text> btn_playerupgradelist_text_1;//List containing reference of the text insided the button
    private List<Text> btn_playerupgradelist_text_2;//List containing reference of the text insided the button
    private Text m_baseupgradeText;                 //Reference to the base upgrade menu's second line of text
    private List<GameObject> go_baseupgradeslist;   //List containing references to the base upgrades in the canvas
    private List<Image> img_baseupgradelist;        //List containing references of base upgrade icons
    private List<Text> txt_baseupgradelist;         //List containing references of base upgrade text
    private List<Button> btn_baseupgradelist_1;     //List containing references of base upgrade buttons
    private List<Button> btn_baseupgradelist_2;     //List containing references of base restore buttons
    private List<Text> btn_baseupgradelist_text_1;  //List containing reference of the text insided the button
    private List<Text> btn_baseupgradelist_text_2;  //List containing reference of the text insided the button

    //Private variables
    private PlayerManager selected_player;

	// Use this for initialization
	void StartInitialization () {
        //Initialize lists
        go_playerupgradeslist = new List<GameObject>();
        img_playerupgradelist = new List<Image>();
        txt_playerupgradelist = new List<Text>();
        btn_playerupgradelist_1 = new List<Button>();
        btn_playerupgradelist_2 = new List<Button>();
        btn_playerupgradelist_text_1 = new List<Text>();
        btn_playerupgradelist_text_2 = new List<Text>();

        go_baseupgradeslist = new List<GameObject>();
        img_baseupgradelist = new List<Image>();
        txt_baseupgradelist = new List<Text>();
        btn_baseupgradelist_1 = new List<Button>();
        btn_baseupgradelist_2 = new List<Button>();
        btn_baseupgradelist_text_1 = new List<Text>();
        btn_baseupgradelist_text_2 = new List<Text>();


        //Set references
        m_gamemanager = GameObject.FindWithTag("Gamemanager").GetComponent<GameManager>();
        m_usermanager = m_gamemanager.getUserManager();
        m_base = GameObject.FindWithTag("Base");
        m_baseupgradescript = m_base.GetComponent<BaseUpgradeScript>();

        m_baseupgrademenu = gameObject.transform.GetChild(0).gameObject;

        m_playerupgradeText = m_baseupgrademenu.transform.GetChild(0).GetComponent<Text>();
        go_playerupgradeslist.Add(m_baseupgrademenu.transform.GetChild(1).gameObject);
        m_playerupgradeText = m_baseupgrademenu.transform.GetChild(2).GetComponent<Text>();
        go_baseupgradeslist.Add(m_baseupgrademenu.transform.GetChild(3).gameObject);

        //Set references to children
        foreach(GameObject go_playerupgrade in go_playerupgradeslist)
        {
            img_playerupgradelist.Add(go_playerupgrade.transform.GetChild(0).GetComponent<Image>());
            txt_playerupgradelist.Add(go_playerupgrade.transform.GetChild(1).GetComponent<Text>());

            GameObject btn_1 = go_playerupgrade.transform.GetChild(2).gameObject;
            GameObject btn_2 = go_playerupgrade.transform.GetChild(3).gameObject;
            btn_playerupgradelist_1.Add(btn_1.GetComponent<Button>());
            btn_playerupgradelist_2.Add(btn_2.GetComponent<Button>());
            btn_playerupgradelist_text_1.Add(btn_1.transform.GetChild(0).GetComponent<Text>());
            btn_playerupgradelist_text_2.Add(btn_2.transform.GetChild(0).GetComponent<Text>());
        }

        foreach (GameObject go_baseupgrade in go_baseupgradeslist)
        {
            img_baseupgradelist.Add(go_baseupgrade.transform.GetChild(0).GetComponent<Image>());
            txt_baseupgradelist.Add(go_baseupgrade.transform.GetChild(1).GetComponent<Text>());

            GameObject btn_1 = go_baseupgrade.transform.GetChild(2).gameObject;
            GameObject btn_2 = go_baseupgrade.transform.GetChild(3).gameObject;
            btn_baseupgradelist_1.Add(btn_1.GetComponent<Button>());
            btn_baseupgradelist_2.Add(btn_2.GetComponent<Button>());
            btn_baseupgradelist_text_1.Add(btn_1.transform.GetChild(0).GetComponent<Text>());
            btn_baseupgradelist_text_2.Add(btn_2.transform.GetChild(0).GetComponent<Text>());
        }

    }

    //On enable function
    void OnEnable()
    {
        //Invoke functions
        InvokeRepeating("updateCanvas", 0f, 0.1f);
    }

    //On Disable function
    void OnDisable()
    {
        CancelInvoke("updateCanvas");
    }

    // Update is called once per frame
    private void updateCanvas() {
        m_listBaseUpgrades = m_baseupgradescript.getBaseUpgradeList();
        m_listPlayerUpgrades = m_baseupgradescript.getPlayerUpgradeList();
    }

    //Purchase Player upgrade
    public void purchasePlayerUpgrade(int upgrade_index)
    {

    }

    //Purchase Base Upgrade
    public void purchaseBaseUpgrade(int upgrade_index)
    {

    }

    //Setter for selected_player
    public void setSelectedPlayer(PlayerManager player)
    {
        selected_player = player;
    }

    private string typeToText(BaseUpgrade.BaseUpgradeType type)
    {
        switch (type)
        {
            case BaseUpgrade.BaseUpgradeType.BaseHealthUpgrade:
                return "Base Health";
            case BaseUpgrade.BaseUpgradeType.RestoreBaseHealth:
                return "Base Health";
            case BaseUpgrade.BaseUpgradeType.PlayerHealthUpgrade:
                return "Player Health";
            case BaseUpgrade.BaseUpgradeType.RestorePlayerHealth:
                return "Player Health";
            default:
                return "";
        }
    }
}
