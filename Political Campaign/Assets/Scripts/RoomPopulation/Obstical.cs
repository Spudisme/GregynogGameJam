using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstical : MonoBehaviour
{
    public int health;
    public bool physics;
    public Room room;
    public void SetRoom(Room room) {
        this.room = room;
    }
    public int GetHealth() {
        return health;
    }

    public bool GetPhysics() {
        return physics;
    }

    private void Update() {
        if (health <= 0) {
            room.GetObjectsInRoom().Remove(this.gameObject);
            Destroy(this.gameObject);
            if (this.GetComponent<ItemStorage>()) {
                this.GetComponent<ItemStorage>().SpawnItems();
            }
        }
    }

    public void OnHit(int attackDamage) {
        health -= attackDamage;
    }
}
