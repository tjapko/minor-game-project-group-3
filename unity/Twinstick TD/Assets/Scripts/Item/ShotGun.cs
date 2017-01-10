using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
/// <summary>
///  Weapon constructor 
///  inherits from "Item"
/// </summary>
public class ShotGun : Weapon {

	//public variables 
	public static int price = 4000;
	public static float fireRate = 1f;          // the fire rate of the weapon 
	public static float launchForce = 50f;       // the launchforce of the weapon for the bullet (speed)
	public static float maxDamage = 2f;         // the damage of the weapon if there is a full hit. 
	public static float reloadTime = 10f;        // the time it takes to reload 
	public static int clipSize = 20;            // the size of the clip 
	public static int ammo = 30;                // the ammo there is left (excluding the ammoInClip)
	public static int ammoprice = 10;			// the price for new ammo !!per clip!!
	public static int ammoInClip = 20;          // the ammo in the clip 
	public static int maxAmmo = 200;				// max ammo player can carry
	public static float bulletLifeTime = 0.4f;    // the lifetime of the bullet 

	//Constructor
	public ShotGun(string name, int id, string description, string iconname,  int _price, ItemType type, float _fireRate, float _launchForce, float _maxDamage, float _reloadTime, int _clipSize, int _ammo, int _ammoprice, int _ammoInClip, int _maxAmmo, float _bulletLifeTime)
	{
		name = "Default Weapon 2";
		id = 2;
		description = "Default weapon!";
		iconname = "shotgun";
		price = _price;
		type = Weapon.ItemType.Shotgun;
		fireRate = _fireRate;
		launchForce = _launchForce;
		maxDamage = _maxDamage;
		reloadTime = _reloadTime;
		clipSize = _clipSize;
		ammo = _ammo;
		ammoprice = _ammoprice;
		ammoInClip = _ammoInClip;
		maxAmmo = _maxAmmo;
		bulletLifeTime = _bulletLifeTime;
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
