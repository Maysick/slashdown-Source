using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BeamEnemy : Enemy {

    public EnemyMovePattern movePattern;
    public EnemyShootPattern shootPattern;

    public bool intervalMove = true;

    public float moveDelay = 0.5f;
    public float speed = 2f;
    public float bulletDelay;
    private bool bulletCycleRunning;
    private bool moveCycleRunning;

    public Vector2 targetLocation;

    public float spriteSwitch;

    private int spriteI = 0;
    public Sprite[] sprites;

    public GameObject graphicsParent;
    public SpriteRenderer graphics;

    private GameObject beam;

    public SpriteRenderer shadow;

    public override void Start() {
        base.Start();


    }

    public override void Die() {
        if (beam != null) Destroy(beam);

        GameManager.instance.BurstCoinsFromPosition(transform.position, level);
        Instantiate(PrefabManager.instance.GetPrefabByName("Cloud"), transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        SoundManager.instance.PlaySound("EnemyDead");
        WaveManager.instance.killCount++;
        Destroy(gameObject);
    }

    public override void Spawn() {
        base.Spawn();
        graphics.enabled = true;
        shadow.enabled = true;
        targetLocation = GetVector2Pos();
        if (intervalMove) {
            moveCycleRunning = true;
            StartCoroutine(MoveCycle());
        }

        bulletCycleRunning = true;
        StartCoroutine(ShootCycle());
        StartCoroutine(SpriteAnim());
    }

    public override void DisableSelf() {
        base.DisableSelf();
        graphics.enabled = false;
        shadow.enabled = false;
    }

    void SwitchSprites() {
        spriteI++;
        if (spriteI >= sprites.Length) spriteI -= sprites.Length;
        graphics.sprite = sprites[spriteI];
    }

    IEnumerator SpriteAnim() {
        while (true) {
            yield return new WaitForSeconds(spriteSwitch);
            SwitchSprites();
        }
    }

    IEnumerator MoveCycle() {
        yield return new WaitForSeconds(Random.value * moveDelay);
        while (moveCycleRunning) {
            MoveInterval();
            yield return new WaitForSeconds(moveDelay);
        }
    }

    IEnumerator ShootCycle() {
        yield return new WaitForSeconds(Random.value * bulletDelay);
        while (bulletCycleRunning) {
            ShootInterval();
            yield return new WaitForSeconds(bulletDelay);
        }
    }

    public void Update() {


        if (intervalMove) {
            Vector2 newPos = Vector2.MoveTowards(GetVector2Pos(), targetLocation, speed * Time.deltaTime);
            transform.position = new Vector3(newPos.x, transform.position.y, newPos.y);
        }   
    }

    public void MoveInterval() {
        if (movePattern == EnemyMovePattern.Sporadic) {
            Vector2 randomLocation = GetVector2Pos() + GameManager.instance.RandomPointInRing(1, 2);
            randomLocation = GameManager.instance.ClampInsideRange(randomLocation);
            targetLocation = randomLocation;
        }
    }

    public void ShootBeam() {
        beam = Instantiate(PrefabManager.instance.GetPrefabByName("Beam"), transform.position, PrefabManager.instance.GetPrefabByName("Beam").transform.rotation) as GameObject;
    }

    public void ShootInterval() {
        if (shootPattern == EnemyShootPattern.Player) {
            float rot = GameManager.instance.AngleBetweenVectors(GameManager.instance.player.transform.position, transform.position);
            ShootBeam();
        }
    }
}
