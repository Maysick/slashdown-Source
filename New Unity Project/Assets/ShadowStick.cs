using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowStick : MonoBehaviour {

    private float initY;

    private void Awake() {
        initY = transform.position.y;
    }

    private void Update() {
        transform.position = new Vector3(transform.position.x, initY, transform.position.z);
    }
}
