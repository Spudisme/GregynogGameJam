using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    Room roomRef;
    int health = 40;

    public void init(Room room) {
        this.roomRef = room;
    }

    private void Update() {
        if (health <= 0) {
            roomRef.GetEnemiesInRoom().Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void OnHit(int damageTaken) {
        health -= damageTaken;
    }
}
