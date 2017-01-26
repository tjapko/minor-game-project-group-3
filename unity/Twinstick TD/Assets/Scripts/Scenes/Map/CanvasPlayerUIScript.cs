using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CanvasPlayerUIScript : MonoBehaviour {
    //Public variables
    public float m_showRewardTime = 2.0f;   //Show reward time

    //References to managers
    private GameManager m_gamemanager;      //Reference to game manager (used to invoke next wave)
    private UserManager m_usermanager;      //Reference to the usermanager in the game manager
    private WaveManager m_wavemanager;
    private List<PlayerManager> m_players;  //Reference to the players in the game

    //References to GameObject
    private GameObject m_wavecontrol;          //Reference to the wave control panel
    private GameObject m_constructionpanel;    //Reference to the construction panel
    private List<GameObject> m_playerstats;     //Reference to the player stats UI
    private List<Text> m_player_money;          //Reference to the text displaying the money of the player
    private List<Text> m_player_kills;          //Reference to the text displaying the kills ma
    private List<GameObject> m_weaponstats;     //Referene to the weapons of the player
    private List<List<Image>> m_weaponIcon;     //Reference to the weapon icon slots
    private List<List<Text>> m_weaponammo;      //Reference to the ammo Text component
    private List<Slider> m_healthslider;        //Slider for player health
    private List<Image> m_fillImage;            //Image for player health
    private GameObject m_wavestats;             //Reference tot he Wave stats UI panel
    private GameObject m_waveremaining;         //Reference to the wave remaining UI panel
    private Text m_waveremainingText;           //Reference to the text in the wave remaining UI panel

    public void StartInitialization()
    {
        //Find gamemanager
        m_gamemanager = GameObject.FindWithTag("Gamemanager").GetComponent<GameManager>();
        m_usermanager = m_gamemanager.getUserManager();
        m_players = m_usermanager.m_playerlist;
        m_wavemanager = m_gamemanager.getWaveManager();

        //Get variables
        int amountofplayers = m_gamemanager.m_amountofplayers;
        m_player_money = new List<Text>();
        m_player_kills = new List<Text>();
        m_playerstats = new List<GameObject>();
        m_weaponstats = new List<GameObject>();
        m_weaponIcon = new List<List<Image>>();
        m_weaponammo = new List<List<Text>>();
        m_healthslider = new List<Slider>();
        m_fillImage = new List<Image>();


        //Set references to children
        m_wavecontrol = gameObject.transform.GetChild(0).gameObject;
        m_constructionpanel = gameObject.transform.GetChild(1).gameObject;
        m_playerstats.Add(gameObject.transform.GetChild(2).gameObject); //Add player 1
        m_playerstats.Add(gameObject.transform.GetChild(3).gameObject); //Add player 2
        //m_playerstats.Add(gameObject.transform.GetChild().gameObject); //Add player 3
        //m_playerstats.Add(gameObject.transform.GetChild().gameObject); //Add player 4
        m_weaponstats.Add(gameObject.transform.GetChild(4).gameObject);
        m_weaponstats.Add(gameObject.transform.GetChild(5).gameObject);
        //m_weaponstats.Add(gameObject.transform.GetChild().gameObject);
        //m_weaponstats.Add(gameObject.transform.GetChild().gameObject);
        m_wavestats = gameObject.transform.GetChild(6).gameObject;
        m_waveremaining = gameObject.transform.GetChild(7).gameObject;

        //Set m_playerstats active
        for (int i = 0; i < m_playerstats.Count; i++)
        {
            m_playerstats[i].SetActive(i < amountofplayers);
        }

        //Set player money Text
        

        for (int i = 0; i < amountofplayers; i++)
        {
            m_player_money.Add(m_playerstats[i].transform.GetChild(1).GetComponent<Text>());
            m_player_kills.Add(m_playerstats[i].transform.GetChild(2).GetComponent<Text>());

            m_healthslider.Add(m_weaponstats[i].transform.GetChild(0).GetComponent<Slider>());
            m_fillImage.Add(m_weaponstats[i].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>());

            List<Image> image_list = new List<Image>();
            image_list.Add(m_weaponstats[i].transform.GetChild(1).GetComponent<Image>());
            image_list.Add(m_weaponstats[i].transform.GetChild(2).GetComponent<Image>());
            image_list.Add(m_weaponstats[i].transform.GetChild(3).GetComponent<Image>());
            m_weaponIcon.Add(image_list);

            List<Text> ammo_list = new List<Text>();
            ammo_list.Add(m_weaponstats[i].transform.GetChild(4).GetComponent<Text>());
            ammo_list.Add(m_weaponstats[i].transform.GetChild(5).GetComponent<Text>());
            ammo_list.Add(m_weaponstats[i].transform.GetChild(6).GetComponent<Text>());
            m_weaponammo.Add(ammo_list);

           

        }

        //Activate weaponstats 
        for (int i = 0; i < m_weaponstats.Count; i++)
        {
            m_weaponstats[i].SetActive(i < amountofplayers);
        }

        m_waveremainingText = m_waveremaining.transform.GetChild(0).GetComponent<Text>();

        //Set active UI
        m_wavecontrol.SetActive(true);
        m_constructionpanel.SetActive(false);
        m_waveremaining.SetActive(false);

    }

    //OnEnable Function
    void OnEnable()
    {
        InvokeRepeating("UpdateUI", 0.0f, 0.1f);
    }

    //OnDisble Function
    void OnDisable()
    {
        CancelInvoke("UpdateUI");
    }

    //Updates the UI
    public void UpdateUI()
    {
        try
        {
            SetCurrencyText();
            SetKillsText();
            updateWeaponIcon();
            UpdateAmmoText();
            updateHealthUI();
            setEnemiesRemainingText();
        } catch
        {
            //Start() hasn't finished setting references
        }
        

    }

    //Show or hide the UI panel
    public IEnumerator showWaveStatsUI()
    {
        //Set the text of the UI
        int wavenumber = m_gamemanager.getWaveNumber();
        m_wavestats.transform.GetChild(0).GetComponent<Text>().text = "Wave " + wavenumber;

        m_wavestats.SetActive(true);
        m_wavestats.GetComponent<Image>().CrossFadeAlpha(1.0f, 2.0f, false);
        m_wavestats.transform.GetChild(0).GetComponent<Text>().CrossFadeAlpha(1.0f, 2.0f, false);

        yield return new WaitForSeconds(1.0f);

        m_wavestats.GetComponent<Image>().CrossFadeAlpha(0.0f, 2.0f, false);
        m_wavestats.transform.GetChild(0).GetComponent<Text>().CrossFadeAlpha(0.0f, 2.0f, false);
        m_wavestats.SetActive(false);
    }

    // sets the currencyText which is visible on the screen to the current Currency
    private void SetCurrencyText()
    {
        foreach (Text currencytext in m_player_money)
        {
            currencytext.text = "Currency: " + m_usermanager.m_playerlist[0].m_stats.getCurrency().ToString();
        }
    }

    // sets the KillsText which is visible on the screen to the current amount of kills
    private void SetKillsText()
    {
        foreach (Text killstext in m_player_kills)
        {
            killstext.text = "Kills: " + m_usermanager.m_playerlist[0].m_stats.getkills().ToString();
        }
    }

    // Sets the enemies remaining text
    private void setEnemiesRemainingText()
    {
        if (m_wavemanager == null)
        {
            m_wavemanager = m_gamemanager.getWaveManager();
        }
        m_waveremainingText.text = "Enemies left: " + m_wavemanager.enemiesRemaining();
    }

    // Sets the icons of the guns
    private void updateWeaponIcon()
    {
        int playerindex = 0;
        foreach (List<Image> player in m_weaponIcon)
        {
            int iconindex = 0;
            foreach (Image icon in player)
            {
                icon.sprite = getWeaponIcon(m_usermanager.m_playerlist[playerindex], iconindex);
                iconindex++;
            }
            playerindex++;
        }
    }

    // Sets the ammo text
    private void UpdateAmmoText()
    {
        int player_index = 0;
        foreach (List<Text> player_ui in m_weaponammo)
        {
            int element_index = 0;
            foreach (Text ammo_text in player_ui)
            {
                Weapon selected_wep = m_usermanager.m_playerlist[player_index].m_inventory.inventory[element_index];
                string max_ammo = (selected_wep.ammo > 10000) ? "∞" : selected_wep.ammo.ToString();
                ammo_text.text = selected_wep.ammoInClip + "/" + max_ammo;
                element_index++;
            }
            player_index++;
        }

    }

    // Gets the icon of the gun
    private Sprite getWeaponIcon(PlayerManager player, int index)
    {
        return player.m_inventory.inventory[index].itemicon;
    }

    // Show/hide Pause menu
    public void showWaveControl(bool status)
    {
        m_wavecontrol.SetActive(status);
    }

    // Show/hide Pause menu
    public void showConstructonPanel(bool status)
    {
        m_constructionpanel.SetActive(false);
    }

    // Show/hide the wave remaining panel
    public void showWaveRemaining(bool status)
    {
        if(m_waveremaining != null)
        {
            m_waveremaining.SetActive(status);
        }
    }

    //Updates health UI
    public void updateHealthUI()
    {
        for(int i = 0; i < m_players.Count; i++)
        {
            PlayerHealth health_script = m_players[i].m_playerhealth;

            m_healthslider[i].maxValue = health_script.m_maxHealth;
            m_healthslider[i].value = health_script.getCurrentHealth();
            m_fillImage[i].color = Color.Lerp(health_script.m_ZeroHealthColor, health_script.m_FullHealthColor, health_script.getCurrentHealth() / health_script.m_maxHealth);
        }
    }

    //Show wave reward
    public IEnumerator showWaveReward()
    {
        CancelInvoke("UpdateUI");

        int index = 0;
        float waveReward = m_usermanager.waveCurrency();
        foreach (Text currencytext in m_player_money)
        {
            int carrotReward = m_usermanager.m_playerlist[index].m_construction.getCarrots();
            currencytext.text = "Wave: +" + waveReward + ", Carrot Fields: +" + carrotReward;
            index++;
        }
        yield return new WaitForSeconds(m_showRewardTime);

        InvokeRepeating("UpdateUI", 0.0f, 0.1f);

    }
}
