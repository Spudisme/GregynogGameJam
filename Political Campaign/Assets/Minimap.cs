using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    public DungeonMapGenerator dmg;
    private GameObject player;
    private Texture2D texture2D;
    private RawImage imageNode;
    int mapRadius;
    void Start()
    {
        texture2D = new Texture2D(60, 60);
        imageNode = GetComponent<RawImage>();
        texture2D.filterMode = FilterMode.Point;
        imageNode.texture = texture2D;
        mapRadius = 60;
        for (int i = 0; i < mapRadius; i++) {
            for (int j = 0; j < mapRadius; j++) {
                Color color = new Color(0, 0, 0, 0);
                texture2D.SetPixel(i, j, color);
            }
        }
        texture2D.Apply();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player == null) {
            player = GameObject.Find("Player(Clone)");
        } else {
            int[,] graph = dmg.GetMap();
            for (int i = 0; i < mapRadius; i++) {
                for (int j = 0; j < mapRadius; j++) {
                    if (!(player.transform.position.x + i - (mapRadius / 2) > 99 || player.transform.position.x + i - (mapRadius / 2) < 0 
                    || player.transform.position.y + j - (mapRadius / 2) > 99 || player.transform.position.y + j - (mapRadius / 2) < 0)) {
                        if (graph[(int) player.transform.position.x + i - (mapRadius / 2),(int) player.transform.position.y + j - (mapRadius / 2)] != 0) {
                            Color color = Color.gray;
                            texture2D.SetPixel(i, j, color);
                        } else {
                            Color color = new Color(0, 0, 0, 0);
                            texture2D.SetPixel(i, j, color);
                        }
                    }
                }
            }
            texture2D.Apply();
        }
    }
}
