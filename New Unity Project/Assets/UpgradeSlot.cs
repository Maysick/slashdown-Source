using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UpgradeSlot : MonoBehaviour {

    public UpgradeTypes type;

    public int current = 0;
    public int[] costs;

    public bool buyable = false;
    public bool outOfStock = false;

    private Image frameImage;
    public Text cost;

    private void Awake() {
        frameImage = GetComponent<Image>();
    }

    private void Start() {
        CheckBuyable();
    }

    public void CheckBuyable() {
        if (current >= costs.Length) {
            buyable = false;
            cost.text = "x X";
            outOfStock = true;
            frameImage.sprite = UpgradeManager.instance.redFrame;
            return;
        }
        buyable = GameManager.instance.player.coins >= costs[current];
        cost.text = "x " + costs[current];
        if (!buyable) {
            frameImage.sprite = UpgradeManager.instance.redFrame;
        } else {
            frameImage.sprite = UpgradeManager.instance.greenFrame;
        }
        
    }

    public void Buy() {
        CheckBuyable();
        if (buyable) {
            SoundManager.instance.PlaySound("Select");
            GameManager.instance.player.coins -= costs[current];
            current++;

            if (type == UpgradeTypes.MinusMaxHealth) {
                GameManager.instance.player.maxHealth -= 1;
                GameManager.instance.player.health = GameManager.instance.player.maxHealth;
            } else if (type == UpgradeTypes.MoreCoinsDropped) {
                GameManager.instance.coinBonus++;
            } else if (type == UpgradeTypes.ShorterRange) {
                GameManager.instance.player.maxMoveDist *= 0.9f;
            } else if (type == UpgradeTypes.SlowerMove) {
                GameManager.instance.player.xSpeed *= 0.9f;
                GameManager.instance.player.ySpeed *= 0.9f;
            } else if (type == UpgradeTypes.SlowerCharge) {
                GameManager.instance.player.chargeSpeed *= 0.9f;
            }


            CheckBuyable();
            UpgradeManager.instance.CheckIfReady();
        }
    }
}
