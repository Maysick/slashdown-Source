using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour {

    private bool tracking = true;
    public float timeBeforeFire = 2f;
    public float fireDelay = 0.5f;
    public float fireDeath = 0.5f;
    private float t = 0;
    bool fired = false;

    public SpriteRenderer sr;

    public BoxCollider bc;

    public Sprite firedSprite;

    private void Awake() {
        sr.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update () {
        t += Time.deltaTime;
        if (tracking) {
            float f = -90 + GameManager.instance.AngleBetweenVectors(GameManager.instance.player.transform.position, transform.position);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, f, transform.eulerAngles.z);
            if (t >= timeBeforeFire) {
                tracking = false;
            }
        }

        if (!fired){
            sr.color = new Color(1, 1, 1, 1f / ((timeBeforeFire + fireDelay) / t));
            sr.gameObject.transform.localScale = new Vector3(sr.gameObject.transform.localScale.x, 0.5f / ((timeBeforeFire + fireDelay) / t), sr.gameObject.transform.localScale.z);
            if (t >= (timeBeforeFire + fireDelay))
                Fire();
        }
        
	}

    void Fire() {
        fired = true;
        bc.enabled = true;
        sr.color = Color.white;
        sr.sprite = firedSprite;
        Destroy(gameObject, fireDeath);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.attachedRigidbody == null) return;
        PlayerController pc = other.attachedRigidbody.gameObject.GetComponent<PlayerController>();
        if (pc != null) {
            bc.enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            pc.TakeHit();
        }
    }
}
