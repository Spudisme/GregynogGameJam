using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudScript : MonoBehaviour
{
    GameObject player;
    public Text coinText;
    public Text WeaponText;
    public Text ballotText;
    Text ammoText;
    Text playerName;
    // Start is called before the first frame update
    void FixedUpdate() {
        if (player == null) {
            player = GameObject.Find("Player(Clone)");
        } else {
            PMCharacter playerInfoClass = player.GetComponent<PMCharacter>();
            coinText.text = ": " + playerInfoClass.GetCoins();
            ballotText.text = ": " + playerInfoClass.GetBallots();
        }
    }
}
