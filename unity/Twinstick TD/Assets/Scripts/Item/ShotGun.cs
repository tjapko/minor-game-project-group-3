using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
/// <summary>
///  Weapon constructor 
///  inherits from "Item"
/// </summary>
public class ShotGun {

	//public variables 
	public static int price = 4000;
	public static float fireRate = 1f;          // the fire rate of the weapon 
	public static float launchForce = 50f;       // the launchforce of the weapon for the bullet (speed)
	public static float maxDamage = 2f;         // the damage of the weapon if there is a full hit. 
	public static float reloadTime = 10f;        // the time it takes to reload 
	public static int ammoprice = 10;			// the price for new ammo !!per clip!!
	public static int maxAmmo = 200;				// max ammo player can carry
	public static float bulletLifeTime = 0.4f;    // the lifetime of the bullet 
	// upgradable stats (ammo & ammoInClip)
	public static int ammo1 = 20;                // the ammo there is left (excluding the ammoInClip) level1
	public static int ammo2 = 35;                // level2 after the 1st upgrade
	public static int ammo3 = 50;                // level3 after the 2nd upgrade
	public static int ammoInClip1 = 10;             // the ammo in the clip level1
	public static int ammoInClip2 = 15;             // level2 after the 1st upgrade 
	public static int ammoInClip3 = 20;             // level3 after the 2nd upgrade
    public static int clipSize1 = 20;            // the size of the clip 
    public static int clipSize2 = 20;           // level2 after the 1st upgrade
    public static int clipSize3 = 20;           // level3 after the 2nd upgrade
}