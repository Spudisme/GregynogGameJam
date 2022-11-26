using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int typeOfPickup;
    public int worth;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            addPickup(collision.gameObject.GetComponent<PMCharacter>());
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate() {
        Collider2D magnetism = Physics2D.OverlapCircle(transform.position, 2);

        if (magnetism.gameObject.layer == LayerMask.NameToLayer("Player")) {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Debug.Log(magnetism.gameObject.name);
            this.transform.position = Vector2.Lerp(transform.position, magnetism.transform.position, Time.deltaTime * 10f);
        }
    }

    private void addPickup(PMCharacter player) {
        switch(typeOfPickup) {
            case 1:
                player.AddBallots(worth);
                break;
            case 2:
                player.AddCoins(worth);
                break;
        }
    }
}
