using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
/// <summary>
///  Weapon constructor 
///  inherits from "Item"
/// </summary>
public class HandGun {

	//public variables 
	public static int price = 100;
	public static float fireRate = 7f;          // the fire rate of the weapon 
	public static float launchForce = 75f;       // the launchforce of the weapon for the bullet (speed)
	public static float maxDamage = 0.5f;         // the damage of the weapon if there is a full hit. 
	public static float reloadTime = 0.5f;        // the time it takes to reload 
	public static int clipSize = 20;            // the size of the clip 
	public static int ammoprice = 50;			// the price for new ammo !!per clip!
	public static int maxAmmo = 500;				// max ammo player can carry
	public static float bulletLifeTime = 2f;    // the lifetime of the bullet 
	// upgradable stats (ammo & ammoInClip)
	public static int ammo1 = 9999;                // the ammo there is left (excluding the ammoInClip) level1
	public static int ammo2 = 9999;                // level2 after the 1st upgrade
	public static int ammo3 = 9999;                // level3 after the 2nd upgrade
	public static int ammoInClip1 = 20;             // the ammo in the clip level1
	public static int ammoInClip2 = 25;             // level2 after the 1st upgrade 
	public static int ammoInClip3 = 30;             // level3 after the 1st upgrade
}
