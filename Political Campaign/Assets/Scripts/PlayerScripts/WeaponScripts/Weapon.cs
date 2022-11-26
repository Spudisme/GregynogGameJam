using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    public int powerOfBullet;
    public float collisionSize;
    public int id;
    public int attackDamage;
    public string weaponName;
    public Sprite weaponSprite;
    public Sprite weaponHandSprite;
    public Sprite weaponShopIcon;
    public string description;
    public int averageWeaaponPrice;
    public float weaponCooldown;
    public int currentAmmo;
    public int maxAmmo;
    public float attackStrength;
    public Vector3 currentRotation;

    public float GetAttackStrength() {
        return attackStrength;
    }

    public int GetAttackDamage() {
        return attackDamage;
    }

    public void setRotation(Vector3 rotation) {
        this.currentRotation = rotation;
    }

    public Sprite GetSprite() {
        return weaponSprite;
    }

    public int GetCurrentAmmo() {
        return currentAmmo;
    }

    public int GetMaxAmmo() {
        return maxAmmo;
    }

    public int GetID() {
        return id;
    }

    public float GetCollisionSize() {
        return collisionSize;
    }

    public string GetName() {
        return weaponName;
    }

    public string GetDescription() {
        return description;
    }

    public float GetCooldown() {
        return weaponCooldown;
    }
}
