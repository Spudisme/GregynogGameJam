using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMCharacter : MonoBehaviour
{
    public int health;
    public int maxHealth = 7;
    int ballots;
    public float runSpeed = 20f;
    public int attackDamage = 10;
    float sphereColliderSize = 0.8f;
    public float attackStrength = 0.5f;
    Animator animator;
    Camera cam;
    Animator weaponAnimator;
    GameObject playerSprite;
    GameObject weapon;
    Vector3 weaponColliderPos;
    Rigidbody2D body;
    float horizontal;
    LayerMask objectLayer;
    float vertical;
    GameObject lr;

    // Start is called before the first frame update
    void Start()
    {
        objectLayer = LayerMask.GetMask("Object");
        lr = gameObject.transform.GetChild(0).gameObject;
        body = GetComponent<Rigidbody2D>();
        playerSprite = gameObject.transform.GetChild(1).gameObject;
        animator = gameObject.transform.GetChild(1).GetComponent<Animator>();
        weaponAnimator = gameObject.transform.GetChild(2).GetChild(0).GetComponent<Animator>();
        weapon = gameObject.transform.GetChild(2).gameObject;
        weaponColliderPos = gameObject.transform.GetChild(2).GetChild(0).GetChild(0).gameObject.transform.position;
        cam = GameObject.Find("Camera").GetComponent<Camera>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = cam.ScreenToWorldPoint(Input.mousePosition) - weapon.transform.position;
        float angle =  Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        weapon.transform.rotation = rotation;
        if(dir.x < 0) {
            weapon.transform.localScale = new Vector3(1, -1, 1);
        } else {
            weapon.transform.localScale = new Vector3(1, 1, 1);
        }
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (Input.GetMouseButtonDown(0)) {
            Attack();
        }

        if(horizontal != 0 || vertical != 0) {
            animator.SetBool("walking", true);
            weaponAnimator.SetBool("walking", true);
        } else {
            animator.SetBool("walking", false);
            weaponAnimator.SetBool("walking", false);
        }
        if (horizontal > 0 || dir.x >= 0) {
            playerSprite.transform.localScale = Vector3.Lerp (playerSprite.transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 10);
        } else if (horizontal < 0 || dir.x < 0) {
            playerSprite.transform.localScale = Vector3.Lerp (playerSprite.transform.localScale, new Vector3(-1, 1, 1), Time.deltaTime * 10);
        } else {
            if (playerSprite.transform.localScale.x > 0) {
                playerSprite.transform.localScale = new Vector3(1f, 1f, 1);
            } else {
                playerSprite.transform.localScale = new Vector3(-1f, 1f, 1);
            }
        }
    }

    public int GetHealth() {
        return health;
    }

    private void Attack() {
        weaponAnimator.SetTrigger("Attack");
        Collider2D[] blockCollision = Physics2D.OverlapCircleAll(transform.position, sphereColliderSize, objectLayer);
        if (blockCollision != null) {
            foreach(Collider2D objCollider in blockCollision) {
                if (objCollider.GetComponent<Obstical>().GetPhysics())
                objCollider.gameObject.GetComponent<Rigidbody2D>().AddForce((objCollider.transform.position - transform.position).normalized * attackStrength);
                objCollider.GetComponent<Obstical>().OnHit(attackDamage);
            }
        }

        Collider2D[] enemyCollision = Physics2D.OverlapCircleAll(transform.position, sphereColliderSize, LayerMask.GetMask("Enemy"));
        if (enemyCollision != null) {
            foreach(Collider2D enemiesCol in enemyCollision) {
                enemiesCol.gameObject.GetComponent<Rigidbody2D>().AddForce((enemiesCol.transform.position - transform.position).normalized * attackStrength);
                enemiesCol.GetComponent<EnemyHealth>().OnHit(attackDamage);
            }
        }
    }
    private void FixedUpdate() {
        body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
        if (Input.GetMouseButton(1)) {
            lr.SetActive(true);
        } else {
            lr.SetActive(false);
        }
    }
}
