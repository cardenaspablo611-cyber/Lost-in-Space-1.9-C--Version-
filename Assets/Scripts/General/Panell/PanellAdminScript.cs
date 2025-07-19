using UnityEngine;
using F = Functions;

public class PanellAdminScript : MonoBehaviour {

    [Header("-- All Panells --")]
    [SerializeField] private GameObject billPanell;
    [SerializeField] private GameObject coinPanell;
    [SerializeField] private GameObject[] livesPanell;
    [SerializeField] private GameObject fpsPanell;
    [SerializeField] private GameObject pausePanell;
    [SerializeField] private GameObject scorePanell;

    private TextMesh billText;
    private TextMesh coinText;
    private TextMesh liveText;
    private TextMesh fpsText;
    private TextMesh pauseText;
    private TextMesh scoreText;

    [Header("-- Money --")]  // -- Money & Lives -- // 
    [SerializeField] private GameObject billPrefab;
    private int priceBill;
    private int money;
    public int Money { get { return money;}}
    private int coins;
    public int Coins { get { return coins;}}
    private int bills;
    public int Bills { get { return bills;}}

    private SpaceshipScript spaceship;
    private int spaceshipLives;

    // -- FPS -- // 
    private float deltaTime = 0.0f;
    private int fps;
    private string FPS = "";
    private bool printFPS;

    // -- After Pause Counter -- //
    private float changingNumberCooldownTime = 0.5f;
    private Cooldown changingNumberCooldown;
    public static bool startAfterCount;
    private string textAfterPause;
    [SerializeField] private int originalPauseController;
    private float pauseController;
    private float originalTimeScale;

    // -- Smooth Time Transition -- //
    private float currentTimeScale;
    private bool isTransitioningTime;
    private float timeScaleTransitionSpeed = 8f;

    [SerializeField] private SpaceshipInUseScript spaceshipInUse;

    // -- Score -- //
    private int score;
    public int Score { get { return score;}}
    private float scoreCooldownTime = 1f;
    private OmegaCooldown scoreCooldown;

    private void Awake() {

        priceBill = F.GetScriptComponentGameObject<MoneyScript>(billPrefab).Price;

        billText = billPanell.GetComponentInChildren<TextMesh>();
        coinText = coinPanell.GetComponentInChildren<TextMesh>();
        fpsText = fpsPanell.GetComponentInChildren<TextMesh>();
        pauseText = pausePanell.GetComponentInChildren<TextMesh>();
        scoreText = scorePanell.GetComponentInChildren<TextMesh>();

        pauseText.text = "";

        scoreCooldown = new OmegaCooldown(scoreCooldownTime);
    }

    private void Update() {
        afterpauseCounterBool();
        checksScore();
        calcualteLives();
        calculateMoney();
        calculateFrameRate();
        updateTimeTransition();
        printInPanells();
    }

    private void checksScore() {

        if (scoreCooldown == null) {
            
            scoreCooldown = new OmegaCooldown(scoreCooldownTime);
            return;
        }

        if (scoreCooldown.coolDownOver()) {
            score += 1;
        }
    }

    private void calcualteLives() {

        Transform parentTransform = GameObject.Find($"{spaceshipInUse.tagOfUsedSpaceship}(Clone)").transform;
        Transform child = parentTransform.GetChild(0);


        spaceshipLives = child.GetComponent<SpaceshipScript>().Lives; 
    }

    private void calculateMoney() {

        bills = (int)(money / priceBill);
        coins = money - bills * priceBill;
    }

    private void calculateFrameRate() {

        UserInterfaceScript userInterface = F.GetScriptComponentName<UserInterfaceScript>("User Interface Object");

        if (userInterface.F3) {

            printFPS = !printFPS;
        }

        if (printFPS) {

            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            fps = Mathf.RoundToInt(1.0f / deltaTime);

            FPS = "FPS: " + fps.ToString();
        } else {

            FPS = "";
        }   
    }

    private void printInPanells() {

        billText.text = bills.ToString();
        coinText.text = coins.ToString();
        fpsText.text = FPS;
        scoreText.text = "Score: " + score.ToString();

        foreach (GameObject live in livesPanell) {

            live.gameObject.SetActive(false);
        }

        for (int i = 0; i <= spaceshipLives - 1; i++) {

            livesPanell[i].gameObject.SetActive(true);
        }
    }

    public void addMoney(int price) {
        
        money += price;
        MoneyLogger.Log($"[PanellAdminScript] addMoney called. Added: {price}, New total money: {money}");
    }

    public void addScore(int amount) {

        score += amount;

        if (score < 0) {
            score = 0;
        }
    }

    private void afterpauseCounterBool() {

        if (startAfterCount) {

            if (changingNumberCooldown.coolDownOver()) {

                printAfterPause();
            }
        }
    }

    private void updateTimeTransition() {
        if (isTransitioningTime) {
            currentTimeScale = Mathf.Lerp(currentTimeScale, originalTimeScale, Time.unscaledDeltaTime * timeScaleTransitionSpeed);
            Time.timeScale = currentTimeScale;

            // Check if we're close enough to the target time scale
            if (Mathf.Abs(currentTimeScale - originalTimeScale) < 0.01f) {
                Time.timeScale = originalTimeScale;
                isTransitioningTime = false;
            }
        }
    }

    public void afterPausing(float givenTime) {
        originalTimeScale = givenTime;
        currentTimeScale = 0f;
        isTransitioningTime = false; // Don't start transition yet

        pauseController = (float)originalPauseController;
        changingNumberCooldown = new Cooldown(changingNumberCooldownTime);
        
        startAfterCount = true;

        printAfterPause();
    }

    private void printAfterPause() {
        textAfterPause = pauseController.ToString();
        pauseText.text = textAfterPause;

        if (pauseController > 1) {
            pauseController -= 1;
        } 
        else if (pauseController == 1) {
            pauseController -= 1;
            isTransitioningTime = true; // Start transition when we hit 1
        }
        else if (pauseController == 0) {
            startAfterCount = false;
            pauseText.text = "";
            // Time scale transition continues in updateTimeTransition until complete
        }
    }
}
