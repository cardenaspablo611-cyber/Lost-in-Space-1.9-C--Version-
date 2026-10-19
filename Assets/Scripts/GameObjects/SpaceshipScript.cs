using UnityEngine;
using F = Functions;
using System.Collections;
using System.Collections.Generic;

public class SpaceshipScript : MonoBehaviour {

    [SerializeField] private SpecificSpaceshipInformation spaceshipInformation;

    private Vector2 position;
    private Vector2 startingPosition;

    private float damageProbability = 1f;
    public float DamageProbability { get { return damageProbability; } }
    private float speed = 10;
    private float currentSpeed;
    private float lateralMovement;
    private float linearMovement;
    private Vector2 size;

    [SerializeField] private Sprite[] spritesUpDown;
    [SerializeField] private Sprite[] spritesRightLeft;
    [SerializeField] private GameObject childUpDown;
    [SerializeField] private GameObject childRightLeft;
    private SpriteRenderer upDown;
    private SpriteRenderer rightLeft;
    private int stateUpDown;
    private int stateRightLeft;

    private bool offLimitPositivY;
    private bool offLimitNegativY;
    private bool offLimitPositivX;
    private bool offLimitNegativX;

    [HideInInspector] public bool OffLimitPositivY { get { return offLimitPositivY;}}
    [HideInInspector] public bool OffLimitNegativY { get { return offLimitNegativY;}}
    [HideInInspector] public bool OffLimitPositivX { get { return offLimitPositivX;}}
    [HideInInspector] public bool OffLimitNegativX { get { return offLimitNegativX;}}

    private UserInterfaceScript userInterface;
    [SerializeField] private GameObject buttonControllerParent;
    private ButtonController buttonControllerObject;
    private SpaceParent space;
    public SpaceParent Space {get {return space;}}
    private AdminScript admin;
    
    // inmunity //
    [SerializeField] private GameObject inmunityBarPrefab;
    private bool inmunity;

    private float blinkingCooldownTime = 0.25f;
    private Cooldown blinkingCooldown;
    private bool blinkingCooldownBool = true;

    private float totalBlinkingCooldownTime = 3f;
    private Cooldown totalBlinkingCooldown;
    private bool totalBlinkingCooldownBool;

    // Bullet //
    [SerializeField] private GameObject bulletPrefab;
    private float bulletCooldownTime = 0.2f;
    private OmegaCooldown bulletCooldown;
    private bool bulletCooldownDeterminator = true;
    private int bulletStartingPointX = 11;
    private int bulletStartingPointY = 25;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private string nameOfBullet;

    // Extras // 
    private bool shield;
    [HideInInspector] public bool Shield { get { return shield;}}
    [SerializeField] private GameObject shieldChildObject;
    private bool bullet;
    public bool bulletUpgrade {get {return bullet;} }
    private bool bulletExtra;
    private bool upSideDown = false;
    public bool UpSideDown { get { return upSideDown;} }
    private bool bulletRush = false;
    public bool BulletRush { get { return bulletRush;} }
    private float extrasCooldownTime = 10f;
    public float ExtrasCooldown { get {return extrasCooldownTime;} }
    private OmegaCooldown bulletExtraCooldown;
    private bool recentlyGain;
    public bool RecentlyGain { get { return recentlyGain;} }

    // Other //

    [SerializeField] private int maxLives;
    [HideInInspector] public int MaxLives { get { return maxLives;}}
    [Range(1,4)]
    private int lives;
    [SerializeField] private GameObject gainHeartAnimationPrefab;

    [HideInInspector] public int Lives { get { return lives;}}
    [HideInInspector] public bool Deadth { get { return deadth;}}
    [HideInInspector] public bool Immunity { get { return inmunity;}}
    private int livesFactor;
    private bool deadth;

    private PanellAdminScript panell;
    private int amountPerDeadth = -100;
    private AudioManager audioManager;

    private void Awake() {
        upDown = childUpDown.GetComponent<SpriteRenderer>();
        rightLeft = childRightLeft.GetComponent<SpriteRenderer>();

        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        size = collider.size;

        userInterface = F.GetScriptComponentName<UserInterfaceScript>("User Interface Object");
        space = F.GetScriptComponentName<SpaceParent>("Space - Parent");
        admin = F.GetScriptComponentName<AdminScript>("Admin");
        panell = F.GetScriptComponentName<PanellAdminScript>("Panell Object");

        if (spaceshipInformation == null) {
            Debug.LogError($"[SpaceshipScript] spaceshipInformation is NULL on {gameObject.name}! Spaceship will not move.");
            return;
        }

        speed *= spaceshipInformation.speed / 30f;
        damageProbability *= spaceshipInformation.attack * 0.25f - 0.25f;

        Debug.Log($"[SpaceshipScript] {spaceshipInformation.spaceshipTag} initialized: speed = {speed}");
    }

    private void Start() {
        
        lives = maxLives;

        settingCooldowns();
        setStartingPosition();

        //StartCoroutine(ShootWithCooldown());

        MoneyLogger.StartNewGameSession();
        audioManager = FindFirstObjectByType<AudioManager>();

    }

    void FixedUpdate() {

        if (!deadth) {
            checkingShieldUpgrade();
            checkingBulletCooldownUpgrade();
            checkingBlinkingEffect();        
            spriteAnimation();
            checkOfLimit();
            spaceshipMovement();
        }
    }

    // -- Setting the parameters -- //
    
    private void setStartingPosition() {

        startingPosition.x = 0;
        startingPosition.y = F.getGameObjectSizeWithScale("Space - Parent").y / 5 * (-1);

        position.x = startingPosition.x;
        position.y = startingPosition.y;

        transform.position = position;
    }

    private void settingCooldowns() {

        blinkingCooldown = new Cooldown(blinkingCooldownTime);
        totalBlinkingCooldown = new Cooldown(totalBlinkingCooldownTime);
        bulletCooldown = new OmegaCooldown(bulletCooldownTime);
    }
    private void spaceshipMovement() {

        lateralMovement = 0f;
        linearMovement = 0f;  

        if (userInterface.Up) {
            linearMovement = 1f;
        }
        if (userInterface.Down) {
            linearMovement = -1f;
        }
        if (userInterface.Right) {
            lateralMovement = 1f;
        } 
        if (userInterface.Left) {
            lateralMovement = -1f;
        } 
    
        calculateLateralMovement();

        position.x += currentSpeed * lateralMovement;
        position.y += currentSpeed * linearMovement;
        transform.position = position;
    }
    private void spriteAnimation() {

        livesFactor = lives * (-3) + maxLives * 3;
        stateUpDown = 0;

        rightLeft.sprite = null;

        if (userInterface.Up) {
            stateUpDown = 1;
        }
        if (userInterface.Down) {
            stateUpDown = 2;
        }
        if (userInterface.Right) {
            if (blinkingCooldownBool) {
                rightLeft.sprite = spritesRightLeft[0];  
            }
        } 
        if (userInterface.Left) {
            if (blinkingCooldownBool) {
                rightLeft.sprite = spritesRightLeft[1];  
            }
        } 
        if (blinkingCooldownBool) { 
            upDown.sprite = spritesUpDown[stateUpDown + livesFactor];
        } else {
            upDown.sprite = null;
            rightLeft.sprite = null;
            shieldChildObject.SetActive(false);
        }

        if (bulletCooldownDeterminator) {
            if (Input.GetKey(KeyCode.Space) && !deadth) {            
                if (bulletCooldown.coolDownOver() && !userInterface.GetComponent<PauseController>().ItsPaused) {
                    createBulletCouple();
                    StartCoroutine(BulletCooldownFuntion());
                }
            } 
        }
    }

    private void calculateLateralMovement() {

        if (userInterface.Down && userInterface.Left || userInterface.Down && userInterface.Right) {
            currentSpeed = (int)F.calculatingHypotenusaSameSides(speed) + 1;
        } else if (userInterface.Up && userInterface.Left || userInterface.Up && userInterface.Right) {
            currentSpeed = (int)F.calculatingHypotenusaSameSides(speed) + 1;
        } else {
            currentSpeed = speed;
        }
    }

    // -- EXTRAS -- //

    public void getExtras(string id) {
        if (id == "shield") {
            shield = true;
        } else if (id == "bullets") {
            recentlyGain = true;
            bullet = true;

            bulletExtraCooldown = new OmegaCooldown(extrasCooldownTime);
        } else if (id == "heart") {

            if (lives != maxLives) {
                lives += 1;

                if (gainHeartAnimationPrefab != null) {
                    GameObject redHeart = Instantiate(gainHeartAnimationPrefab, transform.position, Quaternion.identity);
                    redHeart.transform.SetParent(gameObject.transform);
                } else {
                    Debug.LogWarning("[SpaceshipScript] gainHeartAnimationPrefab is null - heart pickup animation skipped");
                }
            }
        } else if (id == "upSideDown") {
            recentlyGain = true;

            upSideDown = true;
            panell.SetScoreMultiplier(2);

            StartCoroutine(UpSideDownTime());
        } else if (id == "bulletRush") {
            recentlyGain = true;

            bulletRush = true;

            StartCoroutine(ChangingBulletsSpeed());
        }
    }

    private void checkingBlinkingEffect() {

        if (totalBlinkingCooldownBool) {
            if (blinkingCooldown.coolDownOver()) {
                blinkingCooldownBool = !blinkingCooldownBool;
            }
            if (totalBlinkingCooldown.coolDownOver()) {
                blinkingCooldownBool = true;
                totalBlinkingCooldownBool = false;

                inmunity = false;
            }
        }
    }

    private void checkingShieldUpgrade() {

        if (shield) {
            shieldChildObject.SetActive(true);
        } else {
            shieldChildObject.SetActive(false);
        } 
    }

    // -- BULLET -- //

    private void checkingBulletCooldownUpgrade() {
        if (bullet) {
            if (bulletExtraCooldown.coolDownOver()) {

                bullet = false;
            }
        }
    }

    private void createWierdBullet(float x, float y, float extraLateralMovement) {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayEffect(nameOfBullet);
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;

        F.GetScriptComponentGameObject<BulletScript>(bullet).setLateralMovement(extraLateralMovement);

        bullet.transform.position = new Vector2 (transform.position.x + x * 2, transform.position.y + y * 2) ;
    }

    private void createBullet(float x, float y) {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayEffect(nameOfBullet);
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;

        bullet.transform.position = new Vector2 (transform.position.x + x * 2, transform.position.y + y * 2) ;
    }

    private void createBulletCouple() {

        createBullet(bulletStartingPointX,bulletStartingPointY);

        createBullet(-bulletStartingPointX,bulletStartingPointY);

        if (bullet || bulletExtra) {
            createWierdBullet(bulletStartingPointX,bulletStartingPointY,0.5f);
            createWierdBullet(bulletStartingPointX,bulletStartingPointY,0.25f);

            createWierdBullet(-bulletStartingPointX,bulletStartingPointY,-0.5f);
            createWierdBullet(-bulletStartingPointX,bulletStartingPointY,-0.25f);
        }
    }

    // -- OTHER -- //

    private void checkOfLimit() { // -- CHECKING IF [The spaceship is in the corresponding space] -- // 

        offLimitNegativX = false;
        offLimitNegativY = false;
        offLimitPositivX = false;
        offLimitPositivY = false;

        if (transform.position.y - size.y - currentSpeed <= space.End.y) {
            offLimitNegativY = true;
        }
        
        if (transform.position.x + size.x + currentSpeed >= space.End.x) {
            offLimitPositivX = true;
        }

        if (transform.position.x - size.x - currentSpeed <= space.Start.x) {
            offLimitNegativX = true;
        }
        
        if (transform.position.y + size.y + currentSpeed >= space.Start.y) {
            offLimitPositivY = true;
        }
    }

    public void gotHitten() {
        if (!inmunity) {
            
            audioManager.PlayEffect("Spaceship Getting Hit");

            if (!shield) {

                panell.addScore(amountPerDeadth);

                admin.instantiateTakeDamageObject();
                
                // Screen shake on damage
                StartCoroutine(DamageScreenShake());

                if (lives == 1) {

                    if (deadth) return;
                    deadth = true;
                    lives = 0;

                    rightLeft.sprite = null;
                    upDown.sprite = null;
                    GetComponent<SpriteRenderer>().sprite = null;

                    // Instantiate green explosion (the explosionPrefab has green tint)
                    GameObject explosion = Instantiate(explosionPrefab) as GameObject;
                    explosion.transform.position = transform.position;
                    
                    ExplosionScript expScript = explosion.GetComponent<ExplosionScript>();
                    if (expScript != null) {
                        // Make it play ~35% slower: use 1.5x the deathAnimationCooldownTime
                        // AdminScript uses deathAnimationCooldownTime for special deaths
                        // Assuming deathAnimationCooldownTime is ~0.1, use 0.15 for slower playback
                        expScript.getComponents(0, 0.15f);
                        expScript.setDeathAnimation();
                        expScript.UseUnscaledTime(); // So it animates during slowdown
                    }
                    
                    StartCoroutine(SlowMotionDeath());

                } else if (!inmunity) {
                    lives -= 1;
                
                    position = startingPosition;

                    instantiateinmunity();
                }
            } else {

                shield = false;

                instantiateinmunity();
            }
        }
    }

    public void instantiateinmunity() {

        blinkingCooldown = new Cooldown(blinkingCooldownTime);

        totalBlinkingCooldown = new Cooldown(totalBlinkingCooldownTime);
        totalBlinkingCooldownBool = true;

        inmunity = true;

        GameObject inmunityBar = Instantiate(inmunityBarPrefab) as GameObject;
        
        inmunityBar.GetComponent<BarScript>().setTimer(totalBlinkingCooldownTime);
    }

    public void getAHeart() {
        if (lives != maxLives) {
            lives += 1;
        }
    }

    public void resetRecentlyGain() {
        recentlyGain = false;
    }

    private IEnumerator UpSideDownTime() {
        yield return new WaitForSeconds(extrasCooldownTime);

        upSideDown = false;
        panell.SetScoreMultiplier(1);
    }

    private IEnumerator BulletCooldownFuntion() {
        bulletCooldownDeterminator = false;

        yield return new WaitForSeconds(bulletCooldownTime);

        bulletCooldownDeterminator = true;
    }

    private IEnumerator ChangingBulletsSpeed() {

        float originalCooldownTime = bulletCooldownTime; 
        bulletCooldownTime = 0.012f;

        yield return new WaitForSeconds(10f);
        
        bulletCooldownTime = originalCooldownTime;
        bulletRush = false;
    }

    private IEnumerator DamageScreenShake() {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) yield break;
        
        Vector3 originalPosition = mainCamera.transform.position;
        float duration = 0.2f; // Short shake
        float intensity = 0.8f; // Smaller than death shake
        float elapsed = 0f;
        
        while (elapsed < duration) {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            float shakeAmount = intensity * (1f - t); // Decay quickly
            
            Vector3 shake = new Vector3(
                Random.Range(-shakeAmount, shakeAmount),
                Random.Range(-shakeAmount, shakeAmount),
                0f
            );
            
            mainCamera.transform.position = originalPosition + shake;
            yield return null;
        }
        
        mainCamera.transform.position = originalPosition;
    }

    private IEnumerator SlowMotionDeath() {
        float slowdownDuration = 4f;
        float targetTimeScale = 0f;
        float freezeHoldDuration = 0.4f;
        float explosionDuration = 0.7f;

        float originalFixedDeltaTime = Time.fixedDeltaTime;
        float startTimeScale = Time.timeScale;
        
        // Get the main camera for shake
        Camera mainCamera = Camera.main;
        if (mainCamera == null) {
            Debug.LogError("[SpaceshipScript] Camera.main is null!");
            yield break;
        }
        
        Vector3 originalCameraPosition = mainCamera.transform.position;
        
        // Get the Space - Parent GameObject for gameplay-only zoom
        GameObject spaceParent = GameObject.Find("Space - Parent");
        if (spaceParent == null) {
            Debug.LogError("[SpaceshipScript] 'Space - Parent' GameObject not found!");
            yield break;
        }
        
        Vector3 originalSpaceScale = spaceParent.transform.localScale;
        Vector3 originalSpacePosition = spaceParent.transform.position;
        float targetZoomScale = 1.67f; // 1/0.6 = 1.67 to achieve 60% view (zooming in)
        Vector3 shipPosition = transform.position;
        
        // Camera shake parameters
        float shakeIntensity = 2.5f;
        float shakeDecayPower = 2f;
        
        float elapsedTime = 0f;

        // Slow down to complete freeze with camera shake and gameplay zoom
        while (elapsedTime < slowdownDuration) {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / slowdownDuration;
            
            // Smoothstep ease (smooth start and end)
            float easedT = t * t * (3f - 2f * t);
            
            // Update time scale
            Time.timeScale = Mathf.Lerp(startTimeScale, targetTimeScale, easedT);
            Time.fixedDeltaTime = originalFixedDeltaTime * Mathf.Max(Time.timeScale, 0.01f);
            
            // Zoom the gameplay area (Space - Parent) by scaling it up
            float currentScale = Mathf.Lerp(1f, targetZoomScale, easedT);
            spaceParent.transform.localScale = originalSpaceScale * currentScale;
            
            // Adjust Space - Parent position to keep ship centered as it scales
            // When scaling, objects move away from origin, so we need to counteract that
            Vector3 offset = shipPosition - originalSpacePosition;
            Vector3 scaledOffset = offset * (currentScale - 1f);
            spaceParent.transform.position = originalSpacePosition - scaledOffset;
            
            // Camera shake (strongest at start, decaying over time)
            float shakeAmount = shakeIntensity * Mathf.Pow(1f - t, shakeDecayPower);
            Vector3 shake = new Vector3(
                Random.Range(-shakeAmount, shakeAmount),
                Random.Range(-shakeAmount, shakeAmount),
                0f
            );
            
            mainCamera.transform.position = originalCameraPosition + shake;
            
            yield return null;
        }

        Time.timeScale = targetTimeScale;
        Time.fixedDeltaTime = originalFixedDeltaTime * 0.01f;
        
        // Stop shake, keep gameplay zoom
        mainCamera.transform.position = originalCameraPosition;

        // Hold frozen frame
        yield return new WaitForSecondsRealtime(freezeHoldDuration);

        // Explode all enemies on screen
        MeteoriteScript[] allEnemies = FindObjectsOfType<MeteoriteScript>();
        if (allEnemies.Length > 0) {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayEffect("Explosion");
            
            foreach (MeteoriteScript enemy in allEnemies) {
                if (enemy != null && enemy.gameObject != null) {
                    GameObject enemyDeathPrefab = enemy.GetDeathAnimationPrefab();
                    if (enemyDeathPrefab == null) {
                        enemyDeathPrefab = explosionPrefab;
                    }
                    
                    GameObject enemyExplosion = Instantiate(enemyDeathPrefab) as GameObject;
                    enemyExplosion.transform.position = enemy.transform.position;
                    
                    ExplosionScript enemyExpScript = enemyExplosion.GetComponent<ExplosionScript>();
                    if (enemyExpScript != null) {
                        // Use proper cooldown for enemy explosions
                        enemyExpScript.getComponents(0, 0.1f); // Normal explosion speed
                        enemyExpScript.UseUnscaledTime(); // So it animates at timeScale=0
                        enemyExpScript.DisableSound();
                    }
                    
                    Destroy(enemy.gameObject);
                }
            }
            
            yield return new WaitForSecondsRealtime(explosionDuration);
        }

        // Restore everything
        spaceParent.transform.localScale = originalSpaceScale;
        spaceParent.transform.position = originalSpacePosition;
        mainCamera.transform.position = originalCameraPosition;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = originalFixedDeltaTime;

        GameObject.Find("ControlButtonFunctions").GetComponent<ButtonController>().LoadScene("GameOverMenu");
    }    
}
