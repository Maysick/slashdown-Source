using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingArrow : MonoBehaviour {

    public float scrollSpeed = 0.5f;

    float t = 0;

    public MeshRenderer mr;

    private bool mouseDown;
    private bool charging;

    public bool Charging {
        get {
            return charging;
        }

        set {
            charging = value;
            CheckVisibility();
        }
    }

    public bool MouseDown {
        get {
            return mouseDown;
        }

        set {
            mouseDown = value;
            CheckVisibility();
        }
    }

    private void Awake() {
        mr.enabled = false;
    }

    public void CheckVisibility() {
        if (MouseDown && !Charging && !GameManager.instance.player.disableControl)
            mr.enabled = true;
        else mr.enabled = false;
    }

    public void SetSize(float f) {
        f = Mathf.Max(0.01f, f);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, f * 0.3f);
        mr.material.SetTextureScale("_MainTex", new Vector2(1, f));
    }

    private void Update() {

        if (Input.GetMouseButtonDown(0)) MouseDown = true;
        else if (Input.GetMouseButtonUp(0)) MouseDown = false;

        t += Time.deltaTime * scrollSpeed;

        mr.material.mainTextureOffset = new Vector2(0, t);
    }
}
