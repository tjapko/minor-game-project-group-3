using UnityEngine;
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
    public ShopScript m_shopscript;         //Reference to ShopScript, which contains the weapons (Set by the shop)
    public PlayerManager m_currentplayer;    //Reference to the player that has opened the UI
    private GameObject weaponIconsTab;      //Reference to the Weapons Icon Tab
    private GameObject weaponPurchaseTab;   //Reference to the Weapon purchase buttons tab
    private GameObject ammoPurchaseTab;     //Reference to the Ammo purchase buttons tab

    //Variables
    private List<Weapon> weaponlist;
    private List<Weapon> ammolist;

	// Use this for initialization
	void Start () {
        //Set lists
        weaponlist = m_shopscript.weaponsforsale;
        ammolist = m_shopscript.ammoforsale;

        //Set references
        weaponIconsTab = gameObject.transform.GetChild(0).GetChild(1).gameObject;
        weaponPurchaseTab = gameObject.transform.GetChild(0).GetChild(3).gameObject;
        ammoPurchaseTab = gameObject.transform.GetChild(0).GetChild(4).gameObject;

    }
	
	// Update is called once per frame
	void Update () {
        //If the UI is active update the list of weapons and ammo
        if(gameObject.activeSelf)
		{
            //Set the lists
            weaponlist = m_shopscript.weaponsforsale;
            ammolist = m_shopscript.ammoforsale;

            //Set the icons
            weaponIconsTab.transform.GetChild(0).GetComponent<Image>().sprite = weaponlist[0].itemicon;
            weaponIconsTab.transform.GetChild(1).GetComponent<Image>().sprite = weaponlist[1].itemicon;
            weaponIconsTab.transform.GetChild(2).GetComponent<Image>().sprite = weaponlist[2].itemicon;

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

    //Function to purchase ammo (button)
	public void purchase_ammo(int index) {
		// check whether player got enough money & doesn't have max ammo already
		if (m_currentplayer.m_stats.getCurrency () >= weaponlist [index].ammoprice && (m_currentplayer.m_inventory.inventory[index].ammo + weaponlist[index].ammoInClip) < weaponlist[index].maxAmmo) {
			//decrease currency
			m_currentplayer.m_stats.addCurrency(-weaponlist[index].ammoprice);
			//add ammo (amount that goes in one clip) to certain weapon
			Debug.Log ("first: " + m_currentplayer.m_inventory.inventory [index].ammo);
			m_currentplayer.m_inventory.inventory[index].ammo += weaponlist[index].ammoInClip;
			Debug.Log (m_currentplayer.m_inventory.inventory [index].ammo);
		}
    }
}
