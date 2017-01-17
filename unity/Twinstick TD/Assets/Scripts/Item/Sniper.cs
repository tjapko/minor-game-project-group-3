﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
/// <summary>
///  Weapon constructor 
///  inherits from "Item"
/// </summary>
public class Sniper {

    //public variables 
         
    public static float launchForce = 100f;     // the launchforce of the weapon for the bullet (speed)
    public static float reloadTime = 1f;        // the time it takes to reload                       
    public static int ammoprice = 20;           // the price for new ammo !!per clip!!
    public static float bulletLifeTime = 3f;    // the lifetime of the bullet 

    // upgradable stats (ammo & ammoInClip)

    public static float[] fireRate = { 1.0f, 1.5f , 2.5f };    // the fire rate of the weapon 
    public static int[] ammo = { 10, 15, 20};           // all the different ammo levels 
    public static int[] ammoInClip = { 5, 10, 15 };     // all ammo from weapon when you buy it 
    public static int[] clipSize = { 5, 10, 15 };       // all ammo in clip when you buy them 
    public static int[] maxAmmo = { 50, 75, 100 };      // max ammo player can carry for every level
    public static float[] maxDamage = { 10, 11f, 13f }; // the damage for every level
    public static int[]  price = { 2000, 3500 , 6000};  // the price of the weapon for every level  
}