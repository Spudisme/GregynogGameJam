using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStorage : MonoBehaviour
{
    public List<GameObject> itemsHeld;

    public void SpawnItems() {
        foreach(GameObject go in itemsHeld) {
            for(int i = 0; i < Random.Range(3, 7); i++) {
                Vector3 offset = new Vector3(Random.Range(2, -2), Random.Range(2, -2), -1);
                GameObject ga = Instantiate(go, transform.position, Quaternion.identity);
                ga.GetComponent<Rigidbody2D>().AddForce(offset * 90);
            }
        }
    }
}
