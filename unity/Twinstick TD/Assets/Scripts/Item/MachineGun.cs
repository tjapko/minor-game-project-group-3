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
	public static float launchForce = 50f;       // the launchforce of the weapon for the bullet (speed)
	public static float reloadTime = 0f;        // the time it takes to reload 
	public static int ammoprice = 40;			// the price for new ammo !!per clip!!
	public static float bulletLifeTime = 2f;    // the lifetime of the bullet 
    // upgradable stats (ammo & ammoInClip)
    public static int[] ammo = { 180, 360, 720};                // all the different ammo levels 
    public static int[] ammoInClip = { 60, 90, 120 };            // all ammo from weapon when you buy it 
    public static int[] clipSize = { 60, 120, 240 };              // all ammo in clip when you buy them 
    public static int[] maxAmmo = { 180, 360, 720 };            // max ammo player can carry for every level
    public static float[] maxDamage = {1f, 2f, 3f };     // the damage for every level  
    public static int[] price = { 6000, 7000, 8000,0 };          // the price of the weapon for every level
    public static float[] fireRate = { 10f , 12f , 15f };       // the fire rate of the weapon 
}