using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilWeapon : Weapon
{
    public PencilWeapon() {
        this.powerOfBullet = 10;
        this.id = 0;
        this.weaponName = "Pencil";
        this.averageWeaaponPrice = 10;
        this.description = "Just a regular old pencil";
        this.weaponCooldown = 1f;
        this.maxAmmo = 1;
        this.currentAmmo = maxAmmo;
        this.attackDamage = 10;
        this.attackStrength = 10000;
        this.collisionSize = 0.8f;
    }
}
