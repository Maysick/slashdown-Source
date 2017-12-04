using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeTypes {
    MinusMaxHealth,
    SlowerCharge,
    ShorterRange,
    SlowerMove,
    MoreCoinsDropped
}

public class UpgradeManager : MonoBehaviour {

    public static UpgradeManager instance;

    public UpgradeSlot[] slots;

    public Sprite greenFrame;
    public Sprite redFrame;

    public GameObject upgradeParent;
    public GameObject continueButton;

    public void CheckIfReady() {
        bool allReady = true;
        foreach(UpgradeSlot us in slots) {
            us.CheckBuyable();
            if (us.buyable && !us.outOfStock) allReady = false;
        }

        Debug.Log(allReady);

        if (allReady) {
            continueButton.SetActive(true);
        }
    }

    public void Continue() {
        Time.timeScale = 1f;
        SoundManager.instance.PlaySound("Select");
        GameManager.instance.player.disableControl = false;
        upgradeParent.SetActive(false);
        continueButton.SetActive(false);
    }

    public void OpenUpgradeScreen() {
        Time.timeScale = 0;
        GameManager.instance.player.disableControl = true;
        upgradeParent.SetActive(true);
        CheckIfReady();
    }

    private void Awake() {
        instance = this;
    }
}
