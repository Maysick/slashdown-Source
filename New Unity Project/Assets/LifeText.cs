using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeText : MonoBehaviour {

    Text text;

    private void Awake() {
        text = GetComponent<Text>();
    }

    private void Update() {
        text.text = "x " + GameManager.instance.player.health + "/" + GameManager.instance.player.maxHealth;
    }
}
