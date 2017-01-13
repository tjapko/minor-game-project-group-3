using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
/// <summary>
///  Weapon constructor 
///  inherits from "Item"
/// </summary>
public class HandGun
{

    //public variables 
    public static int price = 100;
    public static float fireRate = 7f;          // the fire rate of the weapon 
    public static float launchForce = 75f;       // the launchforce of the weapon for the bullet (speed)
    public static float maxDamage = 0.5f;         // the damage of the weapon if there is a full hit. 
    public static float reloadTime = 0.5f;        // the time it takes to reload 
    public static int ammoprice = 50;           // the price for new ammo !!per clip!
    public static int maxAmmo = 500;                // max ammo player can carry
    public static float bulletLifeTime = 2f;    // the lifetime of the bullet 
                                                // upgradable stats (ammo & ammoInClip)

    public static int ammo = 9999;       // all the different ammo levels 
    public static int ammoInClip =20;    // all ammo from weapon when you buy it 
    public static int clipSize =  20;    // all ammo in clip when you buy them 
   
}
