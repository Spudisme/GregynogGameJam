using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    DungeonMapGenerator dmg;
    int[,] map;
    Room room;
    Animator animator;
    float speed = 1f;
    Vector2 moveTo;
    Rigidbody2D body;
    int movementIndex;
    public List<Vector2> movementQueue = new List<Vector2>();
    GameObject targetObject;
    GameObject enemySprite;
    float movementChangeCooldown;
    public void MoveTo(Vector2 moveTo) {
        this.moveTo = moveTo;
    }

    private void Start() {
        enemySprite = this.transform.GetChild(0).gameObject;
        animator = this.transform.GetChild(0).GetComponent<Animator>();
        movementIndex = 0;
        moveTo = transform.position;
        body = GetComponent<Rigidbody2D>();
        movementChangeCooldown = Time.time + .2f;
    }

    public void SetTarget(GameObject targetobj) {
        targetObject = targetobj;
    }

    public void init(DungeonMapGenerator dmg, int[,] map, Room room) {
        this.dmg = dmg;
        this.map = map;
        this.room = room;
    }

    public void SetMovementPath(List<Vector2> list) {
        movementIndex = 0;
        movementQueue = list;
    }

    private void FixedUpdate() {
        animator.SetBool("Walking", false);
        if (targetObject != null) {
            if (Vector2.Distance(transform.position, targetObject.transform.position) < 20 && movementChangeCooldown <= Time.time) {
                movementIndex = 0;
                Vector2 enemyVec = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
                Vector2 playerVec = new Vector2(Mathf.RoundToInt(targetObject.transform.localPosition.x), Mathf.RoundToInt(targetObject.transform.localPosition.y));
                movementQueue = AStarPathfinding.startPathFiding(enemyVec, playerVec, "Enemy", room, dmg.GetMap());
                movementChangeCooldown = Time.time + .4f;
            }
        }
        if (movementQueue.Count > 0) {
            Vector2 target = movementQueue[movementIndex];
            if (Vector2.Distance(transform.position, target) > 1.2f) {
                Vector2 moveDir = (target - (Vector2) transform.position).normalized;
                if ((target - (Vector2) transform.position).x > 0) {
                   enemySprite.transform.localScale = new Vector3(3.13f, 3.13f, 1);
                } else {
                    enemySprite.transform.localScale = new Vector3(-3.13f, 3.13f, 1);
                }
                float distanceBefore = Vector2.Distance(transform.position, target);
                body.velocity = moveDir * speed;
                animator.SetBool("Walking", true);
            } else {
                movementIndex++;
                if (movementIndex >= movementQueue.Count) {
                    movementQueue = new List<Vector2>();
                }
            }
        } else if (targetObject != null) { 
            if (Vector2.Distance(transform.position, targetObject.transform.position) > 1.2f) {
                Vector2 moveDir = ((Vector2) targetObject.transform.position - (Vector2) transform.position).normalized;
                if (((Vector2) targetObject.transform.position - (Vector2) transform.position).x < 0) {
                   enemySprite.transform.localScale = new Vector3(3.13f, 3.13f, 1);
                } else {
                    enemySprite.transform.localScale = new Vector3(-3.13f, 3.13f, 1);
                }
                body.velocity = moveDir * speed;
                animator.SetBool("Walking", true);
            }
        }
    }

    private void OnDrawGizmos() {
        for(int i = 0; i < movementQueue.Count - 1; i++) {
            Gizmos.DrawLine(movementQueue[i], movementQueue[i + 1]);
        }
    }
}
