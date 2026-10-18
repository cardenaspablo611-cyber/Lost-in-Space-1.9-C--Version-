using System.Collections; // <-- Add this line
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


    private SpaceshipScript spaceship;
    private int spaceshipLives;

    // -- FPS -- // 
    private int fps;
    private string FPS = "";
    private bool printFPS;
    private UserInterfaceScript userInterface;

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
    private float scoreCooldownTime = 1f;
    private OmegaCooldown scoreCooldown;
    private int scoreMultiplier = 1;

    private MoneyAdministratorScript moneyAdmin;

    public int Score { 
        get { 
            var (score, _, _, _, _) = moneyAdmin.GetTemporalInfo();
            return score;
        }
    }

    public int Money { 
        get { 
            var (_, bills, coins, _, _) = moneyAdmin.GetTemporalInfo();
            return bills * 100 + coins;
        }
    }
    
    public int Bills { 
        get { 
            var (_, bills, _, _, _) = moneyAdmin.GetTemporalInfo();
            return bills;
        }
    }
    
    public int Coins { 
        get { 
            var (_, _, coins, _, _) = moneyAdmin.GetTemporalInfo();
            return coins;
        }
    }

    private void Awake() {
        moneyAdmin = FindFirstObjectByType<MoneyAdministratorScript>();
        if (moneyAdmin == null)
        {
            Debug.LogError("MoneyAdministratorScript not found in the scene!");
        }

        priceBill = F.GetScriptComponentGameObject<MoneyScript>(billPrefab).Price;

        billText = billPanell.GetComponentInChildren<TextMesh>();
        coinText = coinPanell.GetComponentInChildren<TextMesh>();
        fpsText = fpsPanell.GetComponentInChildren<TextMesh>();
        pauseText = pausePanell.GetComponentInChildren<TextMesh>();
        scoreText = scorePanell.GetComponentInChildren<TextMesh>();

        pauseText.text = "";

        userInterface = F.GetScriptComponentName<UserInterfaceScript>("User Interface Object");

        scoreCooldown = new OmegaCooldown(scoreCooldownTime);
        StartCoroutine(FPSUpdaterCoroutine());
    }

    private void Update() {
        afterpauseCounterBool();
        checksScore();
        calcualteLives();
        updateTimeTransition();
        printInPanells();
        
        if (userInterface.F3) {
            printFPS = !printFPS;
        }
    }

    private void checksScore() {

        if (scoreCooldown == null) {
            
            scoreCooldown = new OmegaCooldown(scoreCooldownTime);
            return;
        }

        if (scoreCooldown.coolDownOver()) {
            var (currentScore, _, _, _, _) = moneyAdmin.GetTemporalInfo();
            moneyAdmin.AddGameStats(1, 0, 0); // Add 1 to score
        }
    }

    private void calcualteLives() {

        Transform parentTransform = GameObject.Find($"{spaceshipInUse.tagOfUsedSpaceship}(Clone)").transform;
        Transform child = parentTransform.GetChild(0);


        spaceshipLives = child.GetComponent<SpaceshipScript>().Lives; 
    }

    private void printInPanells() {
        var (score, bills, coins, _, _) = moneyAdmin.GetTemporalInfo();
        
        billText.text = bills.ToString();
        coinText.text = coins.ToString();
        scoreText.text = "Score: " + score.ToString();
        fpsText.text = FPS.ToString();

        foreach (GameObject live in livesPanell) {
            live.gameObject.SetActive(false);
        }

        for (int i = 0; i <= spaceshipLives - 1; i++) {
            livesPanell[i].gameObject.SetActive(true);
        }
    }

    public void addMoney(int price) {
        var (_, currentBills, currentCoins, _, _) = moneyAdmin.GetTemporalInfo();
        
        int newCoins = currentCoins + price;
        
        if (newCoins < 0) {
            newCoins = 0;
        }
        
        int additionalBills = newCoins / 100;
        int remainingCoins = newCoins % 100;
        
        int newBills = currentBills + additionalBills;
        
        if (newBills < 0) {
            newBills = 0;
        }
        
        int billsToAdd = newBills - currentBills;
        int coinsToAdd = remainingCoins - currentCoins;
        
        if (billsToAdd > 0) {
            moneyAdmin.AddBills(billsToAdd);
        }
        if (coinsToAdd > 0) {
            moneyAdmin.AddCoins(coinsToAdd);
        }
        
        MoneyLogger.Log($"[PanellAdminScript] addMoney called. Added: {price}, New total: {newBills} bills, {remainingCoins} coins");
    }

    public void addScore(int amount) {
        int finalAmount = amount;
        
        if (amount > 0) {
            finalAmount = amount * scoreMultiplier;
        }
        
        var (currentScore, _, _, _, _) = moneyAdmin.GetTemporalInfo();
        int newScore = currentScore + finalAmount;
        
        if (newScore < 0) {
            newScore = 0;
        }
        
        int scoreDifference = newScore - currentScore;
        moneyAdmin.AddGameStats(scoreDifference, 0, 0);
    }

    public void SetScoreMultiplier(int multiplier) {
        scoreMultiplier = multiplier;
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
        }
    }

    private IEnumerator FPSUpdaterCoroutine() {
        while (true) {
            if (printFPS) {
                float fpsValue = 1.0f / Time.unscaledDeltaTime;
                fps = Mathf.RoundToInt(fpsValue);
                FPS = "FPS: " + fps.ToString();
            } else {
                FPS = "";
            }
            yield return new WaitForSeconds(0.25f);
        }
    }
}
