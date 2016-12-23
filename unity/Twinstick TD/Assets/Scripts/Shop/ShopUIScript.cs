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
    private GameObject weaponText;          //Reference to the weapon text
    private GameObject weaponPurchaseTab;   //Reference to the Weapon purchase buttons tab
    private GameObject ammoText;            //Reference to the ammo text
    private GameObject ammoPurchaseTab;     //Reference to the Ammo purchase buttons tab

    //Icons
    private Image icon_1;
    private Image icon_2;
    private Image icon_3;

    //Text
    private Text weapontext_1;
    private Text weapontext_2;
    private Text weapontext_3;
    private Text ammotext_1;
    private Text ammotext_2;
    private Text ammotext_3;

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
        weaponText = gameObject.transform.GetChild(0).GetChild(3).gameObject;
        weaponPurchaseTab = gameObject.transform.GetChild(0).GetChild(4).gameObject;
        ammoText = gameObject.transform.GetChild(0).GetChild(5).gameObject;
        ammoPurchaseTab = gameObject.transform.GetChild(0).GetChild(6).gameObject;

        icon_1 = weaponIconsTab.transform.GetChild(0).GetComponent<Image>();
        icon_2 = weaponIconsTab.transform.GetChild(1).GetComponent<Image>();
        icon_3 = weaponIconsTab.transform.GetChild(2).GetComponent<Image>();

        weapontext_1 = weaponText.transform.GetChild(0).GetComponent<Text>();
        weapontext_2 = weaponText.transform.GetChild(1).GetComponent<Text>();
        weapontext_3 = weaponText.transform.GetChild(2).GetComponent<Text>();

        ammotext_1 = ammoText.transform.GetChild(0).GetComponent<Text>();
        ammotext_2 = ammoText.transform.GetChild(1).GetComponent<Text>();
        ammotext_3 = ammoText.transform.GetChild(2).GetComponent<Text>();

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
            icon_1.sprite = weaponlist[0].itemicon;
            icon_2.sprite = weaponlist[1].itemicon;
            icon_3.sprite = weaponlist[2].itemicon;

            weapontext_1.text = weaponlist[0].itemprice.ToString();
            weapontext_2.text = weaponlist[1].itemprice.ToString();
            weapontext_3.text = weaponlist[2].itemprice.ToString();

            ammotext_1.text = weaponlist[0].ammoprice.ToString();
            ammotext_2.text = weaponlist[1].ammoprice.ToString();
            ammotext_3.text = weaponlist[2].ammoprice.ToString();

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
		
	private int findIndex(int weaponIndex){
		for (int i = 0; i < 3; i++) {
			if (m_currentplayer.m_inventory.inventory [i].itemtype == weaponlist [weaponIndex].itemtype) {
				return i;
			}
		}
		//random number more than weapons in list thatś only returned when the weapon is nog yet in the inventory of player
		return 100;
	}

    //Function to purchase ammo (button)
	public void purchase_ammo(int weaponIndex) {
		//check where what weapon is in the inventory
		int inventoryIndex = findIndex(weaponIndex);
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
}
