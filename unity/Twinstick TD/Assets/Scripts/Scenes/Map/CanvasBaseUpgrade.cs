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
    private Basehealth m_basehealth;                //Reference to base health script
    private List<List<BaseUpgrade>> m_listBaseUpgrades;   //Reference to the list of base upgrades
    private List<List<BaseUpgrade>> m_listPlayerUpgrades; //Reference to the list of player upgrades

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
	public void StartInitialization () {
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
        m_base = m_gamemanager.getBaseManager().m_Instance;
        m_basehealth = m_base.GetComponent<Basehealth>();
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
        if (selected_player != null)
        {
            selected_player.m_shooting.enabled = true;
        }
    }

    // Update is called once per frame
    private void updateCanvas() {
        m_listBaseUpgrades = m_baseupgradescript.getBaseUpgradeList();
        m_listPlayerUpgrades = m_baseupgradescript.getPlayerUpgradeList();

        updateBaseUpgrade();
        updatePlayerUpgrade();

        if (selected_player != null)
        {
            selected_player.m_shooting.enabled = false;
        }

    }

    //Purchase Player upgrade
    public void purchasePlayerUpgrade_1(int upgrade_duo)
    {
        selectUpgrade(m_listPlayerUpgrades[upgrade_duo][0]);
    }

    //Purchase Player upgrade
    public void purchasePlayerUpgrade_2(int upgrade_duo)
    {
        selectUpgrade(m_listPlayerUpgrades[upgrade_duo][1]);
    }

    //Purchase Base Upgrade
    public void purchaseBaseUpgrade_1(int upgrade_duo)
    {
        selectUpgrade(m_listBaseUpgrades[upgrade_duo][0]);
    }

    //Purchase Base Upgrade
    public void purchaseBaseUpgrade_2(int upgrade_duo)
    {
        selectUpgrade(m_listBaseUpgrades[upgrade_duo][1]);
    }

    //Setter for selected_player
    public void setSelectedPlayer(PlayerManager player)
    {
        selected_player = player;
    }

    //Update Base function
    private void updateBaseUpgrade()
    {
        for (int i = 0; i < go_baseupgradeslist.Count; i++)
        {
            //Only apply changes when there are enough display panels available
            if (i < m_listBaseUpgrades.Count)
            {
                PlayerStatistics m_stats = selected_player.m_stats;
                BaseUpgrade upgrade_1 = m_listBaseUpgrades[i][0];
                BaseUpgrade upgrade_2 = m_listBaseUpgrades[i][1];

                img_baseupgradelist[i].sprite = upgrade_1.getIcon();
                txt_baseupgradelist[i].text = typeToText(upgrade_1.getBaseUpgradeType());

                if (upgrade_1.getPrice() == -1)
                {
                    btn_baseupgradelist_1[i].interactable = false;
                    btn_baseupgradelist_text_1[i].text = "Locked";
                }
                else
                {
                    if (m_stats.m_currency >= upgrade_1.getPrice())
                    {
                        if(upgrade_1.getBaseUpgradeType() == BaseUpgrade.BaseUpgradeType.RestoreBaseHealth)
                        {
                            if(m_basehealth.getCurrentHealth() >= m_basehealth.m_maxhealth)
                            {
                                btn_baseupgradelist_1[i].interactable = false;
                                btn_baseupgradelist_text_1[i].text = "Max health";
                            } else
                            {
                                btn_baseupgradelist_1[i].interactable = true;
                                btn_baseupgradelist_text_1[i].text = buttonText(upgrade_1.getBaseUpgradeType()) + upgrade_1.getPrice();
                            }
                        } else
                        {
                            btn_baseupgradelist_1[i].interactable = true;
                            btn_baseupgradelist_text_1[i].text = buttonText(upgrade_1.getBaseUpgradeType()) + upgrade_1.getPrice();
                        }
                        
                    }
                    else
                    {
                        btn_baseupgradelist_1[i].interactable = false;
                        btn_baseupgradelist_text_1[i].text = "Locked";
                    }
                }

                if (upgrade_2.getPrice() == -1)
                {
                    btn_baseupgradelist_2[i].interactable = false;
                    btn_baseupgradelist_text_2[i].text = "Locked";
                }
                else
                {
                    if (m_stats.m_currency >= upgrade_2.getPrice())
                    {
                        if (upgrade_2.getBaseUpgradeType() == BaseUpgrade.BaseUpgradeType.RestoreBaseHealth)
                        {
                            if (m_basehealth.getCurrentHealth() >= m_basehealth.m_maxhealth)
                            {
                                btn_baseupgradelist_2[i].interactable = false;
                                btn_baseupgradelist_text_2[i].text = "Max health";
                            }
                            else
                            {
                                btn_baseupgradelist_2[i].interactable = true;
                                btn_baseupgradelist_text_2[i].text = buttonText(upgrade_2.getBaseUpgradeType()) + upgrade_2.getPrice();
                            }
                        }
                        else
                        {
                            btn_baseupgradelist_2[i].interactable = true;
                            btn_baseupgradelist_text_2[i].text = buttonText(upgrade_2.getBaseUpgradeType()) + upgrade_2.getPrice();
                        }

                    }
                    else
                    {
                        btn_baseupgradelist_2[i].interactable = false;
                        btn_baseupgradelist_text_2[i].text = "Locked";
                    }
                }

            } else
            {
                break;
            }
        }
    }

    //Update Player function
    private void updatePlayerUpgrade()
    {
        for (int i = 0; i < go_playerupgradeslist.Count; i++)
        {
            //Only apply changes when there are enough display panels available
            if (i < m_listBaseUpgrades.Count)
            {
                PlayerStatistics m_stats = selected_player.m_stats;
                BaseUpgrade upgrade_1 = m_listPlayerUpgrades[i][0];
                BaseUpgrade upgrade_2 = m_listPlayerUpgrades[i][1];

                img_playerupgradelist[i].sprite = upgrade_1.getIcon();
                txt_playerupgradelist[i].text = typeToText(upgrade_1.getBaseUpgradeType());

                if (upgrade_1.getPrice() == -1)
                {
                    btn_playerupgradelist_1[i].interactable = false;
                    btn_playerupgradelist_text_1[i].text = "Locked";
                }
                else
                {
                    if (m_stats.m_currency >= upgrade_1.getPrice())
                    {
                        btn_playerupgradelist_1[i].interactable = true;
                        btn_playerupgradelist_text_1[i].text = buttonText(upgrade_1.getBaseUpgradeType()) + upgrade_1.getPrice();
                    }
                    else
                    {
                        btn_playerupgradelist_1[i].interactable = false;
                        btn_playerupgradelist_text_1[i].text = "Locked";
                    }
                }

                if (upgrade_2.getPrice() == -1)
                {
                    btn_playerupgradelist_2[i].interactable = false;
                    btn_playerupgradelist_text_2[i].text = "Locked";
                }
                else
                {
                    if (m_stats.m_currency >= upgrade_1.getPrice())
                    {
                        btn_playerupgradelist_2[i].interactable = true;
                        btn_playerupgradelist_text_2[i].text = buttonText(upgrade_2.getBaseUpgradeType()) + upgrade_2.getPrice();
                    }
                    else
                    {
                        btn_playerupgradelist_2[i].interactable = false;
                        btn_playerupgradelist_text_2[i].text = "Locked";
                    }
                }

            }
        }
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

    private string buttonText(BaseUpgrade.BaseUpgradeType type)
    {
        switch (type)
        {
            case BaseUpgrade.BaseUpgradeType.BaseHealthUpgrade:
                return "Upgrade Base Health: ";
            case BaseUpgrade.BaseUpgradeType.RestoreBaseHealth:
                return "Buy Health: ";
            case BaseUpgrade.BaseUpgradeType.PlayerHealthUpgrade:
                return "Upgrade Player Health: ";
            case BaseUpgrade.BaseUpgradeType.RestorePlayerHealth:
                return "Buy Health: ";
            case BaseUpgrade.BaseUpgradeType.Empty:
                return "Locked";
            default:
                return "";
        }
    }

    private void selectUpgrade(BaseUpgrade selected_upgrade)
    {
        PlayerStatistics player_stats = selected_player.m_stats;

        switch (selected_upgrade.getBaseUpgradeType())
        {
            case BaseUpgrade.BaseUpgradeType.BaseHealthUpgrade:
                Upgrade_BaseHealth upgrade_1 = (Upgrade_BaseHealth)selected_upgrade;

                if(upgrade_1.getPrice() != -1 && player_stats.getCurrency() >= upgrade_1.getPrice())
                {
                    player_stats.substractCurrency(upgrade_1.getPrice());
                    upgrade_1.UpgradeBase(m_base);
                }
                break;
            case BaseUpgrade.BaseUpgradeType.RestoreBaseHealth:
                Restore_BaseHealth upgrade_2 = (Restore_BaseHealth)selected_upgrade;
                Debug.Log("price: " + upgrade_2.getPrice());
                if (upgrade_2.getPrice() != -1 && player_stats.getCurrency() >= upgrade_2.getPrice())
                {
                    upgrade_2.restoreBaseHealth(m_base, selected_player);
                }
                break;
            case BaseUpgrade.BaseUpgradeType.PlayerHealthUpgrade:
                Upgrade_PlayerHealth upgrade_3 = (Upgrade_PlayerHealth)selected_upgrade;
                if (upgrade_3.getPrice() != -1 && player_stats.getCurrency() >= upgrade_3.getPrice())
                {
                    
                    player_stats.substractCurrency(upgrade_3.getPrice());
                    upgrade_3.upgradePlayerHealth(selected_player);
                }
                break;
            case BaseUpgrade.BaseUpgradeType.RestorePlayerHealth:
                Restore_PlayerHealth upgrade_4 = (Restore_PlayerHealth)selected_upgrade;

                if (upgrade_4.getPrice() != -1 && player_stats.getCurrency() >= upgrade_4.getPrice())
                {
                    upgrade_4.restorePlayerHealth(selected_player);
                }

                break;
            case BaseUpgrade.BaseUpgradeType.Empty:
                break;
            default:
                break;
        }
    }
}
