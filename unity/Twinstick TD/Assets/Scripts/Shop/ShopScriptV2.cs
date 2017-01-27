using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Shopscript
/// Contains the basic information of weapons and ammo that are sold in the shop
/// Allows the user to 
/// </summary>
public class ShopScriptV2 : MonoBehaviour {
    //Temp variables 
    string use_button = "f";    //Set to use button of player[i]

    //Public variables
    [HideInInspector] public List<Weapon> weaponsforsale;
    public int m_rebuildbasecost;   //Rebuild base cost 
    public int[] upgrade_cost;      //Upgrade to next tier [0] should be empty

    //References
    private GameManager m_gamemanager;  // Reference to the game manager
    private UserManager m_usermanager;  // Reference to the user manager

    //Private variables 
    private Weapon weapon1, weapon2, weapon3, weapon4;
    private int current_tier;   //int : current tier of purchasable weapons (starts at 1)


    // Use this for initialization
    public void Start() {
        //Instantiate Lists
        weaponsforsale = new List<Weapon>();

        //Set references
        m_gamemanager = GameObject.FindWithTag("Gamemanager").GetComponent<GameManager>();
        m_usermanager = m_gamemanager.getUserManager();
        
        //Set variables
        current_tier = 1;

        //        For testing
        //      ............. Weapon(name               , id , description       , iconname  , price            , itemtype                   , fireratef           , launchforcef           , maxDamagef           , reloadTimef           , clipsize            , ammo            , ammopriceperclip     , ammoInClip            , maxAmmo            , lifetime)
        weapon1 = new Weapon("Default Weapon", 1, "Default weapon!", "Weapon1", HandGun.price, Weapon.ItemType.HandGun, HandGun.fireRate, HandGun.launchForce, HandGun.maxDamage, HandGun.reloadTime, HandGun.clipSize, HandGun.ammo, HandGun.ammoprice, HandGun.ammoInClip, HandGun.maxAmmo, HandGun.bulletLifeTime);
        weapon4 = new Weapon("Default Weapon 4", 4, "Default weapon!", "laser", Laser.price[0], Weapon.ItemType.Laser, Laser.fireRate[0], Laser.launchForce, Laser.maxDamage[0], Laser.reloadTime, Laser.clipSize[0], Laser.ammo[0], Laser.ammoprice, Laser.ammoInClip[0], Laser.maxAmmo[0], Laser.bulletLifeTime);
        weapon2 = new Weapon("Default Weapon 2", 2, "Default weapon!", "shotgun", ShotGun.price[0], Weapon.ItemType.Shotgun, ShotGun.fireRate[0], ShotGun.launchForce, ShotGun.maxDamage[0], ShotGun.reloadTime, ShotGun.clipSize[0], ShotGun.ammo[0], ShotGun.ammoprice, ShotGun.ammoInClip[0], ShotGun.maxAmmo[0], ShotGun.bulletLifeTime);
        weapon3 = new Weapon("Default Weapon 3", 3, "Default weapon!", "Weapon3", MachineGun.price[0], Weapon.ItemType.MachineGun, MachineGun.fireRate[0], MachineGun.launchForce, MachineGun.maxDamage[0], MachineGun.reloadTime, MachineGun.clipSize[0], MachineGun.ammo[0], MachineGun.ammoprice, MachineGun.ammoInClip[0], MachineGun.maxAmmo[0], MachineGun.bulletLifeTime);
		weaponsforsale.Add((Weapon)weapon1);
		weaponsforsale.Add((Weapon)weapon4);
		weaponsforsale.Add((Weapon)weapon2);
		weaponsforsale.Add((Weapon)weapon3);

    }

    //Upgrade function

    public void Upgrade(int level)
    {
        SetGun(level, "ammo");
        SetGun(level, "price");
        SetGun(level, "firerate");
        SetGun(level, "maxdamage");
        SetGun(level, "clipsize");
        SetGun(level, "ammoinclip");
        SetGun(level, "maxammo");

        List<Weapon> temp = new List<Weapon>();
        temp.Add((Weapon)weapon1);
        temp.Add((Weapon)weapon4);
        temp.Add((Weapon)weapon2);
        temp.Add((Weapon)weapon3);

        weaponsforsale = temp;
    }

    // sets the ammo for weapons1-4 
    private void SetGun(int level, string specification)
    {
        switch (specification.ToLower())
        {
            case "ammo":
                // Set ammo a level higher for all weapons
                weapon2.ammo = ShotGun.ammo[level];
                weapon3.ammo = MachineGun.ammo[level];
                weapon4.ammo = Laser.ammo[level];
                break;

        // Set ammoInClip a level higher for all weapons
            case "ammoinclip":
                weapon2.ammoInClip = ShotGun.ammoInClip[level];
                weapon3.ammoInClip = MachineGun.ammoInClip[level];
                weapon4.ammoInClip = Laser.ammoInClip[level];
                break;
            // Set clipSize a level higher  for all weapons
            case "slipsize":
                weapon2.clipSize = ShotGun.clipSize[level];
                weapon3.clipSize = MachineGun.clipSize[level];
                weapon4.clipSize = Laser.clipSize[level];
                break;
            // Set Damage a level higher for all weapons 
            case "maxdamage":
                weapon2.maxDamage = ShotGun.maxDamage[level];
                weapon3.maxDamage = MachineGun.maxDamage[level];
                weapon4.maxDamage = Laser.maxDamage[level];
                break;
            // Set the price a level higher for all weapons 
            case "price":
                weapon2.price = ShotGun.price[level];
                weapon3.price = MachineGun.price[level];
                weapon4.price = Laser.price[level];
                break;

            // Set the fireRate a level higher for all weapons
            case "firerate":
                weapon2.fireRate = ShotGun.fireRate[level];
                weapon3.fireRate = MachineGun.fireRate[level];
                weapon4.fireRate = Laser.fireRate[level];
                break;

            // Set the maxAmmo a level higher for all weapons
            case "maxammo":
                weapon2.maxAmmo = ShotGun.maxAmmo[level];
                weapon3.maxAmmo = MachineGun.maxAmmo[level];
                weapon4.maxAmmo = Laser.maxAmmo[level];
                break;

            case "id":
                weapon2.itemID += weaponsforsale.Count;
                weapon3.itemID += weaponsforsale.Count;
                weapon4.itemID += weaponsforsale.Count;
                break;

        }
    }



    //Increase tier and load new weapons
    public void incTier()
    {
        Upgrade(current_tier);
        current_tier++;
    }

    //Getter for current_tier
    public int getCurrentTier()
    {
        return current_tier;
    }

    //Reset shop
    public void resetShop()
    {
        Start();
    }
}
