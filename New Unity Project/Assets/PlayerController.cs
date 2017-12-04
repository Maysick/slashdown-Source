using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float xSpeed;
    public float ySpeed;

    public float chargeSpeed = 12;

    public float magicConstant = 2;

    public float slowDist = 2f;

    public float minMoveDist;
    public float maxMoveDist;

    public LayerMask mouseLayers;

    public GameObject arrowParent;

    public bool charging;
    Vector2 targetLocation;
    Vector2 startPosition;

    public float chargeMoveThreshold;

    public ScrollingArrow arrow;

    public bool canMove = true;

    public Animator animator;

    public int health;
    public int maxHealth;
    public bool invincible = false;

    public float flashTime = 0.2f;
    public int flashes = 5;

    public SpriteRenderer sr;

    public int coins = 0;

    public bool disableControl = true;

    [HideInInspector]
    public bool dead = false;

    public bool Charging {
        get {
            return charging;
        }

        set {
            charging = value;
            arrow.Charging = value;
            animator.SetBool("Charging", value);
        }
    }

    IEnumerator DelayEnableInput() {
        yield return new WaitForSeconds(2f);
        disableControl = false;
    }

    private void Awake() {
        StartCoroutine(DelayEnableInput());
    }

    private void Update() {

        Vector3 mouseHitPosition = Vector3.zero;
        bool mouseHit = false; //shouldn't ever be false

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000f, mouseLayers.value)) {
            Vector2 mod = GameManager.instance.ClampInsideRange(new Vector2(hit.point.x, hit.point.z));
            mouseHitPosition = new Vector3(mod.x, 0, mod.y);
            mouseHit = true;
            //Instantiate(GameManager.instance.debugPrefab, hit.point, Quaternion.identity);
        }

        if (!disableControl && mouseHit) {
            float rot = GameManager.instance.AngleBetweenVectors(transform.position, mouseHitPosition);
            arrowParent.transform.localEulerAngles = new Vector3(arrowParent.transform.rotation.x, rot, arrowParent.transform.rotation.z);
            float dist = Vector2.Distance(new Vector2(mouseHitPosition.x, mouseHitPosition.z), GetVector2Pos());
            dist /= magicConstant;
            dist = Mathf.Clamp(dist, minMoveDist, maxMoveDist);
            arrow.SetSize(dist);
        }

        if (!disableControl && Camera.main.WorldToScreenPoint(transform.position).x > Input.mousePosition.x) {
            animator.transform.localScale = new Vector3(-1, 1, 1);
        } else animator.transform.localScale = new Vector3(1, 1, 1);

        if (!disableControl && Input.GetMouseButtonUp(0)) {
            if (!Charging && canMove)
                BeginCharge(new Vector2(mouseHitPosition.x, mouseHitPosition.z));
        }

        if (Charging) {
            Charge();
        }

        animator.SetBool("Walking", false);

        if (!disableControl && canMove) {
            Vector2 rawInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (rawInput.magnitude > 1f) {
                rawInput.Normalize();
            }

            Vector2 movement = new Vector2(rawInput.x * xSpeed, rawInput.y * ySpeed);

            Move(movement);
        }

        Vector2 clampedPos = GameManager.instance.ClampInsideRange(GetVector2Pos());
        transform.position = new Vector3(clampedPos.x, transform.position.y, clampedPos.y);
    }

    private Vector2 GetVector2Pos() {
        return new Vector2(transform.position.x, transform.position.z);
    }

    private void Move(Vector2 movement) {
        if (movement.magnitude >= 0.1f) {
            animator.SetBool("Walking", true);
        }
        Vector2 adjustedMove = movement * Time.deltaTime;
        transform.position = new Vector3(transform.position.x + adjustedMove.x, transform.position.y, transform.position.z + adjustedMove.y);
    }

    private void BeginCharge(Vector2 location) {
        SoundManager.instance.PlaySound("Slash1");
        float dist = Vector2.Distance(location, GetVector2Pos()) / magicConstant;
        if (dist < minMoveDist) {
            location = GetVector2Pos() + ((location - GetVector2Pos()).normalized * minMoveDist * magicConstant);
        } else if (dist > maxMoveDist) {
            location = GetVector2Pos() + ((location - GetVector2Pos()).normalized * maxMoveDist * magicConstant);
        }

        Charging = true;
        canMove = false;
        targetLocation = new Vector2(location.x, location.y);
    }

    private void Charge() {
        float dist = Vector2.Distance(GetVector2Pos(), targetLocation);
        float modChargeSpeed = chargeSpeed;

        if (dist < slowDist) {
            modChargeSpeed = chargeSpeed * (dist + 0.01f) / slowDist;
        }
        
        Vector2 newPos = Vector2.MoveTowards(GetVector2Pos(), targetLocation, Time.deltaTime * modChargeSpeed);
        Vector2 delta = GameManager.instance.ClampInsideRange(newPos) - GetVector2Pos();
        transform.position = new Vector3(newPos.x, transform.position.y, newPos.y);

        if (delta.magnitude < chargeMoveThreshold || Vector2.Distance(newPos, targetLocation) < chargeMoveThreshold) {
            Charging = false;
            SoundManager.instance.PlaySound("Slash2");
            StartCoroutine(DelayCanMove(0.2f));
        }
    }

    IEnumerator DelayCanMove(float t) {
        yield return new WaitForSeconds(t);
        if (!charging) {
            canMove = true;
        }

    }

    IEnumerator InvincibilityFrames() {
        for (int i = 0; i < flashes; i++) {
            sr.enabled = false;
            yield return new WaitForSeconds(flashTime);
            sr.enabled = true;
            yield return new WaitForSeconds(flashTime);
        }
        sr.enabled = true;
        invincible = false;

    }

    public void TakeHit() {
        if (!invincible && !dead) {
            SoundManager.instance.PlaySound("Hurt");
            invincible = true;
            health -= 1;
            if (health <= 0) {
                Die(); return;
            }
            StartCoroutine(InvincibilityFrames());
        }
    }

    void Die() {
        dead = true;
        disableControl = true;
        invincible = true;
        animator.SetTrigger("Dead");
        StartCoroutine(GameOver());
    }

    IEnumerator GameOver() {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(2);
    }

    private void OnTriggerEnter(Collider other) {
        Coin c = other.GetComponent<Coin>();
        if (c != null) {
            c.Collect();
        }
    }
}
