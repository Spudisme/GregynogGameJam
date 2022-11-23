using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomActivation : MonoBehaviour
{
    Room theRoom;

    DungeonMapGenerator dmg;

    public void init(DungeonMapGenerator dmg, Room room) {
        this.dmg = dmg;
        this.theRoom = room;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            foreach(GameObject enemy in theRoom.GetEnemiesInRoom()) {
                Vector2 enemyVec = new Vector2(Mathf.RoundToInt(enemy.transform.position.x), Mathf.RoundToInt(enemy.transform.position.y));
                Vector2 playerVec = new Vector2(Mathf.RoundToInt(other.transform.position.x), Mathf.RoundToInt(other.transform.position.y));
                List<Vector2> listToMove = AStarPathfinding.startPathFiding(enemyVec, playerVec, "Enemy", theRoom, dmg.GetMap());
                enemy.GetComponent<EnemyMovement>().SetMovementPath(listToMove);
                enemy.GetComponent<EnemyMovement>().SetTarget(other.gameObject);
            }
        }
    }

}
