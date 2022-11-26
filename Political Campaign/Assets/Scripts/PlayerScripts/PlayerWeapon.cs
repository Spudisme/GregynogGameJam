using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    // Weapon Ideas:
    // Tea
    // All the milk margerate took
    // Lies
    // Fists
    //Store all possible weapons and their buttels aswell as the function for their alt-fire.
    Weapon currentWeapon;
    Animator weaponAnimator;
    Vector3 hitbox;
    List<Weapon> weaponNameFromID = new List<Weapon>();

    public void init() {
        PencilWeapon pw = new PencilWeapon();
        weaponNameFromID.Add(new PencilWeapon());
    }

    public void equiptWeapon(int id) {
        currentWeapon = weaponNameFromID[id];
        this.transform.GetChild(2).GetChild(id).gameObject.SetActive(true);
        
        weaponAnimator = this.transform.GetChild(2).GetChild(id).GetComponent<Animator>();
    }

    void Update() {
        hitbox = this.transform.GetChild(2).GetChild(currentWeapon.GetID()).GetChild(0).transform.position;
    }

    public void PencilAttack() {
        Collider2D[] blockCollision = Physics2D.OverlapCircleAll(hitbox, currentWeapon.GetCollisionSize(), LayerMask.GetMask("Object"));
        if (blockCollision != null) {
            foreach(Collider2D objCollider in blockCollision) {
                if (objCollider.GetComponent<Obstical>().GetPhysics())
                objCollider.gameObject.GetComponent<Rigidbody2D>().AddForce((objCollider.transform.position - hitbox).normalized * currentWeapon.GetAttackStrength());
                objCollider.GetComponent<Obstical>().OnHit(currentWeapon.GetAttackDamage());
            }
        }

        Collider2D[] enemyCollision = Physics2D.OverlapCircleAll(hitbox, currentWeapon.GetCollisionSize(), LayerMask.GetMask("Enemy"));
        if (enemyCollision != null) {
            foreach(Collider2D enemiesCol in enemyCollision) {
                enemiesCol.gameObject.GetComponent<Rigidbody2D>().AddForce((enemiesCol.transform.position - hitbox).normalized * currentWeapon.GetAttackStrength());
                enemiesCol.GetComponent<EnemyHealth>().OnHit(currentWeapon.GetAttackDamage());
            }
        }
    }

    public Weapon GetCurrentWeapon() {
        return currentWeapon;
    }

    public void OnAttack() {
        weaponAnimator.SetTrigger("Attack");
        print(currentWeapon.GetID());
        switch (currentWeapon.GetID()) {
            case 0:
                PencilAttack();
                break;
        }
    }
}
