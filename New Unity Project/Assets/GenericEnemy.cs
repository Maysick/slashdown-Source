using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyMovePattern {
    Stationary,
    Sporadic
}

public enum EnemyShootPattern {
    Player,
    EightBurst,
    Shotgun
}

public class GenericEnemy : Enemy {

    public EnemyMovePattern movePattern;
    public EnemyShootPattern shootPattern;

    public bool intervalMove = true;

    public float moveDelay = 0.5f;
    public float speed = 2f;
    public float bulletSpeed = 4f;
    public float bulletDelay;
    private bool bulletCycleRunning;
    private bool moveCycleRunning;

    public Vector2 targetLocation;

    private float t;
    public float bobSpeed = 1f;
    public float bobHeight = 0.1f;
    public float spriteSwitch;
    private float initialHeight;

    private int spriteI = 0;
    public Sprite[] sprites;

    public GameObject graphicsParent;
    public SpriteRenderer graphics;

    public SpriteRenderer shadow;
    

    public override void Start() {
        base.Start();
        initialHeight = graphicsParent.transform.localPosition.y;

    }

    public override void Spawn() {
        base.Spawn();
        intervalMove = true;
        graphics.enabled = true;
        shadow.enabled = true;
        

        targetLocation = GetVector2Pos();
        if (intervalMove) {
            moveCycleRunning = true;
            StartCoroutine(MoveCycle());
        }

        bulletCycleRunning = true;
        if (bulletDelay != 0) StartCoroutine(ShootCycle());
        StartCoroutine(SpriteAnim());
    }

    public override void DisableSelf() {
        intervalMove = false;
        base.DisableSelf();
        shadow.enabled = false;
        graphics.enabled = false;
    }

    void SwitchSprites() {
        spriteI++;
        if (spriteI >= sprites.Length) spriteI -= sprites.Length;
        graphics.sprite = sprites[spriteI];
    }

    IEnumerator SpriteAnim() {
        while (moveCycleRunning) {
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

        t += Time.deltaTime * bobSpeed;
        if (t >= Mathf.PI * 2) t = -Mathf.PI * 2;
        graphicsParent.transform.localPosition = new Vector3(graphicsParent.transform.localPosition.x, initialHeight + (Mathf.Sin(t) * bobHeight), graphicsParent.transform.localPosition.z);


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

    public void ShootBullet(float _bulletSpeed, float angle) {
        GameObject prefab = PrefabManager.instance.GetPrefabByName("Bullet");
        GameObject newBulletObject = Instantiate(prefab, transform.position, prefab.transform.rotation) as GameObject;
        Bullet newBullet = newBulletObject.GetComponent<Bullet>();
        newBullet.angle = angle;
        newBullet.speed = _bulletSpeed;
        newBullet.Fire();
    }

    public void ShootInterval() {
        if (shootPattern == EnemyShootPattern.Player) {
            float rot = GameManager.instance.AngleBetweenVectors(GameManager.instance.player.transform.position, transform.position);
            ShootBullet(bulletSpeed, rot);
        } else if (shootPattern == EnemyShootPattern.EightBurst) {
            for (int i = 0; i < 8; i++) {
                float rot = i * (360 / 8f);
                ShootBullet(bulletSpeed, rot);
            }
        } else if (shootPattern == EnemyShootPattern.Shotgun) {
            for (int i = 0; i < 5; i++) {
                float rot = GameManager.instance.AngleBetweenVectors(GameManager.instance.player.transform.position, transform.position);
                rot += Random.Range(-15, 15);
                ShootBullet(bulletSpeed * Random.Range(0.75f, 1.25f), rot);
            }
        }
    }
}
