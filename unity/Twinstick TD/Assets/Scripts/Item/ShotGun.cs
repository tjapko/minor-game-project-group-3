using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
/// <summary>
///  Weapon constructor 
///  inherits from "Item"
/// </summary>
public class ShotGun
{

    //public variables 
    public static float launchForce = 50f;       // the launchforce of the weapon for the bullet (speed)
    public static float reloadTime = 1.5f;        // the time it takes to reload 
    public static int ammoprice = 30;           // the price for new ammo !!per clip!!  
    public static float bulletLifeTime = 0.4f;    // the lifetime of the bullet 
    // upgradable stats (ammo & ammoInClip)
    public static int[] maxAmmo = { 100, 150 , 250 };// max ammo player can carry for every level
    public static float[] maxDamage = { 1f, 2f, 3f };// the damage for every level  
public static int[] price = { 4000, 5000, 6000 };//  the prices for every level
	public static int[] ammo = { 40, 65, 105 };      // all the different ammo levels 
    public static int[] ammoInClip = { 10, 15, 20 }; // all ammo from weapon when you buy it 
    public static int[] clipSize = { 10, 15, 20 };   // all ammo in clip when you buy them 
    public static float[] fireRate = { 1f,1.5f ,3f };// the fire rate of the weapon

}