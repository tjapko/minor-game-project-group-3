using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
/// <summary>
///  Weapon constructor 
///  inherits from "Item"
/// </summary>
public class Sniper {

	//public variables 
	public static int price = 2000;
	public static float fireRate = 1.0f;          // the fire rate of the weapon 
	public static float launchForce = 100f;       // the launchforce of the weapon for the bullet (speed)
	public static float maxDamage = 10f;         // the damage of the weapon if there is a full hit. 
	public static float reloadTime = 1f;        // the time it takes to reload 
	            // the size of the clip 
	public static int ammoprice = 20;			// the price for new ammo !!per clip!!
	public static int maxAmmo = 50;				// max ammo player can carry
	public static float bulletLifeTime = 3f;    // the lifetime of the bullet 
	// upgradable stats (ammo & ammoInClip)
	public static int ammo1 = 10;                // the ammo there is left (excluding the ammoInClip) level1
	public static int ammo2 = 15;                // level2 after the 1st upgrade
	public static int ammo3 = 20;                // level3 after the 2nd upgrade
	public static int ammoInClip1 = 5;             // the ammo in the clip level1
	public static int ammoInClip2 = 10;             // level2 after the 1st upgrade 
	public static int ammoInClip3 = 15;             // level3 after the 2nd upgrade
    public static int clipSize1 = 5;            // the clipsize of the weapon level1
    public static int clipSize2 = 10;           // level2 after the 1st upgrade 
    public static int clipSize3 = 15;           // level3 after the 2nd upgrade 
}
