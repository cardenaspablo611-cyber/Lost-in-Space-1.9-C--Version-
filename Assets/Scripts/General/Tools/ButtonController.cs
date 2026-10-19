using UnityEngine.SceneManagement;
using UnityEngine;
using F = Functions;
using System.Collections;

public class ButtonController : MonoBehaviour {

    public SpaceshipInUseScript use;

    private MoneyAdministratorScript moneyAdmin;
    private AudioManager audioManager;
    
    [SerializeField] private GameObject SpaceshipShopAnimation;
    [SerializeField] private SpecificSpaceshipInformation[] spaceships;

    [SerializeField] private SceneData sceneData;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void RegisterSceneLoadHandler() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Time.timeScale = 1f;
    }

    private void Start()
    {
        moneyAdmin = FindFirstObjectByType<MoneyAdministratorScript>();
        if (moneyAdmin == null)
        {
            Debug.LogError("MoneyAdministratorScript not found in the scene!");
        }
        
        audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene!");
        }
    }

    public void LoadScene(string sceneName) {

        if (SceneManager.GetActiveScene().name == "Game") {
            
            PauseController pauseInterface = F.GetScriptComponentName<PauseController>("User Interface Object");
            Time.timeScale = pauseInterface.OriginalTimeScale;
        }

        if (sceneName != SceneManager.GetActiveScene().name) {
            if (SceneManager.GetActiveScene().name == "OptionsMenu") {
                StartCoroutine(loadANewScene(sceneData.LastScene));
            } else {
                StartCoroutine(loadANewScene(sceneName));
            }
        }

        sceneData.LastScene = SceneManager.GetActiveScene().name;
    }

    public IEnumerator loadANewScene(string sceneName) {

        GameObject firstRectangle = GameObject.Find("Rectangle 1");
        GameObject secondRectangle = GameObject.Find("Rectangle 2");

        GameObject backgroundMusic = GameObject.Find("Background Music");

        firstRectangle.GetComponent<RectangleAdministrator>().Inside();
        secondRectangle.GetComponent<RectangleAdministrator>().Inside();

        backgroundMusic.GetComponent<BackgroundMusicController>().fadeOut();

        yield return new WaitForSecondsRealtime(1.2f);

        SceneManager.LoadScene(sceneName);
    }


    public void Exit() {

        Application.Quit();
    }

    public void PauseOver() {

        PauseController pauseInterface = F.GetScriptComponentName<PauseController>("User Interface Object");
        pauseInterface.stopPause();
    }

    public void buySpaceship() {
        
        SpaceshipsShopAnimationScript spaceshipAnimation = F.GetScriptComponentName<SpaceshipsShopAnimationScript>("Spaceship Animations");

        InformationSpaceshipsScript spaceshipOpened = spaceshipAnimation.openSpaceship.GetComponent<InformationSpaceshipsScript>();

        if (!spaceshipOpened.spaceshipInformation.bought) {

            var (score, bills, coins, enemiesTakenDown, hitsOnEnemies) = moneyAdmin.GetGeneralInfo();
            int allMoneyWhatYouHave = 100 * bills + coins;
            int allMoneyThatItCost = 100 * spaceshipOpened.spaceshipInformation.bills + spaceshipOpened.spaceshipInformation.coins;
            MoneyLogger.Log($"[ButtonController] Attempting to buy spaceship. Player money: {allMoneyWhatYouHave}, Cost: {allMoneyThatItCost}");
            if (allMoneyWhatYouHave - allMoneyThatItCost >= 0) {

                spaceshipOpened.buyingSpaceship();

                moneyAdmin.SubtractBills(spaceshipOpened.spaceshipInformation.bills);
                moneyAdmin.SubtractCoins(spaceshipOpened.spaceshipInformation.coins);

                var (afterScore, afterBills, afterCoins, afterEnemiesTakenDown, afterHitsOnEnemies) = moneyAdmin.GetGeneralInfo();
                MoneyLogger.Log($"[ButtonController] Spaceship bought. Remaining money: bills={afterBills}, coins={afterCoins}");
            }

        }

    }

    public void useSpaceship() {
        
        SpaceshipsShopAnimationScript spaceshipAnimation = F.GetScriptComponentName<SpaceshipsShopAnimationScript>("Spaceship Animations");

        SpecificSpaceshipInformation newSpaceshipInUse = spaceshipAnimation.openSpaceship.GetComponent<InformationSpaceshipsScript>().spaceshipInformation;
        SpecificSpaceshipInformation lastSpaceshipInUse = spaceshipAnimation.spaceshipInUse.GetComponent<InformationSpaceshipsScript>().spaceshipInformation;

        if (!newSpaceshipInUse.use && newSpaceshipInUse.bought) {

            newSpaceshipInUse.use = true;
            lastSpaceshipInUse.use = false;

        }
    }

    public void updateUsingBuyingSpaceships() {
        use.updateUsingSpaceship();
    }

    public void updateMoneyData() {
        // This method is now handled by MoneyAdministratorScript, so you may not need this anymore.
        // If you need to transfer or reset money, use the appropriate methods from moneyAdmin.
        MoneyLogger.Log("[ButtonController] updateMoneyData called. Now handled by MoneyAdministratorScript.");
    }

    public void changeType(int number) {
        SpaceshipShopAnimation.GetComponent<SpaceshipsShopAnimationScript>().addNumber(number);
    }

    public void resetAllData() {

        // If you want to reset money, use moneyAdmin.ResetTemporal() and moneyAdmin.ResetGeneral()
        moneyAdmin.ResetTemporal();
        moneyAdmin.ResetGeneral();

        foreach (SpecificSpaceshipInformation spaceship in use.informationSpaceship) {

            if (spaceship.spaceshipTag == "Normal Spaceship") {

                spaceship.use = true;
                spaceship.bought = true;
            } else {

                spaceship.use = false;
                spaceship.bought = false;
            }
        }

        use.tagOfUsedSpaceship = "Normal Spaceship";
    }

    public void playButtonAudio() {
        audioManager.PlayEffect("Button Clicked");
    }
}
