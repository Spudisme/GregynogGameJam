using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFadeScript : MonoBehaviour
{
    GameObject player;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerExit2D(Collider2D other) {
        anim.SetTrigger("Leaves");
    }

    public void RemoveObj() {
        Destroy(this.gameObject);
    }
}
