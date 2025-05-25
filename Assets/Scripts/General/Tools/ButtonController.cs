using UnityEngine.SceneManagement;
using UnityEngine;
using F = Functions;
using System.Collections;

public class ButtonController : MonoBehaviour {

    public SpaceshipInUseScript use;

    [SerializeField] private PlayerData temporalAdvancements;
    [SerializeField] private PlayerData Advancements;
    
    [SerializeField] private GameObject SpaceshipShopAnimation;
    [SerializeField] private SpecificSpaceshipInformation[] spaceships;

    [SerializeField] private SceneData sceneData;

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

        yield return new WaitForSecondsRealtime(0.5f);

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
        PlayerData money = spaceshipAnimation.currentState;

        if (!spaceshipOpened.spaceshipInformation.bought) {

            int allMoneyWhatYouHave = 100 * money.bills + money.coins;
            int allMoneyThatItCost = 100 * spaceshipOpened.spaceshipInformation.bills + spaceshipOpened.spaceshipInformation.coins;

            if (allMoneyWhatYouHave - allMoneyThatItCost >= 0) {

                spaceshipOpened.buyingSpaceship();

                allMoneyWhatYouHave -= allMoneyThatItCost;

                money.bills = (allMoneyWhatYouHave / 100);
                money.coins = allMoneyWhatYouHave - money.bills * 100;
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
        Advancements.AddDataToPlayerData(temporalAdvancements);
        temporalAdvancements.resetStaticVariables();
    }

    public void changeType(int number) {
        SpaceshipShopAnimation.GetComponent<SpaceshipsShopAnimationScript>().addNumber(number);
    }

    public void resetAllData() {

        temporalAdvancements.resetStaticVariables();
        Advancements.resetStaticVariables();

        foreach (SpecificSpaceshipInformation spaceship in use.informationSpaceship) {

            if (spaceship.tag == "Normal Spaceship") {

                spaceship.use = true;
                spaceship.bought = true;
            } else {

                spaceship.use = false;
                spaceship.bought = false;
            }
        }

        use.tagOfUsedSpaceship = "Normal Spaceship";
    }
}
