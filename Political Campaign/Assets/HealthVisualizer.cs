using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthVisualizer : MonoBehaviour
{
    private GameObject player;
    public Image playerHealth;
    public Sprite playerHealthHalf;
    public Sprite playerHealthSprite;
    private int currentHealth = 0;
    public GameObject heartSpawnPoint;
    public int spacing;

    private List<Image> currentlyLoadedHearts = new List<Image>();

    void FixedUpdate()
    {
        if (player == null) {
            player = GameObject.Find("Player(Clone)");
        } else {
            if (currentHealth != player.GetComponent<PMCharacter>().GetHealth()) {
                UpdateHealth();
            }
        }
    }

    private void UpdateHealth() {
        int health = player.GetComponent<PMCharacter>().GetHealth();
        currentHealth = health;
        foreach(Image img in currentlyLoadedHearts) {
            img.gameObject.SetActive(false);
        }
        for (int i = 0; i < (health + (health % 2)) / 2; i++) {
            if (i < currentlyLoadedHearts.Count) {
                currentlyLoadedHearts[i].gameObject.SetActive(true);
                currentlyLoadedHearts[i].sprite = playerHealthSprite;
            } else {
                Image heart = Instantiate(playerHealth, heartSpawnPoint.transform, true);
                Canvas canvas = GetComponent<Canvas>();
                heart.GetComponent<RectTransform>().SetPositionAndRotation(heartSpawnPoint.transform.position + new Vector3(i * 40 * canvas.scaleFactor, 0, 0), Quaternion.identity);
                heart.transform.localScale = heart.transform.localScale * canvas.scaleFactor;
                currentlyLoadedHearts.Add(heart);
            }
        }
        if ((health % 2) == 1) {
            currentlyLoadedHearts[(health + (health % 2)) / 2 - 1].sprite = playerHealthHalf;
        }
    }
}
