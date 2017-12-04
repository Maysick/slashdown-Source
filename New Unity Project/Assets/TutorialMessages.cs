using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMessages : MonoBehaviour {

    string[] messages = {
        "Move with WASD.\n",
        "Move with WASD.\nClick to slash.",
        "Defeat enemies and\n collect their coins.",
        "Buy upgrades with\n coins you collect.",
    };

    public Text text;
    Image image;
    public float textDelay = 2f;

    private void Awake() {
        image = GetComponent<Image>();
        StartCoroutine(AdvanceText());
        image.enabled = false;
        text.enabled = false;
    }

    IEnumerator AdvanceText() {
        yield return new WaitForSeconds(2f);

        image.enabled = true;
        text.enabled = true;

        for (int i = 0; i < messages.Length; i++) {
            yield return new WaitForSeconds(textDelay);
            text.text = messages[i];
        }

        yield return new WaitForSeconds(textDelay);

        while(GameManager.instance.player.coins < 1) {
            yield return new WaitForSeconds(0.5f);        
        }

        
        yield return new WaitForSeconds(0.25f);
        gameObject.SetActive(false);
        WaveManager.instance.BeginGame();

    }
}
