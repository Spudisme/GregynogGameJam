using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player = null;
    Camera cam;
    public float normalThreshold;
    public float scopedThreshold;
    public float speed = 5f;
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player == null) {
            player = GameObject.Find("Player(Clone)");
        } else {
            float threshold = normalThreshold;
            if (Input.GetMouseButton(1)) {
                threshold = scopedThreshold;
            }
            
            Vector3 mosuePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPos = (player.transform.position + mosuePos) / 2f;

            targetPos.x = Mathf.Clamp(targetPos.x, -threshold + player.transform.position.x, threshold + player.transform.position.x);
            targetPos.y = Mathf.Clamp(targetPos.y, -threshold + player.transform.position.y, threshold + player.transform.position.y);
            targetPos.z = -5;

            this.transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
        }
    }
}
