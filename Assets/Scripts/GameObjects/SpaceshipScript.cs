using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using F = Functions;
using System.Collections;
using System.Collections.Generic;

public class SpaceshipScript : MonoBehaviour {

    // Global death flag to prevent all game systems from running
    public static bool IsGameOver { get; private set; } = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ResetGameOverFlag() {
        // Reset flag at app start
        IsGameOver = false;
        // Also register scene load callback to reset on every scene change
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        // Reset IsGameOver whenever any scene loads (ensures retry/new game works)
        IsGameOver = false;
    }

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
    
    // death //
    private bool deadth;
    
    // Death zoom target: fraction of original orthographic size (e.g. 0.75 = 75% = less zoom)
    // Tune this value to adjust death zoom amount. Smaller = more zoom. Range: 0.5-0.8 recommended.
    [SerializeField] [Range(0.4f, 1.0f)] private float deathZoomTarget = 0.75f;

    private PanellAdminScript panell;
    private int amountPerDeadth = -100;
    private AudioManager audioManager;

    private void Awake() {
        // Safety: Ensure IsGameOver is false when spaceship instance is created
        // (Should already be reset by OnSceneLoaded, but this is extra protection)
        IsGameOver = false;
        
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
        if (!inmunity && !IsGameOver) {
            
            audioManager.PlayEffect("Spaceship Getting Hit");

            if (!shield) {

                panell.addScore(amountPerDeadth);

                admin.instantiateTakeDamageObject();
                
                if (lives == 1) {

                    if (deadth) return;
                    
                    Debug.Log("[DeathSeq] Ship dying - setting flags and disabling visuals");
                    
                    // Set flags immediately to stop all game systems
                    deadth = true;
                    IsGameOver = true;
                    lives = 0;
                    
                    // Disable collider to prevent further hits
                    Collider2D shipCollider = GetComponent<Collider2D>();
                    if (shipCollider != null) {
                        shipCollider.enabled = false;
                    }
                    
                    // Disable all power-up visuals/effects
                    shield = false;
                    bullet = false;
                    bulletExtra = false;
                    upSideDown = false;
                    bulletRush = false;
                    inmunity = false;
                    
                    // Hide shield visual
                    if (shieldChildObject != null) {
                        shieldChildObject.SetActive(false);
                    }
                    
                    // Hide power-up indicator sprite (bullet/upSideDown/bulletRush visuals)
                    extrasBehaviorSpacehip extrasVisual = GetComponent<extrasBehaviorSpacehip>();
                    if (extrasVisual != null) {
                        SpriteRenderer extrasSpriteRenderer = extrasVisual.GetComponent<SpriteRenderer>();
                        if (extrasSpriteRenderer != null) {
                            extrasSpriteRenderer.sprite = null;
                        }
                    }
                    
                    // NOTE: Don't call StopAllCoroutines here as it would stop DamageScreenShake
                    // which was just started above. Power-up coroutines will be stopped when
                    // scene changes anyway.
                    
                    Debug.Log("[DeathSeq] Power-ups disabled and visuals hidden");

                    // Hide ship sprites
                    rightLeft.sprite = null;
                    upDown.sprite = null;
                    GetComponent<SpriteRenderer>().sprite = null;

                    // Run death sequence on a persistent GameObject (ControlButtonFunctions)
                    GameObject controlButtonFunctions = GameObject.Find("ControlButtonFunctions");
                    if (controlButtonFunctions != null) {
                    MonoBehaviour persistentScript = controlButtonFunctions.GetComponent<ButtonController>();
                    if (persistentScript != null) {
                        persistentScript.StartCoroutine(SlowMotionDeathSequence(transform.position, explosionPrefab, deathZoomTarget));
                    } else {
                            Debug.LogError("[DeathSeq] ButtonController not found!");
                        }
                    } else {
                        Debug.LogError("[DeathSeq] ControlButtonFunctions GameObject not found!");
                    }

                } else if (!inmunity) {
                    
                    Debug.Log("[DmgShake] Non-lethal hit - triggering damage shake");
                    // Screen shake on damage (non-lethal hits only)
                    StartCoroutine(DamageScreenShake());
                    
                    lives -= 1;
                
                    position = startingPosition;

                    instantiateinmunity();
                }
            } else {
                // Shield absorbed hit
                Debug.Log("[DmgShake] Shield absorbed hit - triggering damage shake");
                // Screen shake even when shield absorbs
                StartCoroutine(DamageScreenShake());

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
        Debug.Log("[DmgShake] START - Damage shake triggered on non-lethal hit");
        
        Camera mainCamera = Camera.main;
        if (mainCamera == null) {
            Debug.LogWarning("[DmgShake] Camera.main is null!");
            yield break;
        }
        
        Camera frameCamera = GameObject.Find("Frame Camera")?.GetComponent<Camera>();
        
        Vector3 originalMainPosition = mainCamera.transform.position;
        Vector3 originalFramePosition = frameCamera != null ? frameCamera.transform.position : Vector3.zero;
        
        // Fixed intensity: 1.2-1.5 world units (clearly visible but smaller than death shake)
        float intensity = 1.35f;
        float duration = 0.3f; // Slightly longer than before for visibility
        float elapsed = 0f;
        
        Debug.Log($"[DmgShake] intensity={intensity} units, duration={duration}s, origMainPos={originalMainPosition}, origFramePos={originalFramePosition}");
        
        while (elapsed < duration) {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            float shakeAmount = intensity * (1f - t); // Decay over time
            
            Vector3 shake = new Vector3(
                Random.Range(-shakeAmount, shakeAmount),
                Random.Range(-shakeAmount, shakeAmount),
                0f
            );
            
            mainCamera.transform.position = originalMainPosition + shake;
            if (frameCamera != null) {
                frameCamera.transform.position = originalFramePosition + shake;
            }
            yield return null;
        }
        
        mainCamera.transform.position = originalMainPosition;
        if (frameCamera != null) {
            frameCamera.transform.position = originalFramePosition;
        }
        Debug.Log("[DmgShake] END - Shake complete, cameras restored");
    }

    public static IEnumerator SlowMotionDeathSequence(Vector3 shipPosition, GameObject explosionPrefab, float zoomTarget) {
        // This coroutine runs on a persistent GameObject (ControlButtonFunctions)
        // so it won't stop if the ship is disabled
        
        Debug.Log("[DeathSeq] START - Death sequence beginning");
        
        // Instantiate green explosion at ship position
        GameObject explosion = Object.Instantiate(explosionPrefab) as GameObject;
        explosion.transform.position = shipPosition;
        
        ExplosionScript expScript = explosion.GetComponent<ExplosionScript>();
        if (expScript != null) {
            expScript.getComponents(0, 0.15f); // 35% slower
            expScript.setDeathAnimation();
            expScript.UseUnscaledTime(); // Animates during slowdown
        }
        
        float slowdownDuration = 4f;
        float targetTimeScale = 0f;
        float freezeHoldDuration = 0.4f;

        float originalFixedDeltaTime = Time.fixedDeltaTime;
        // Start from CURRENT speed (e.g. 200% = 2.0) and ramp gradually to 0
        // Do NOT clamp - Pablo wants to see 200->150->100->50->20->0
        float startTimeScale = Time.timeScale;
        Debug.Log($"[DeathSeq] Starting slowdown from timeScale={startTimeScale} to 0 over {slowdownDuration}s");
        
        // Get both cameras
        Camera mainCamera = Camera.main;
        if (mainCamera == null) {
            Debug.LogError("[DeathSeq] Camera.main is null!");
            yield break;
        }
        
        Camera frameCamera = GameObject.Find("Frame Camera")?.GetComponent<Camera>();
        if (frameCamera == null) {
            Debug.LogWarning("[DeathSeq] Frame Camera not found - shake will only apply to main camera");
        }
        
        Vector3 originalMainCameraPosition = mainCamera.transform.position;
        Vector3 originalFrameCameraPosition = frameCamera != null ? frameCamera.transform.position : Vector3.zero;
        float originalOrthographicSize = mainCamera.orthographicSize;
        
        // Death shake: Fixed 2.5 world units (as in 8108446 - clearly visible)
        float deathShakeIntensity = 2.5f;
        float shakeDecayPower = 2f;
        
        Debug.Log($"[DeathSeq] Camera size={originalOrthographicSize}, death shake intensity={deathShakeIntensity} units");
        
        // Zoom via camera orthographicSize and move toward ship death position
        float targetOrthographicSize = originalOrthographicSize * zoomTarget;
        // Calculate target camera position (move toward ship, with same Z)
        Vector3 targetCameraPosition = new Vector3(shipPosition.x, shipPosition.y, originalMainCameraPosition.z);
        
        Debug.Log($"[DeathSeq] Will zoom from ortho={originalOrthographicSize} to {targetOrthographicSize} (factor={zoomTarget}), moving camera toward ship at {shipPosition}");
        
        float elapsedTime = 0f;
        float lastLogTime = 0f;

        // Tell PanellAdminScript to stop its timeScale transitions
        PanellAdminScript panellAdmin = GameObject.Find("Panell Object")?.GetComponent<PanellAdminScript>();
        if (panellAdmin != null) {
            panellAdmin.StopTimeScaleTransitions();
            Debug.Log("[DeathSeq] Stopped PanellAdmin timeScale transitions");
        }

        // Slow down with camera shake and zoom (orthographicSize)
        // Use EASE-OUT curve for gradual visible slowdown
        while (elapsedTime < slowdownDuration) {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / slowdownDuration;
            
            // Ease-out cubic: fast start, slow end - visibly gradual
            float easedT = 1f - Mathf.Pow(1f - t, 3f);
            
            // Update time scale - UNCLAMPED start, gradual to 0
            // DON'T modify fixedDeltaTime - let FixedUpdate naturally run fewer times per second
            Time.timeScale = Mathf.Lerp(startTimeScale, targetTimeScale, easedT);
            
            // Log timeScale every frame for verification
            Debug.Log($"[DeathSeq] Frame t={t:F3} easedT={easedT:F3} TIME.TIMESCALE={Time.timeScale:F3}");
            
            // Zoom gameplay camera via orthographicSize and move toward ship death position
            mainCamera.orthographicSize = Mathf.Lerp(originalOrthographicSize, targetOrthographicSize, easedT);
            Vector3 baseCameraPosition = Vector3.Lerp(originalMainCameraPosition, targetCameraPosition, easedT);
            
            // Clamp camera position so zoomed view stays inside original bounds
            float currentHalfHeight = mainCamera.orthographicSize;
            float currentHalfWidth = currentHalfHeight * mainCamera.aspect;
            float originalHalfHeight = originalOrthographicSize;
            float originalHalfWidth = originalHalfHeight * mainCamera.aspect;
            
            // Calculate max camera offset (how far camera can move without showing outside original bounds)
            float maxOffsetX = originalHalfWidth - currentHalfWidth;
            float maxOffsetY = originalHalfHeight - currentHalfHeight;
            
            // Clamp camera position
            Vector3 clampedPosition = baseCameraPosition;
            clampedPosition.x = Mathf.Clamp(baseCameraPosition.x, 
                originalMainCameraPosition.x - maxOffsetX, 
                originalMainCameraPosition.x + maxOffsetX);
            clampedPosition.y = Mathf.Clamp(baseCameraPosition.y,
                originalMainCameraPosition.y - maxOffsetY,
                originalMainCameraPosition.y + maxOffsetY);
            
            // Screen shake (both cameras) - decays over time
            float shakeAmount = deathShakeIntensity * Mathf.Pow(1f - t, shakeDecayPower);
            Vector3 shake = new Vector3(
                Random.Range(-shakeAmount, shakeAmount),
                Random.Range(-shakeAmount, shakeAmount),
                0f
            );
            
            mainCamera.transform.position = clampedPosition + shake;
            if (frameCamera != null) {
                frameCamera.transform.position = originalFrameCameraPosition + shake;
            }
            
            // Log every 0.5 seconds
            if (elapsedTime - lastLogTime >= 0.5f) {
                Debug.Log($"[DeathSeq] t={t:F2} timeScale={Time.timeScale:F2} ortho={mainCamera.orthographicSize:F1} shake={shakeAmount:F1}");
                lastLogTime = elapsedTime;
            }
            
            yield return null;
        }

        // Final timeScale = 0 (full freeze)
        Time.timeScale = targetTimeScale;
        
        // Stop shake
        mainCamera.transform.position = originalMainCameraPosition;
        if (frameCamera != null) {
            frameCamera.transform.position = originalFrameCameraPosition;
        }
        
        Debug.Log($"[DeathSeq] Slowdown complete, entering freeze hold at timeScale 0, staying zoomed");

        // Create black fade overlay (on UI layer, rendered by Frame Camera)
        GameObject fadeOverlay = new GameObject("DeathFadeOverlay");
        fadeOverlay.layer = 5; // UI layer
        Canvas canvas = fadeOverlay.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = frameCamera != null ? frameCamera : mainCamera;
        canvas.sortingOrder = 1000; // On top of everything
        
        UnityEngine.UI.CanvasScaler scaler = fadeOverlay.AddComponent<UnityEngine.UI.CanvasScaler>();
        scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
        
        GameObject blackPanel = new GameObject("BlackPanel");
        blackPanel.transform.SetParent(fadeOverlay.transform, false);
        blackPanel.layer = 5;
        
        UnityEngine.UI.Image image = blackPanel.AddComponent<UnityEngine.UI.Image>();
        image.color = new Color(0, 0, 0, 0); // Start transparent
        
        RectTransform rect = blackPanel.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        rect.anchoredPosition = Vector2.zero;
        
        Debug.Log("[DeathSeq] Created black fade overlay, starting fade in");
        
        // Fade to black over freeze hold duration (0.4s)
        float fadeDuration = 0.4f;
        float fadeElapsed = 0f;
        
        while (fadeElapsed < freezeHoldDuration) {
            fadeElapsed += Time.unscaledDeltaTime;
            float fadeT = Mathf.Clamp01(fadeElapsed / fadeDuration);
            if (image != null) {
                image.color = new Color(0, 0, 0, fadeT);
            }
            yield return null;
        }
        
        // Ensure fully black
        if (image != null) {
            image.color = Color.black;
        }
        
        Debug.Log("[DeathSeq] Fade to black complete, starting door close transition");
        
        // Start door closing animation (doors use unscaled time)
        ButtonController buttonController = GameObject.Find("ControlButtonFunctions")?.GetComponent<ButtonController>();
        if (buttonController != null) {
            // LoadScene starts door animation and will load scene at 1.2s
            buttonController.StartCoroutine(buttonController.loadANewScene("GameOverMenu"));
        } else {
            Debug.LogError("[DeathSeq] ButtonController not found, cannot start door transition!");
            yield break;
        }
        
        // Wait for doors to fully close (1.0s animation) + small buffer
        yield return new WaitForSecondsRealtime(1.1f);
        
        Debug.Log("[DeathSeq] Doors closed, restoring camera and timeScale behind them");
        
        // Restore camera zoom and timeScale behind closed doors (not visible anymore)
        // Do this at 1.1s, before scene loads at 1.2s
        mainCamera.orthographicSize = originalOrthographicSize;
        mainCamera.transform.position = originalMainCameraPosition;
        if (frameCamera != null) {
            frameCamera.transform.position = originalFrameCameraPosition;
        }
        Time.timeScale = 1f;
        Time.fixedDeltaTime = originalFixedDeltaTime;
        
        Debug.Log("[DeathSeq] END - Restored behind doors, scene switch imminent");
        
        // Scene will load in 0.1s (loadANewScene waits 1.2s total)
    }    
}
