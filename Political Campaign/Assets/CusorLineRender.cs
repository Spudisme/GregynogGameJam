using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CusorLineRender : MonoBehaviour
{
    // Start is called before the first frame update
    LineRenderer lr;
    public Material[] m1;
    int maxIndex = 7;
    int currentIndex = 1;
    Camera cam;
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        cam = GameObject.Find("Camera").GetComponent<Camera>();
        InvokeRepeating("changeMat", 0f, 0.02f);
    }

    private void changeMat() {
        currentIndex--;
        if (currentIndex == 0) {
            currentIndex = maxIndex;
        }
        lr.material = m1[currentIndex];
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, transform.position + new Vector3(0, 0, 0.1f));
        lr.SetPosition(1, cam.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 4));
    }
}
