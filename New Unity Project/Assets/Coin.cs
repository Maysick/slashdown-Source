using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public bool canBeCollected;
    public float timeBeforeCanBeCollected = 1f;

    private void Awake() {
        StartCoroutine(EnableCollection());
    }

    IEnumerator EnableCollection() {
        yield return new WaitForSeconds(timeBeforeCanBeCollected);
        canBeCollected = true;
    }

    public void Collect() {
        if (!canBeCollected) return;
        GameManager.instance.player.coins++;
        SoundManager.instance.PlaySound("Coin");
        Destroy(gameObject);
    }
}
