﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
/// <summary>
///  Weapon constructor 
///  inherits from "Item"
/// </summary>
public class Laser {

    //public variables 
         
    public static float launchForce = 100f;     // the launchforce of the weapon for the bullet (speed)
	public static float reloadTime = 0.8f;        // the time it takes to reload                       
    public static int ammoprice = 20;           // the price for new ammo !!per clip!!
    public static float bulletLifeTime = 4f;    // the lifetime of the bullet 
    // upgradable stats (ammo & ammoInClip)
    public static float[] fireRate = { 1.0f, 2.0f , 4.0f };    // the fire rate of the weapon 
	public static int[] ammo = { 100, 200, 400 };           // all the different ammo levels 
    public static int[] ammoInClip = { 20, 30, 40 };     // all ammo from weapon when you buy it 
    public static int[] clipSize = { 20, 30, 40 };       // all ammo in clip when you buy them 
    public static int[] maxAmmo = { 100, 200, 400 };      // max ammo player can carry for every level
    public static float[] maxDamage = { 3f, 4f, 6f }; // the damage for every level
    public static int[]  price = { 2000, 3000 , 5000,0};  // the price of the weapon for every level  
}
