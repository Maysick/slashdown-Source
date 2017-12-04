using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WaveManager : MonoBehaviour {

    public static WaveManager instance;

    public GameObject[] waves;
    private int currentWave = 0;

    public float waveCheckTime = 2f;

    private bool running = false;

    public float randomSpawnInterval = 3f;

    public GameObject[] randomEnemies;

    public bool endless = true;

    public int killCount = 0;

    public Text kc;

    IEnumerator SpawnRandom() {
        while (true) {
            Vector2 random = Random.insideUnitCircle * GameManager.instance.radius;
            Vector3 pos = new Vector3(random.x, 0.57f, random.y);
            GameObject rand = randomEnemies[Random.Range(0, randomEnemies.Length)];
            Instantiate(rand, pos, rand.transform.rotation);
            yield return new WaitForSeconds(randomSpawnInterval);
            randomSpawnInterval = Mathf.Clamp(randomSpawnInterval * 0.9f, 1f, 3f);
        }
    }

    public void Awake() {
        instance = this;
    }

    IEnumerator CheckWaveDone() {
        while (running) {
            yield return new WaitForSeconds(waveCheckTime);
            if (waves[currentWave].transform.childCount == 0) {
                WaveComplete();
            }
        }
    }

    void WaveComplete() {
        GameManager.instance.player.health = GameManager.instance.player.maxHealth;
        if (currentWave + 1 == waves.Length) {
            StartCoroutine(SpawnRandom());
            running = false;
            endless = true;
            kc.enabled = true;
        } else {
            currentWave++;
            UpgradeManager.instance.OpenUpgradeScreen();
            waves[currentWave].SetActive(true);
        }
    }

    public void BeginGame() {
        running = true;
        UpgradeManager.instance.OpenUpgradeScreen();
        waves[currentWave].SetActive(true);
        StartCoroutine(CheckWaveDone());
    }

}
