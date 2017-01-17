﻿using UnityEngine;
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
	public static float reloadTime = 1f;        // the time it takes to reload 
	public static int ammoprice = 20;			// the price for new ammo !!per clip!!
	
	public static float bulletLifeTime = 2f;    // the lifetime of the bullet 
                                               
    // upgradable stats (ammo & ammoInClip)
    public static int[] ammo = { 120, 200, 300};                // all the different ammo levels 
    public static int[] ammoInClip = { 60, 80, 90 };            // all ammo from weapon when you buy it 
    public static int[] clipSize = { 60, 80, 90 };              // all ammo in clip when you buy them 
    public static int[] maxAmmo = { 180, 160, 250 };            // max ammo player can carry for every level
    public static float[] maxDamage = {0.25f, 0.4f, 0.7f };     // the damage for every level  
    public static int[] price = { 6000, 7000, 10000 };          // the price of the weapon for every level
    public static float[] fireRate = { 10f , 10f , 12f };       // the fire rate of the weapon 
}