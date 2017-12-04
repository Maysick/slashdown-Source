using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float maxHealth;
    public float health;

    public int level = 1;

    public float timeBeforeSpawn;

    public bool spawned = false;

    protected virtual void Awake() {
        DisableSelf();
    }

    public virtual void Start() {
        health = maxHealth;

        Invoke("SpawnIndicator", timeBeforeSpawn);
        Invoke("Spawn", timeBeforeSpawn + 1);
    }

    public virtual void DisableSelf() {

    }

    public void SpawnIndicator() {
        Instantiate(PrefabManager.instance.GetPrefabByName("Indicator"), transform.position, PrefabManager.instance.GetPrefabByName("Indicator").transform.rotation);
    }

    public virtual void Spawn() {
        spawned = true;
    }

    protected Vector2 GetVector2Pos() {
        return new Vector2(transform.position.x, transform.position.z);
    }

    public virtual void TakeHit(float damage) {
        health = health - damage;
        if (health < 0) {
            health = 0;
            Die();
        }
    }

    public virtual void Die() {
        if (!spawned) return;
        WaveManager.instance.killCount++;
        GameManager.instance.BurstCoinsFromPosition(transform.position, level);
        Instantiate(PrefabManager.instance.GetPrefabByName("Cloud"), transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        SoundManager.instance.PlaySound("EnemyDead");
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.attachedRigidbody == null) return;
        PlayerController pc = other.attachedRigidbody.gameObject.GetComponent<PlayerController>();
        if (pc != null) {
            if (pc.charging) {
                Die();
            }
        }
    }
}
