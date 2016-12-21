using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
/// <summary>
///  Weapon constructor 
///  inherits from "Item"
/// </summary>
public class Weapon : Item {

    
    public float fireRate;          // the fire rate of the weapon 
    public float launchForce;       // the launchforce of the weapon for the bullet (speed)
    public float maxDamage;         // the damage of the weapon if there is a full hit. 
    public int clipSize;            // the size of the clip 
    public float reloadTime;        // the time it takes to reload 
    public int ammo;                // the ammo there is left (excluding the ammoInClip)
	public int ammoprice;			// the price for new ammo !!per clip!!
    public int ammoInClip;          // the ammo in the clip 
	public int maxAmmo;				// max ammo player can carry
    //Constructor
    // weapon(name , id , description , iconname ,price ,itemtype, fireratef , launchforcef , maxDamagef, reloadTimef,clipsize ,  ammo , ammoInClip)
	public Weapon(string name, int id, string description, string iconname,  int price, ItemType type, float _fireRate, float _launchForce, float _maxDamage, float _reloadTime, int _clipSize, int _ammo, int _ammoprice, int _ammoInClip, int _maxAmmo) : base(name, id, description, iconname, price, type)
    {
        fireRate = _fireRate;
        launchForce = _launchForce;
        maxDamage = _maxDamage;
        clipSize = _clipSize;
        ammo = _ammo;
		_ammoprice = _ammoprice;
        ammoInClip = _ammoInClip;
        reloadTime = _reloadTime;
		maxAmmo = _maxAmmo;
    }

    // empty Weapon constructor, needed for the inventory
    public Weapon() : base()
    {

    }
    
    /// <summary>
    /// Checks whether the weapon has ammo left in the clip 
    /// </summary>
    /// <returns> boolean </returns>
    public Boolean hasAmmo()
    {

        if (ammoInClip > 0)
            return true;
        else
            return false;
    }

    /// <summary>
    /// reloading of the weapon
    /// </summary>
    public void reload()
    {
        if (ammo > clipSize)
        {
            ammoInClip = clipSize;
            ammo -= clipSize;
        }
        else
        {
            ammoInClip = ammo;
            ammo = 0;
        }
    }
}
