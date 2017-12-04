using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour {

    private float t;
    public GameObject grow;

    private void Awake() {
        
    }

    private void Update() {
        t = Mathf.Clamp(t + Time.deltaTime, 0, 1);
        float f = Mathf.Lerp(0.1f, 0.92f, t);
        grow.transform.localScale = new Vector3(f, f, 1);
        if (t >= 1) {
            Destroy(gameObject);
        }
    }
}
