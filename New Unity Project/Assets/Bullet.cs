using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float angle;
    public float angleRad;
    public float speed;

    private bool fired = false;

    public SpriteRenderer sr;

    public float animTiming = 0.1f;

    public Sprite[] sprites;
    public int spriteI;

    void Awake() {
        sr.enabled = false;
        Destroy(gameObject, 10f);
    }

    public void Fire() {
        //transform.localEulerAngles = new Vector3(0, 0, angle);
        fired = true;
        angle = 90 - angle;
        angleRad = angle / 180 * Mathf.PI;
        sr.enabled = true;
    }

    private void Update() {
        if (fired) {
            Vector2 movementVector = new Vector2(Mathf.Cos(angleRad) * speed * Time.deltaTime, Mathf.Sin(angleRad) * speed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x + movementVector.x, transform.position.y, transform.position.z + movementVector.y);
        }
    }


    IEnumerator DeathAnimation() {
        for (int i = 1; i < sprites.Length; i++) {
            sr.sprite = sprites[i];
            yield return new WaitForSeconds(animTiming);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.attachedRigidbody == null) return;
        PlayerController pc = other.attachedRigidbody.gameObject.GetComponent<PlayerController>();
        if (pc != null) {
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            fired = false;
            StartCoroutine(DeathAnimation());
            pc.TakeHit();
        }
    }
}
