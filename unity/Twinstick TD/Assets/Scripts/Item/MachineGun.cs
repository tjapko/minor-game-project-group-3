using UnityEngine;
using System.Collections;

public class MachineGun: Item , I_Weapon{

    private float fireRate;
    private float maxDamage;

    public MachineGun(string name, int id, string description, string iconname, ItemType type) : base(name, id , description , iconname , type)
    {
        
    }




}
