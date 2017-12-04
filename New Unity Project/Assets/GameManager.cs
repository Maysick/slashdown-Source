using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameObject debugPrefab;

    public PlayerController player;

    public float radius;

    public GameObject tilePrefab;

    public int tileRadius = 20;

    public int coinBonus = 0;

    public Vector2 ClampInsideRange(Vector2 pos) {
        if (pos.magnitude > radius) {
            return pos.normalized * radius;
        } else return pos;
    }

    public void BurstCoinsFromPosition(Vector3 position, int num) {
        GameObject coinFab = PrefabManager.instance.GetPrefabByName("Coin");
        for (int i = 0; i < num + coinBonus; i++) {
            GameObject newCoinObject = Instantiate(coinFab, position, coinFab.transform.rotation) as GameObject;
            Vector2 rand = RandomPointInRing(1, 2);
            Vector3 force = new Vector3(rand.x, 7f, rand.y);
            force *= 0.85f;
            newCoinObject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }
    }

    public float AngleBetweenVectors(Vector2 a, Vector2 b) {
        return -90 - (180 / Mathf.PI * Mathf.Atan2(a.y - b.y, a.x - b.x));
    }

    public float AngleBetweenVectors(Vector3 b, Vector3 a) {
        return -90 - (180 / Mathf.PI * Mathf.Atan2(a.z - b.z, a.x - b.x));
    }

    public Vector2 RandomPointInRing(float innerRadius, float outerRadius) {
        float randomAngle = Random.value * Mathf.PI * 2;
        Vector2 randomRingPoint = new Vector2(Mathf.Cos(randomAngle) * (innerRadius + (Random.value * (outerRadius - innerRadius))),
            Mathf.Sin(randomAngle) * (innerRadius + (Random.value * (outerRadius - innerRadius))));
        return randomRingPoint;
    }

    private void Awake() {
        instance = this;
       
        /*for (int x = 0; x < tileRadius; x++) {
            for (int y = 0; y < tileRadius; y++) {
                Vector3 position = new Vector3(x - (tileRadius / 2f), 0.495f, y - (tileRadius / 2f));
                position.x *= 2;
                position.z *= 2;
                Instantiate(tilePrefab, position, tilePrefab.transform.rotation);
            }
        }*/
        
    }
}
