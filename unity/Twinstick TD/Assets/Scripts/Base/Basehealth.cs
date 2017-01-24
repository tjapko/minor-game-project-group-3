using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Class Basehealh
/// </summary>
public class Basehealth : MonoBehaviour {

    //References
    [Header("References")]
    public GameObject m_hitBaseCanvasPrefab;    //Reference to friendlyHit canvas;
    private GameObject m_maincamera;            //Reference to maincamera

    //Public variables
    public float m_StartingHealth = 100f;           //Starting health:Untouched
    public float m_StartingMaxHealth = 100f;        //Starting max health:Untouched
    public float m_maxhealth;                       //Maximum health: Current max health
    public Color m_FullHealthColor = Color.green;   //Full health colour
    public Color m_ZeroHealthColor = Color.red;     //Zero health colour
    public Slider m_Slider;
    public Image m_FillImage;
    public AudioSource deathsound;
    public AudioClip death;

    //Private variables
    private float m_CurrentHealth;                  //Current health of tower
    [HideInInspector] public bool m_Dead = false;                            //Boolean if tower is dead
    
    // OnEnable
    public void OnEnable()
    {
        //Set references
        m_maincamera = GameObject.FindWithTag("MainCamera");

        m_Dead = false;
		gameObject.SetActive (true);
        //Set starting variables
        m_maxhealth = m_StartingMaxHealth;
        m_CurrentHealth = m_StartingHealth;
        SetHealthUI();
    }

    

    //Take damage function
    public void TakeDamage(float amount)
    {
		if (gameObject.activeInHierarchy == true) {
			createHitMark (m_hitBaseCanvasPrefab, -amount);
			//Amount must be smaller than zero
			if (amount > 0) {
				m_CurrentHealth -= amount;
				SetHealthUI ();
			}
			//Check if base has less than 0 health
			if (m_CurrentHealth <= 0f && !m_Dead) {
				m_CurrentHealth = 0f;
				OnDeath ();
			}
		}
		if (m_CurrentHealth != 0) {
			ScreenShake.ShakeBase ();
		}
		if (m_CurrentHealth == 0) {
			ScreenShake.ShakeBaseOndeath ();
		}
    }

    //Heal base
    public void Healbase(float amount)
    {
        //Heal base to max health
        if(m_CurrentHealth + amount >= m_maxhealth)
        {
            m_CurrentHealth = m_maxhealth;
        } else if (amount > 0)
        {
            m_CurrentHealth += amount;
        }
    }

    public void SetHealthUI()
    {
        // Set the slider's value appropriately.
        m_Slider.value = m_CurrentHealth;

        // Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_maxhealth);
    }

    // OnDeath
    private void OnDeath()
    {
        deathsound.transform.parent = null;
        deathsound.clip = death;
        deathsound.Play();
        // Destroy(deathsound.gameObject, 8f);

        //reset turrets of base
        BaseUpgradeScript upgrade_script = GetComponent<BaseUpgradeScript>();
        upgrade_script.getBaseUpgradeList().ForEach(delegate (List<BaseUpgrade> list) {
            foreach (BaseUpgrade upgrade in list)
            {
                if (upgrade.getBaseUpgradeType() == BaseUpgrade.BaseUpgradeType.BaseUpgrade)
                {
                    ((Upgrade_Base)upgrade).revertUpgrade();
                }
            }
        });

        m_Dead = true;
        gameObject.SetActive(false);

        //Set shop canvas to rebuild canvas
        ShopScript m_shopscript = GameObject.FindWithTag("Shop").GetComponent<ShopScript>();
        ShopUIScript m_shopuiscript = m_shopscript.m_instance_UI.GetComponent<ShopUIScript>();
        m_shopuiscript.setBaseDestroyed(true);
        //		ScreenShake.ShakeBaseOndeath ();

        
    }

    // On Rebuild
    public void OnRebuild()
    {
        OnEnable();
        ShopScript m_shopscript = GameObject.FindWithTag("Shop").GetComponent<ShopScript>();
        ShopUIScript m_shopuiscript = m_shopscript.m_instance_UI.GetComponent<ShopUIScript>();
        m_shopuiscript.setBaseDestroyed(false);
    }

    //Spawn hitmark
    private void createHitMark(GameObject prefab, float amount)
    {
        //Set hitmark
        GameObject hitbox = GameObject.Instantiate(prefab, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
        HitMarkScript hitbox_script = hitbox.GetComponent<HitMarkScript>();
        hitbox_script.setDamage(amount);
        hitbox_script.setCamera(m_maincamera);
        hitbox_script.lookToCamera();
        hitbox.SetActive(true);
    }

    //Increase max health
    public void incMaxHealth(int amount)
    {
        m_maxhealth += amount;
        m_CurrentHealth += amount;
    }

    //Getter for current health
    public float getCurrentHealth()
    {
        return m_CurrentHealth;
    }
}
