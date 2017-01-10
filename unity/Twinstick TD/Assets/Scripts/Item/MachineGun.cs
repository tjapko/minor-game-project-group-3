using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
/// <summary>
///  Weapon constructor 
///  inherits from "Item"
/// </summary>
public class MachineGun {

	//public variables 
	public static int price = 6000;
	public static float fireRate = 10f;          // the fire rate of the weapon 
	public static float launchForce = 50f;       // the launchforce of the weapon for the bullet (speed)
	public static float maxDamage = 0.25f;         // the damage of the weapon if there is a full hit. 
	public static float reloadTime = 1f;        // the time it takes to reload 
	public static int clipSize = 60;            // the size of the clip 
	public static int ammoprice = 20;			// the price for new ammo !!per clip!!
	public static int maxAmmo = 480;				// max ammo player can carry
	public static float bulletLifeTime = 2f;    // the lifetime of the bullet 
	// upgradable stats (ammo & ammoInClip)
	public static int ammo1 = 120;                // the ammo there is left (excluding the ammoInClip) level1
	public static int ammo2 = 160;                // level2 after the 1st upgrade
	public static int ammo3 = 200;                // level3 after the 2nd upgrade
	public static int ammoInClip1 = 60;             // the ammo in the clip level1
	public static int ammoInClip2 = 80;             // level2 after the 1st upgrade 
	public static int ammoInClip3 = 100;            // level3 after the 1st upgrade
}