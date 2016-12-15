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
    public ShopScript m_shopscript;         //Reference to ShopScript, which contains the weapons (Set by the shop)
    public PlayerManager m_currentplayer;    //Reference to the player that has opened the UI
    private GameObject weaponIconsTab;      //Reference to the Weapons Icon Tab
    private GameObject weaponPurchaseTab;   //Reference to the Weapon purchase buttons tab
    private GameObject ammoPurchaseTab;     //Reference to the Ammo purchase buttons tab

    //Variables
    private List<Item> weaponlist;
    private List<Item> ammolist;

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
        }
	}

    //Function to purchase weapon (button)
    public void purchase_weapon(int index)
    {
        
        if(m_currentplayer.m_stats.getCurrency() >= weaponlist[index].itemprice &&
            !m_currentplayer.m_inventory.InventoryContains(weaponlist[index]))
        {
            m_currentplayer.m_stats.addCurrency(-1 * weaponlist[index].itemprice);
            m_currentplayer.m_inventory.addItem(weaponlist[index]);
        }
        
    }
}