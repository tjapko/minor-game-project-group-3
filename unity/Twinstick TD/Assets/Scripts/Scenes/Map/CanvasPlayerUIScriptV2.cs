using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CanvasPlayerUIScriptV2 : MonoBehaviour
{

    //Public variables
    [Header("References")]
    public Color m_FullHealthColor = Color.green;   //Full health colour
    public Color m_ZeroHealthColor = Color.red;     //Zero health colour

    //References
    [Header("References")]
    public GameObject m_HighscorePanel;         //Reference to the high score panel
    public Text txt_highscore;                  //Reference to the text in the high score panel
    public GameObject m_PlayerStatisticsPanel;  //Reference to the player statistics panel
    public Slider sld_healthslider;             //Reference to the health slider in the player statistics panel
    public Image img_healthfillImage;           //Reference to the the fill image in the health slider
    public Image img_weapon_1;                  //Reference to the image of weapon 1
    public Image img_weapon_2;                  //Reference to the image of weapon 2
    public Image img_weapon_3;                  //Reference to the image of weapon 3
    public Text txt_ammo_weapon_1;              //Reference to the ammo of weapon 1
    public GameObject m_WaveStatisticsPanel;    //Reference to the wave statistics panel
    public Text txt_waveremaining;              //Reference to the wave remaining text in the wave statistics panel
    public Text txt_wavenumber;                 //Reference to the wave number text in the wave statistics panel
	public Text txt_kills;

    //References to managers
    private GameManager m_gamemanager;      //Reference to game manager (used to invoke next wave)
    private UserManager m_usermanager;      //Reference to the user manager in the game manager
    private PlayerManager m_player;         //Reference to the player
    private WaveManager m_wavemanager;      //Reference to the wave manager in the game manager

    public void StartInitialization()
    {
        //Find gamemanager
        m_gamemanager = GameObject.FindWithTag("Gamemanager").GetComponent<GameManager>();
        m_usermanager = m_gamemanager.getUserManager();
        

        //Instantiate
        m_HighscorePanel.SetActive(true);
        m_PlayerStatisticsPanel.SetActive(true);
        m_WaveStatisticsPanel.SetActive(true);

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
            m_player = m_usermanager.m_playerlist[0];
            m_wavemanager = m_gamemanager.getWaveManager();

            updateHighScoreText();
            updateHealth();
            updateWeapon();
            updateWaveStatistics();
            setWaveNumber();
			setKills();
        }
        catch
        {

        }


    }

    //Set high score text
    private void updateHighScoreText()
    {
        txt_highscore.text = "" + m_player.m_stats.getScore();
		if (m_player.m_stats.getScore () < 0) {
			txt_highscore.text = "0";
		}
    }

    //Updat Health UI
    private void updateHealth()
    {
        // Set the slider's value appropriately.
        sld_healthslider.maxValue = m_player.m_playerhealth.m_maxHealth;
        sld_healthslider.value = m_player.m_playerhealth.getCurrentHealth();

        // Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
        img_healthfillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_player.m_playerhealth.getCurrentHealth() / m_player.m_playerhealth.m_maxHealth);
    }

    //Update weapon icon
    private void updateWeapon()
    {
        img_weapon_1.sprite = m_player.m_inventory.inventory[0].itemicon;
        img_weapon_2.sprite = m_player.m_inventory.inventory[1].itemicon;
        img_weapon_3.sprite = m_player.m_inventory.inventory[2].itemicon;

        Weapon selected_wep = m_player.m_inventory.inventory[0];
        string max_ammo = (selected_wep.ammo > 10000) ? "∞" : selected_wep.ammo.ToString();
        txt_ammo_weapon_1.text = selected_wep.ammoInClip + "/" + max_ammo;
    }

    //Update Wave statistics
    private void updateWaveStatistics()
    {
        txt_waveremaining.text = "" + m_wavemanager.enemiesRemaining().ToString();
    }

    // Sets the current wave number
    public void setWaveNumber()
    {
        txt_wavenumber.text = "" + m_gamemanager.getWaveNumber();
    }

	// Sets the number of kills
	public void setKills()
	{
		txt_kills.text = "" + m_player.m_stats.getkills ();
	}
}
