using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Weapon : Item {

    public float maxBulletDistance;


    public float fireRate;
    public float launchForce;
    public float maxDamage;
    public int clipSize;
    public float reloadTime;
    public int ammo;
    public int ammoInClip;
    // weapon(name , id , description , iconname , itemtype, price , fireratef , launchforcef , maxDamagef, reloadTimef,clipsize ,  ammo , ammoInClip, bullet , m_bullet)
    public Weapon(string name, int id, string description, string iconname,  int price,ItemType type, float _fireRate, float _launchForce, float _maxDamage, float _reloadTime, int _clipSize, int _ammo, int _ammoInClip) : base(name, id, description, iconname, price, type)
    {
        fireRate = _fireRate;
        launchForce = _launchForce;
        maxDamage = _maxDamage;
        clipSize = _clipSize;
        ammo = _ammo;
        ammoInClip = _ammoInClip;
        reloadTime = _reloadTime;
    }

    public Weapon() : base()
    {

    }

    public Boolean hasAmmo()
    {

        if (ammoInClip > 0)
            return true;
        else
            return false;
    }

    public void reload()
    {
        if (ammo > clipSize)
        {
            ammoInClip = clipSize;
            ammo = -clipSize;
        }
        else
        {
            ammoInClip = ammo;
            ammo = 0;
        }
    }
}
