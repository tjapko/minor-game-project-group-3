using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CanvasConstructionScriptV2 : MonoBehaviour
{

    //Public variables
    [Header("Public variables")]
    public float m_interval_constTimer;         //Float: interval of updating construction timer
    public float m_interval_UIUpdate;           //Float: interval of updating UI
    public float m_showRewardTime;              //Float: show reward time

    //References
    [Header("References")]
    public GameObject m_ConstructionPhaseText;  //Reference to the construction phase text
    public GameObject m_NextWavePanel;          //Reference to the next wave panel (Contains the next wave button)
    public GameObject m_ConstructionPanel;      //Reference to the construction panel that contains the construction image
    public GameObject m_CurrencyPanel;          //Reference to the currency panel
    public GameObject m_BaseUpgradePanel;       //Reference to the base upgrade panel (contains base upgrades)
    public GameObject m_BaseUpgrade;            //Reference to the base upgrade sub panel
    public GameObject m_RebuildBase;            //Reference to the rebuild base sub panel
    public GameObject m_ShopPanel;              //Reference to the shop panel (Contains the weapons)
    public GameObject m_shop;                   //Reference to the shop sub panel
    public GameObject m_shopempty;              //Reference to the empty sub panel

    //References in children
    [Header("References children")]
    public Text txt_nextwavebtn;                //Reference to the text in next wave button
    public Text txt_currency;                   //Reference to the currency text panel
    public List<Button> btn_upgradePbtn;        //References to the player upgrade buttons
    public List<Text> txt_upgradePbtn;          //References to the text in the player upgrade buttons
    public List<Button> btn_upgradeBbtn;        //References to the base upgrade buttons
    public List<Text> txt_upgradeBbtn;          //References to the text in the upgrade base buttons
    public Button btn_rebuildBase;              //Reference to the rebuild base button
    public Text txt_rebuildBase;                //Reference to the text inside the rebuild base button
    public List<Button> btn_buyweapons;         //References to the buy weapon buttons
    public List<Text> txt_buyweapons;           //References to the text in the buy weapon buttons
    public List<Button> btn_buyammo;            //References to the buy ammo buttons
    public List<Text> txt_ammoweapons;          //References to the text in the refill ammo buttons
    public Button btn_upgradeweapons;           //Reference to the upgrade weapon button
    public Text txt_upgradeweapons;             //Reference to the text in the uprade weapon button
    public Text txt_construction_1;             //Reference to the construction upgrade 1 text
    public Text txt_construction_2;             //Reference to the construction upgrade 2 text
    public Text txt_construction_3;             //Reference to the construction upgrade 3 text
    public Text txt_construction_4;             //Reference to the construction upgrade 4 text

    //Private references
    private GameManager m_gamemanager;          //Reference to the game manager
    private UserManager m_usermanager;          //Reference to the user manager
    private BaseManager m_basemanager;          //Reference to the base manager
    private PlayerManager m_playermanager;      //Reference to the playermanager (Fix when multiplayer)
    private ShopScriptV2 m_shopscript;            //Reference to the shop script
    private float m_wavedelay;                  //Reference to the wave delay
    private float m_scaleUpgradeWeaponPrice = 0.25f;

    private BaseUpgradeScript m_bUpgradeScript; //Reference to the base upgrade script
    private List<List<BaseUpgrade>> m_listPlayerUpgrades;   //List containing player upgrades
    private List<List<BaseUpgrade>> m_listBaseUpgrades;   //List containing base upgrades
    private List<Weapon> m_weaponlist;          //List of weapons that are sold

    //Private variables
    private float current_timer;                //Value of timer
    private bool base_destroyed;                //Boolean if base is destroyed

    public void StartInitialization()
    {
        //Set references
        m_gamemanager = GameObject.FindWithTag("Gamemanager").GetComponent<GameManager>();
        m_usermanager = m_gamemanager.getUserManager();
        m_basemanager = m_gamemanager.getBaseManager();
        m_bUpgradeScript = m_basemanager.m_Instance.GetComponent<BaseUpgradeScript>();
        m_shopscript = GameObject.FindWithTag("Shop").GetComponent<ShopScriptV2>();
        m_wavedelay = GameManager.m_waveDelay;

        //Initialize
        m_ConstructionPhaseText.SetActive(true);  
        m_NextWavePanel.SetActive(true);
        m_ConstructionPanel.SetActive(true);
        m_CurrencyPanel.SetActive(true);
        m_BaseUpgradePanel.SetActive(true);
        m_BaseUpgrade.SetActive(true);
        m_RebuildBase.SetActive(false);
        m_ShopPanel.SetActive(true);
        m_shop.SetActive(true);
        m_shopempty.SetActive(false);
    }

    //OnEnable Function
    void OnEnable()
    {
        //Set variables
        current_timer = m_wavedelay;

        //Invoke repeating
        InvokeRepeating("updateTimer", 0.0f, m_interval_constTimer);
        InvokeRepeating("UpdateUI", 0.0f, m_interval_UIUpdate);
    }

    //OnDisble Function
    void OnDisable()
    {
        CancelInvoke("updateTimer");
        CancelInvoke("UpdateUI");
    }

    //Updates the UI
    public void UpdateUI()
    {
        try
        {
            //Set playermanager
            if(m_playermanager == null)
            {
                m_playermanager = m_usermanager.m_playerlist[0];
            }
            //Update currency
            updateTxtCurrency();

            if (m_basemanager.m_Instance.activeSelf)
            {
                //Show/hide panels
                showShopMenu(true);
                showUpgradePanel(true);
                showEmptyShopMenu(false);
                showRebuildPanel(false);
                showConstructionPanel(true);

                //Update panels
                updateUpgradeBaseBTN();
                updatePurchaseWeaponsBTN();
                updatePurchaseAmmoBTN();
                updateConstructionCost();
                updateUpgradeShopMenu();
            } else
            {
                //Show/hide panels
                showShopMenu(false);
                showUpgradePanel(false);
                showEmptyShopMenu(true);
                showRebuildPanel(true);
                showConstructionPanel(false);

                //Update panels
                updateRebuildBaseMenu();
            }
            
        }
        catch
        {

        }


    }

    //update timer function
    private void updateTimer()
    {
        current_timer -= current_timer >= 0 ? m_interval_constTimer : 0f;
        txt_nextwavebtn.text = "Next Wave: " + current_timer;
    }

    //Update currency button text
    private void updateTxtCurrency()
    {
        txt_currency.text = "Currency : " + m_playermanager.m_stats.getCurrency();
    }

    //Update upgrade base button text : Needs fix for multiplayer or multiple upgrades
    private void updateUpgradeBaseBTN()
    {
        m_listPlayerUpgrades = m_bUpgradeScript.getPlayerUpgradeList();
        m_listBaseUpgrades = m_bUpgradeScript.getBaseUpgradeList();

        //Get upgrades
        BaseUpgrade up_player = m_listPlayerUpgrades[0][0];
        BaseUpgrade heal_player = m_listPlayerUpgrades[0][1];
        BaseUpgrade up_base = m_listBaseUpgrades[0][0];
        BaseUpgrade heal_base = m_listBaseUpgrades[0][1];

        //Set interactability
        btn_upgradePbtn[0].interactable = determineUpgradeBTNInteractable(m_playermanager, m_basemanager, up_player);
        btn_upgradePbtn[1].interactable = determineUpgradeBTNInteractable(m_playermanager, m_basemanager, heal_player);
        btn_upgradeBbtn[0].interactable = determineUpgradeBTNInteractable(m_playermanager, m_basemanager, up_base);
        btn_upgradeBbtn[1].interactable = determineUpgradeBTNInteractable(m_playermanager, m_basemanager, heal_base);

        //Set button text
        txt_upgradePbtn[0].text = determineUpgradeButtonText(m_playermanager, m_basemanager, up_player);
        txt_upgradePbtn[1].text = determineUpgradeButtonText(m_playermanager, m_basemanager, heal_player);
        txt_upgradeBbtn[0].text = determineUpgradeButtonText(m_playermanager, m_basemanager, up_base);
        txt_upgradeBbtn[1].text = determineUpgradeButtonText(m_playermanager, m_basemanager, heal_base);

    }

    //Update weapons : Needs fix for multiple upgrades
    private void updatePurchaseWeaponsBTN()
    {
        //Get weapon list
        m_weaponlist = m_shopscript.weaponsforsale;

        //Update weapon price and interactable (also ammo interactable)
        PlayerInventory inv = m_playermanager.m_inventory;
        for (int i = 0; i < m_weaponlist.Count; i++)
        {
            if (inv.InventoryContains(m_weaponlist[i]))
            {
                txt_buyweapons[i].text = "Sold out";
                btn_buyweapons[i].interactable = false;
            }
            else
            {
                txt_buyweapons[i].text = "Buy Weapon:\n " + m_weaponlist[i].price.ToString();
                btn_buyweapons[i].interactable = m_playermanager.m_stats.getCurrency() > m_weaponlist[i].price;
            }
        }

    }

    //Update ammo : Needs fix for multiple upgrades
    private void updatePurchaseAmmoBTN()
    {
        //Get weapon list
        m_weaponlist = m_shopscript.weaponsforsale;
        //Get weapons player has currently equiped
        List<Weapon> player_weapons = m_playermanager.m_inventory.inventory;

        //Update ammo price
        for (int i = 0; i < m_weaponlist.Count; i++)
        {
            int inv_index = findSameWeaponType(m_weaponlist[i].itemtype);

            //Determine if player has same weapon type in inventory
            if (inv_index == -1)
            {
                btn_buyammo[i].interactable = false;
                txt_ammoweapons[i].text = "Locked";
            }
            else
            {
                //Player has weapon, Check if player is allowed to buy ammo
                Weapon selected_weapon = player_weapons[inv_index];
                bool allowedtobuy = ((selected_weapon.ammo + selected_weapon.ammoInClip) < selected_weapon.maxAmmo);
                btn_buyammo[i].interactable = allowedtobuy;
                //Change text of button
                if (allowedtobuy)
                {
                    txt_ammoweapons[i].text = "Buy Ammo:\n" + player_weapons[inv_index].ammoprice.ToString();
                }
                else if (selected_weapon.itemtype == Item.ItemType.Empty)
                {
                    btn_buyammo[i].interactable = false;
                    txt_ammoweapons[i].text = "Locked";
                }
                else
                {
                    txt_ammoweapons[i].text = "Weapon is\n full";
                }
            }
        }
    }

    //Update Rebuild base menu
    public void updateRebuildBaseMenu()
    {
        if (m_gamemanager.getBaseManager().m_Instance.activeSelf)
        {
            txt_rebuildBase.text = "Base has\n been built";
            btn_rebuildBase.interactable = false;
        }
        else
        {
            if (m_playermanager.m_stats.m_currency >= m_shopscript.m_rebuildbasecost)
            {
                txt_rebuildBase.text = "Rebuild base:\n " + m_shopscript.m_rebuildbasecost;
                btn_rebuildBase.interactable = true;
            }
            else
            {
                txt_rebuildBase.text = "Not Enough\n funds: " + m_shopscript.m_rebuildbasecost;
                btn_rebuildBase.interactable = false;
            }
        }
    }

    //Update upgrade shop menu
    public void updateUpgradeShopMenu()
    {
        if (m_shopscript.getCurrentTier() >= m_shopscript.upgrade_cost.Length - 1)
        {
            btn_upgradeweapons.transform.parent.gameObject.SetActive(false);
            txt_upgradeweapons.text = "Max upgrade\n reached";
        }
        else
        {
            btn_upgradeweapons.transform.parent.gameObject.SetActive(true);
            btn_upgradeweapons.interactable = m_playermanager.m_stats.getCurrency() >= UpgradeShopCost();
            txt_upgradeweapons.text = "Uprade Shop: \n" + UpgradeShopCost();
        }
    }

    //update construction cost
    private void updateConstructionCost()
    {
        txt_construction_1.text = determineConstructionText(PlayerConstruction.PlayerObjectType.PlayerWall);
        txt_construction_2.text = determineConstructionText(PlayerConstruction.PlayerObjectType.PlayerCarrotField);
        txt_construction_3.text = determineConstructionText(PlayerConstruction.PlayerObjectType.PlayerMud);
        txt_construction_4.text = determineConstructionText(PlayerConstruction.PlayerObjectType.PlayerTurret);

        txt_construction_1.color = determineConstructionTextColour(PlayerConstruction.PlayerObjectType.PlayerWall);
        txt_construction_2.color = determineConstructionTextColour(PlayerConstruction.PlayerObjectType.PlayerCarrotField);
        txt_construction_3.color = determineConstructionTextColour(PlayerConstruction.PlayerObjectType.PlayerMud);
        txt_construction_4.color = determineConstructionTextColour(PlayerConstruction.PlayerObjectType.PlayerTurret);

    }

    //Determine if upgrade button should be interactable
    private bool determineUpgradeBTNInteractable(PlayerManager selected_player, BaseManager selected_base, BaseUpgrade selected_upgrade)
    {
        switch (selected_upgrade.getBaseUpgradeType())
        {
            case BaseUpgrade.BaseUpgradeType.BaseUpgrade:
                return (selected_upgrade.getPrice() != -1) && (selected_player.m_stats.m_currency >= selected_upgrade.getPrice());
            case BaseUpgrade.BaseUpgradeType.RestoreBaseHealth:
                return (selected_player.m_stats.m_currency >= selected_upgrade.getPrice()) && selected_base.m_basehealth.getCurrentHealth() < selected_base.m_basehealth.m_maxhealth;
            case BaseUpgrade.BaseUpgradeType.PlayerHealthUpgrade:
                return (selected_upgrade.getPrice() != -1) && (selected_player.m_stats.m_currency >= selected_upgrade.getPrice());
            case BaseUpgrade.BaseUpgradeType.RestorePlayerHealth:
                return (selected_player.m_stats.m_currency >= selected_upgrade.getPrice()) && selected_player.m_playerhealth.getCurrentHealth() < selected_player.m_playerhealth.m_maxHealth;
            case BaseUpgrade.BaseUpgradeType.Empty:
                return false;
            default:
                return false;
        }
    }

    //Determine text of upgrade button
    private string determineUpgradeButtonText(PlayerManager selected_player, BaseManager selected_base, BaseUpgrade selected_upgrade)
    {
        PlayerStatistics stats = selected_player.m_stats;
        PlayerHealth health = selected_player.m_playerhealth;
        Basehealth m_basehealth = selected_base.m_basehealth;

        switch (selected_upgrade.getBaseUpgradeType())
        {
            case BaseUpgrade.BaseUpgradeType.BaseUpgrade:
                if (selected_upgrade.getPrice() == -1)
                {
                    return "Max Health\n Reached";
                }
                else
                {
                    return stats.getCurrency() >= selected_upgrade.getPrice() ? selected_upgrade.getPrice().ToString() : "Not Enough\n funds " + selected_upgrade.getPrice();
                }
            case BaseUpgrade.BaseUpgradeType.RestoreBaseHealth:
                if (m_basehealth.getCurrentHealth() >= m_basehealth.m_maxhealth)
                {
                    return "Max Health";
                }
                else
                {
                    return stats.getCurrency() >= selected_upgrade.getPrice() ? selected_upgrade.getPrice().ToString() : "Not Enough\n funds " + selected_upgrade.getPrice();
                }
            case BaseUpgrade.BaseUpgradeType.PlayerHealthUpgrade:
                if (selected_upgrade.getPrice() == -1)
                {
                    return "Max Health\n Reached";
                }
                else
                {
                    return stats.getCurrency() >= selected_upgrade.getPrice() ? selected_upgrade.getPrice().ToString() : "Not Enough\n funds " + selected_upgrade.getPrice();
                }
            case BaseUpgrade.BaseUpgradeType.RestorePlayerHealth:
                if (health.getCurrentHealth() >= health.m_maxHealth)
                {
                    return "Max Health";
                }
                else
                {
                    return stats.getCurrency() >= selected_upgrade.getPrice() ? selected_upgrade.getPrice().ToString() : "Not Enough\n funds " + selected_upgrade.getPrice();
                }
            case BaseUpgrade.BaseUpgradeType.Empty:
                return "";
            default:
                return "Error";
        }
    }

    //Determine text of construction
    private string determineConstructionText(PlayerConstruction.PlayerObjectType type)
    {
        switch (type)
        {
            case PlayerConstruction.PlayerObjectType.PlayerWall:
                return PlayerConstruction.getCurrentWalls() >= PlayerConstruction.maxWalls ?
                    "Sold Out" : m_basemanager.m_Instance.activeSelf ?
                    PlayerConstruction.determinePrice(PlayerConstruction.PlayerObjectType.PlayerWall).ToString() : "Base destroyed";
            case PlayerConstruction.PlayerObjectType.PlayerTurret:
                return PlayerConstruction.getCurrentTurrets() >= PlayerConstruction.maxTurrets ?
                    "Sold Out" : m_basemanager.m_Instance.activeSelf ? 
                    PlayerConstruction.determinePrice(PlayerConstruction.PlayerObjectType.PlayerTurret).ToString() : "Base destroyed"; ;
            case PlayerConstruction.PlayerObjectType.PlayerCarrotField:
                return PlayerConstruction.getCurrentCarrotsFarms() >= PlayerConstruction.maxCarrotFarms ?
                    "Sold Out" : m_basemanager.m_Instance.activeSelf ? 
                    PlayerConstruction.determinePrice(PlayerConstruction.PlayerObjectType.PlayerCarrotField).ToString() : "Base destroyed"; ;
            case PlayerConstruction.PlayerObjectType.PlayerMud:
                return PlayerConstruction.getCurrentMud() >= PlayerConstruction.maxMudPools ?
                    "Sold Out" : m_basemanager.m_Instance.activeSelf ? 
                    PlayerConstruction.determinePrice(PlayerConstruction.PlayerObjectType.PlayerMud).ToString() : "Base destroyed";
            default:
                return "";

        }
    }

    //Determine colour of construction
    private Color determineConstructionTextColour(PlayerConstruction.PlayerObjectType type)
    {
        int playermoney = m_playermanager.m_stats.getCurrency();
        switch (type)
        {
            case PlayerConstruction.PlayerObjectType.PlayerWall:
                return PlayerConstruction.getCurrentWalls() >= PlayerConstruction.maxWalls ?
                    Color.black : playermoney >= PlayerConstruction.determinePrice(PlayerConstruction.PlayerObjectType.PlayerWall) ?
                    Color.black : Color.red;
            case PlayerConstruction.PlayerObjectType.PlayerTurret:
                return PlayerConstruction.getCurrentTurrets() >= PlayerConstruction.maxTurrets ?
                    Color.black : playermoney >= PlayerConstruction.determinePrice(PlayerConstruction.PlayerObjectType.PlayerTurret) ?
                    Color.black : Color.red;
            case PlayerConstruction.PlayerObjectType.PlayerCarrotField:
                return PlayerConstruction.getCurrentCarrotsFarms() >= PlayerConstruction.maxCarrotFarms ?
                    Color.black : playermoney >= PlayerConstruction.determinePrice(PlayerConstruction.PlayerObjectType.PlayerCarrotField) ?
                    Color.black : Color.red;
            case PlayerConstruction.PlayerObjectType.PlayerMud:
                return PlayerConstruction.getCurrentMud() >= PlayerConstruction.maxMudPools ?
                    Color.black : playermoney >= PlayerConstruction.determinePrice(PlayerConstruction.PlayerObjectType.PlayerMud) ?
                    Color.black : Color.red;
            default:
                return Color.black;
        }
    }

    //Find the index of a certain weapon type in the player inventory
    private int findSameWeaponType(Item.ItemType itemtype)
    {
        List<Weapon> inv = m_playermanager.m_inventory.inventory;

        for (int i = 0; i < inv.Count; i++)
        {
            if (inv[i].itemtype.Equals(itemtype))
            {
                return i;
            }

        }
        //random number to indicate that the player doesn't have such a weapon;
        return -1;
    }

    //Function to purchase weapon (button)
    public void purchase_weapon(int index)
    {
        m_weaponlist = m_shopscript.weaponsforsale;
        //First check if weapon exists in the list
        if (m_weaponlist.Count >= index)
        {
            //Check if player has empty slot
            bool empty_slot = m_playermanager.m_inventory.InventoryContains(new Weapon());

            //Player has empty slot
            if (empty_slot)
            {
                //Check if player has enough money and inventory does not contain the weapon already
                if (m_playermanager.m_stats.getCurrency() >= m_weaponlist[index].price &&
                  !m_playermanager.m_inventory.InventoryContains(m_weaponlist[index]))
                {
                    m_playermanager.m_stats.addCurrency(-1 * m_weaponlist[index].price);
                    m_playermanager.m_inventory.addItem(m_weaponlist[index]);
                }
            }
            else
            {
                //Check if player has enough money and inventory does not contain the weapon already
                if (m_playermanager.m_stats.getCurrency() >= m_weaponlist[index].price &&
                  !m_playermanager.m_inventory.InventoryContains(m_weaponlist[index]))
                {
                    m_playermanager.m_stats.addCurrency(-1 * m_weaponlist[index].price);
                    m_playermanager.m_inventory.inventory[0] = m_weaponlist[index];
                }
            }
        }
    }

    //Function to purchase ammo (button)
    public void purchase_ammo(int weaponIndex)
    {
        //Get player inventory
        int inventoryIndex = findSameWeaponType(m_weaponlist[weaponIndex].itemtype);

        if (inventoryIndex != -1)
        {
            // check whether player got enough money & doesn't have max ammo already
            if (m_playermanager.m_stats.getCurrency() >= m_weaponlist[weaponIndex].ammoprice)
            {
                Weapon selected_weapon = m_playermanager.m_inventory.inventory[inventoryIndex];

                if ((selected_weapon.ammo + selected_weapon.ammoInClip) < selected_weapon.maxAmmo)
                {
                    //decrease currency
                    m_playermanager.m_stats.addCurrency(-m_weaponlist[weaponIndex].ammoprice);

                    //add ammo (amount that goes in one clip) to certain weapon
                    if (selected_weapon.ammo + selected_weapon.ammoInClip + selected_weapon.clipSize > selected_weapon.maxAmmo)
                    {
                        m_playermanager.m_inventory.inventory[inventoryIndex].ammo = selected_weapon.maxAmmo - selected_weapon.ammoInClip;
                    }
                    else
                    {
                        m_playermanager.m_inventory.inventory[inventoryIndex].ammo += m_weaponlist[weaponIndex].clipSize;
                    }
                }
            }
        }
    }

    //Upgrade store function
    public void upgradeStore()
    {
        int upgradeCost = UpgradeShopCost();
        if (m_playermanager.m_stats.getCurrency() >= upgradeCost)
        {
            //Substract currency
            m_playermanager.m_stats.addCurrency(-upgradeCost);
            //Increment tier (weapons are loaded automatically)
            m_shopscript.incTier();
        }
    }

    //UpradeCost of shop
    private int UpgradeShopCost()
    {
        List<Weapon> inv = m_playermanager.m_inventory.inventory;
        int weaponsCost = 0;
        foreach (Weapon i in inv)
        {
            weaponsCost += upgradeprice(i);
        }
        
        int upgradecost = (int)(m_shopscript.upgrade_cost[m_shopscript.getCurrentTier()] + weaponsCost * m_scaleUpgradeWeaponPrice);

        return upgradecost;
    }

    //Function upgrade price
    private int upgradeprice(Weapon weapon)
    {
        if (weapon.itemtype == Item.ItemType.Laser)
        {
            return Laser.price[m_shopscript.getCurrentTier()];
        }
        if (weapon.itemtype == Item.ItemType.MachineGun)
        {
            return MachineGun.price[m_shopscript.getCurrentTier()];
        }
        if (weapon.itemtype == Item.ItemType.Shotgun)
        {
            return ShotGun.price[m_shopscript.getCurrentTier()];
        }
        else return 0;
    }

    //Sets base_destroyed
    public void setBaseDestroyed(bool status)
    {
        base_destroyed = status;
    }

    //Rebuilds the base
    public void rebuildBase()
    {
        if (m_playermanager.m_stats.m_currency >= m_shopscript.m_rebuildbasecost)
        {
            m_playermanager.m_stats.substractCurrency(m_shopscript.m_rebuildbasecost);
            m_basemanager.m_basehealth.OnRebuild();
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

    //Buy upgrade
    private void selectUpgrade(BaseUpgrade selected_upgrade)
    {
        PlayerStatistics player_stats = m_playermanager.m_stats;

        switch (selected_upgrade.getBaseUpgradeType())
        {
            case BaseUpgrade.BaseUpgradeType.BaseUpgrade:
                Upgrade_Base upgrade_1 = (Upgrade_Base)selected_upgrade;

                if (upgrade_1.getPrice() != -1 && player_stats.getCurrency() >= upgrade_1.getPrice())
                {
                    player_stats.substractCurrency(upgrade_1.getPrice());
                    //				BaseTurret baseTurretScript = m_base.GetComponentsInChildren<BaseTurret> ()[0];
                    upgrade_1.UpgradeBase(m_basemanager.m_Instance);

                }
                break;
            case BaseUpgrade.BaseUpgradeType.RestoreBaseHealth:
                Restore_BaseHealth upgrade_2 = (Restore_BaseHealth)selected_upgrade;
                if (upgrade_2.getPrice() != -1 && player_stats.getCurrency() >= upgrade_2.getPrice())
                {
                    upgrade_2.restoreBaseHealth(m_basemanager.m_Instance, m_playermanager);
                }
                break;
            case BaseUpgrade.BaseUpgradeType.PlayerHealthUpgrade:
                Upgrade_PlayerHealth upgrade_3 = (Upgrade_PlayerHealth)selected_upgrade;
                if (upgrade_3.getPrice() != -1 && player_stats.getCurrency() >= upgrade_3.getPrice())
                {

                    player_stats.substractCurrency(upgrade_3.getPrice());
                    upgrade_3.upgradePlayerHealth(m_playermanager);
                }
                break;
            case BaseUpgrade.BaseUpgradeType.RestorePlayerHealth:
                Restore_PlayerHealth upgrade_4 = (Restore_PlayerHealth)selected_upgrade;

                if (upgrade_4.getPrice() != -1 && player_stats.getCurrency() >= upgrade_4.getPrice())
                {
                    upgrade_4.restorePlayerHealth(m_playermanager);
                }

                break;
            case BaseUpgrade.BaseUpgradeType.Empty:
                break;
            default:
                break;
        }
    }

    //Show shop menu
    private void showShopMenu(bool status)
    {
        m_shop.SetActive(status);

    }

    //Show empty shop meu
    private void showEmptyShopMenu(bool status)
    {
        m_shopempty.SetActive(status);
    }

    //Show rebuild panel
    private void showRebuildPanel(bool status)
    {
        m_RebuildBase.SetActive(status);
    }

    //show upgrade panel
    private void showUpgradePanel(bool status)
    {
        m_BaseUpgrade.SetActive(status);
    }

    //Show construction panel
    private void showConstructionPanel(bool status)
    {
        m_ConstructionPanel.SetActive(status);
    }

    //Show wave reward
    public IEnumerator showWaveReward()
    {
        CancelInvoke("UpdateUI");

        if(m_playermanager != null)
        {
            float waveReward = m_usermanager.waveCurrency();
            float carrotReward = m_playermanager.m_construction.getCarrots();
            txt_currency.text = "Wave: +" + waveReward + ", Carrot Fields: +" + carrotReward;
            yield return new WaitForSeconds(m_showRewardTime);
        }
        
        InvokeRepeating("UpdateUI", 0.0f, 0.1f);

    }
}