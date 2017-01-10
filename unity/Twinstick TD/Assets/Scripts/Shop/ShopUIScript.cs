﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// ShopUIScript
/// Contains the basic functions to set the UI images
/// Contans the basic function for the UI Buttons
/// </summary>
public class ShopUIScript : MonoBehaviour {

    //References
    [HideInInspector] public ShopScript m_shopscript;         //Reference to ShopScript, which contains the weapons (Set by the shop)
    [HideInInspector] public PlayerManager m_currentplayer;    //Reference to the player that has opened the UI
    private GameObject go_ShopText;             //Reference to text of the shop (gameobject)
    private Text ui_shopText;                   //Reference to the text of the shop
    private GameObject go_ShopUpgradeButton;    //Reference to shop upgrade button
    private Text ui_ShopUpgradeButton;          //Reference to the shop upgrade button text
    private List<GameObject> go_weapons;        //List of references to the weapon game objects
    private List<Image> ui_weapon_icons;        //List of references to the weapon icons
    private List<Text> ui_weapon_name;          //List of references to the weapon names
    private List<Button> btn_weapon_cost;       //List of references to the weapon purchase buttons 
    private List<Text> ui_weapon_cost;          //List of references to the text of the weapons purchase buttons
    private List<Button> btn_weapon_ammo;       //List of references to the ammo purchase buttons
    private List<Text> ui_weapon_ammo_cost;     //List of references to the text of the ammo purchase buttons

    //Variables
    private List<Weapon> weaponlist;
    //private List<Weapon> ammolist;            //Fix (should be fixed when using different types of ammo)

    // Use this for initialization
    void Start () {
        //Find shop
        m_shopscript = GameObject.FindWithTag("Shop").GetComponent<ShopScript>();

        //Set lists
        weaponlist = m_shopscript.weaponsforsale;
        //ammolist = weaponlist; //Fix (should be fixed when using different types of ammo)

        GameObject menu = gameObject.transform.GetChild(0).gameObject;
        GameObject menushopinfo = menu.transform.GetChild(0).gameObject;

        //Set references
        go_ShopText = menushopinfo.transform.GetChild(0).gameObject;
        ui_shopText = go_ShopText.GetComponent<Text>();
        go_ShopUpgradeButton = menushopinfo.transform.GetChild(1).GetChild(0).gameObject;
        ui_ShopUpgradeButton = go_ShopUpgradeButton.GetComponent<Text>();

        //Get weapons in menu
        go_weapons = new List<GameObject>();
        go_weapons.Add(menu.transform.GetChild(1).gameObject);
        go_weapons.Add(menu.transform.GetChild(2).gameObject);
        go_weapons.Add(menu.transform.GetChild(3).gameObject);
        go_weapons.Add(menu.transform.GetChild(4).gameObject);

        //Get icons
        ui_weapon_icons = new List<Image>();
        foreach (GameObject go_weapon in go_weapons)
        {
            ui_weapon_icons.Add(go_weapon.transform.GetChild(0).GetComponent<Image>());
        }

        //Get  weapon type
        ui_weapon_name = new List<Text>();
        foreach(GameObject go_weapon in go_weapons)
        {
            ui_weapon_name.Add(go_weapon.transform.GetChild(1).GetComponent<Text>());
        }

        //Get weapon purchase buttons
        btn_weapon_cost = new List<Button>();
        foreach(GameObject go_weapon in go_weapons)
        {
            btn_weapon_cost.Add(go_weapon.transform.GetChild(2).gameObject.GetComponent<Button>());
        }

        //Get weapon cost
        ui_weapon_cost = new List<Text>();
        foreach(GameObject go_weapon in go_weapons)
        {
            ui_weapon_cost.Add(go_weapon.transform.GetChild(2).GetChild(0).GetComponent<Text>());
        }

        //Get ammo purchase buttons
        btn_weapon_ammo = new List<Button>();
        foreach (GameObject go_weapon in go_weapons)
        {
            btn_weapon_ammo.Add(go_weapon.transform.GetChild(3).gameObject.GetComponent<Button>());
        }

        //Get ammo cost
        ui_weapon_ammo_cost = new List<Text>();
        foreach(GameObject go_weapon in go_weapons)
        {
            ui_weapon_ammo_cost.Add(go_weapon.transform.GetChild(3).GetChild(0).GetComponent<Text>());
        }

        //Set text of shop
        ui_shopText.text = "Weapons and Ammo \n Tier 1";
        ui_ShopUpgradeButton.text = "Uprade Shop: \n" + m_shopscript.upgrade_cost[1];

        //Disable itself
        gameObject.SetActive(false);

    }

    //On enable function
    void OnEnable()
    {
        //Invoke functions
        InvokeRepeating("updateForSaleWeapons", 0f, 0.1f);
    }

    //On Disable function
    void OnDisable()
    {
        CancelInvoke("updateForSaleWeapons");
    }
    List<Weapon> old_list = null;
    //Update the UI panel
    private void updateForSaleWeapons()
    {
        //Set the lists
        weaponlist = m_shopscript.weaponsforsale;
        //ammolist = weaponlist; //Fix (should be fixed when using different types of ammo)

        //Update icons
        int index = 0;
        foreach(Image icon in ui_weapon_icons)
        {
            icon.sprite = weaponlist[index].itemicon;
            index++;
        }

        //Update weapon name
        index = 0;
        foreach(Text weapon_name in ui_weapon_name)
        {
            weapon_name.text = weaponlist[index].itemtype.ToString();
            index++;
        }

        //Update weapon price and interactable (also ammo interactable)
        PlayerInventory inv = m_currentplayer.m_inventory;
        for(int i = 0; i < weaponlist.Count; i++)
        {
            if (inv.InventoryContains(weaponlist[i]))
            {
                ui_weapon_cost[i].text = "Sold out";
                btn_weapon_cost[i].interactable = false;
            } else
            {
                ui_weapon_cost[i].text = "Buy Weapon:\n " + weaponlist[i].itemprice.ToString();
                btn_weapon_cost[i].interactable = true;
            }
        }

        //Update ammo price
        for(int i = 0; i < weaponlist.Count; i++)
        {
            //Get weapons player has currently equiped
            List<Weapon> player_weapons = inv.inventory;
            int inv_index = findSameWeaponType(weaponlist[i].itemtype);

            //Determine if player has same weapon type in inventory
            if (inv_index == 100)
            {
                btn_weapon_ammo[i].interactable = false;
                ui_weapon_ammo_cost[i].text = "Locked";
            } else
            {
                //Player has weapon, Check if player is allowed to buy ammo
                bool allowedtobuy = ((player_weapons[inv_index].ammo + player_weapons[inv_index].ammoInClip) < player_weapons[inv_index].maxAmmo);
                btn_weapon_ammo[i].interactable = allowedtobuy;
                //Change text of button
                if (allowedtobuy)
                {
                    ui_weapon_ammo_cost[i].text = "Buy Ammo:\n" + player_weapons[inv_index].ammoprice.ToString();
                } else
                {
                    ui_weapon_ammo_cost[i].text = "Weapon is\n full";
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        //Enable or disable player shooting script
        if(gameObject.activeSelf)
		{
            if (m_currentplayer != null)
            {
                m_currentplayer.m_shooting.enabled = false;
            }
        } else
        {
            if(m_currentplayer != null)
            {
                m_currentplayer.m_shooting.enabled = true;
            }
        }
	}

    //Function to purchase weapon (button)
    public void purchase_weapon(int index)
    {
		//First check if weapon exists in the list
        if (weaponlist.Count >= index)
        {
            //Check if player has empty slot
            bool empty_slot = m_currentplayer.m_inventory.InventoryContains(new Weapon());

            //Player has empty slot
            if (empty_slot)
            {
                //Check if player has enough money and inventory does not contain the weapon already
                if(m_currentplayer.m_stats.getCurrency() >= weaponlist[index].itemprice &&
                  !m_currentplayer.m_inventory.InventoryContains(weaponlist[index]))
                {
                    m_currentplayer.m_stats.addCurrency(-1 * weaponlist[index].itemprice);
                    m_currentplayer.m_inventory.addItem(weaponlist[index]);
                }
            } else
            {
                //Check if player has enough money and inventory does not contain the weapon already
                if (m_currentplayer.m_stats.getCurrency() >= weaponlist[index].itemprice &&
                  !m_currentplayer.m_inventory.InventoryContains(weaponlist[index]))
                {
                    m_currentplayer.m_stats.addCurrency(-1 * weaponlist[index].itemprice);
                    m_currentplayer.m_inventory.inventory[0] = weaponlist[index];
                }
            }
        }
    }
	
    //Find the index of a certain weapon type in the player inventory
	private int findSameWeaponType(Item.ItemType itemtype){
        List<Weapon> inv = m_currentplayer.m_inventory.inventory;

        for(int i = 0; i < inv.Count; i++)
        {
            if (inv[i].itemtype.Equals(itemtype))
            {
                return i;
            }
            
        }
		//random number to indicate that the player doesn't have such a weapon;
		return 100;
	}

    //Function to purchase ammo (button)
	public void purchase_ammo(int weaponIndex) {
        //Get player inventory
		int inventoryIndex = findSameWeaponType(weaponlist[weaponIndex].itemtype);

		if (inventoryIndex != 100){
			// check whether player got enough money & doesn't have max ammo already
			if (m_currentplayer.m_stats.getCurrency () >= weaponlist [weaponIndex].ammoprice && (m_currentplayer.m_inventory.inventory [inventoryIndex].ammo + weaponlist [weaponIndex].ammoInClip) < weaponlist [weaponIndex].maxAmmo) {
				//decrease currency
				m_currentplayer.m_stats.addCurrency (-weaponlist [weaponIndex].ammoprice);

				//add ammo (amount that goes in one clip) to certain weapon
				m_currentplayer.m_inventory.inventory [inventoryIndex].ammo += weaponlist [weaponIndex].ammoInClip;
			}
		}
    }

    //Upgrade store function
    public void upgradeStore()
    {

        if (m_currentplayer.m_stats.getCurrency() >= m_shopscript.upgrade_cost[m_shopscript.getCurrentTier()])
        {
            //Substract currency
            m_currentplayer.m_stats.addCurrency(-m_shopscript.upgrade_cost[m_shopscript.getCurrentTier()]);
            //Increment tier (weapons are loaded automatically)
            m_shopscript.incTier();
            //Change text of text and buttons
            if(m_shopscript.getCurrentTier() >= m_shopscript.upgrade_cost.Length)
            {
                ui_shopText.text = "Weapons and Ammo \n Tier " + m_shopscript.getCurrentTier();
                go_ShopUpgradeButton.transform.parent.gameObject.SetActive(false);
            } else
            {
                ui_shopText.text = "Weapons and Ammo \n Tier " + m_shopscript.getCurrentTier();
                ui_ShopUpgradeButton.text = "Uprade Shop: \n" + m_shopscript.upgrade_cost[m_shopscript.getCurrentTier()];
            }
            

        }
    }
}
