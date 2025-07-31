using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using F = Functions;

public class ButtonUseBuyAdmin : MonoBehaviour {

    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject useButton;
    private DisableEnableButton buyScript;
    private DisableEnableButton useScript;

    private MoneyAdministratorScript moneyAdmin;

    private SpaceshipsShopAnimationScript spaceshipAnimation; // Is where the opened spaceship is located
    private InformationSpaceshipsScript spaceshipOpened; // The opened spaceship object

    private void Start () {

        spaceshipAnimation = F.GetScriptComponentName<SpaceshipsShopAnimationScript>("Spaceship Animations");

        buyScript = buyButton.GetComponent<DisableEnableButton>();
        useScript = useButton.GetComponent<DisableEnableButton>();
        moneyAdmin = FindFirstObjectByType<MoneyAdministratorScript>();
        if (moneyAdmin == null) {
            Debug.LogError("MoneyAdministratorScript not found in the scene!");
        }
    }

    private void Update () {
        
        spaceshipOpened = spaceshipAnimation.openSpaceship.GetComponent<InformationSpaceshipsScript>();
        
        var (score, bills, coins, enemiesTakenDown, hitsOnEnemies) = moneyAdmin.GetGeneralInfo();
        int allMoneyWhatYouHave = 100 * bills + coins; 
        int allMoneyThatItCost = 100 * spaceshipOpened.spaceshipInformation.bills + spaceshipOpened.spaceshipInformation.coins; 
        MoneyLogger.Log($"[ButtonUseBuyAdmin] Update: Player money={allMoneyWhatYouHave}, Spaceship cost={allMoneyThatItCost}, Bought={spaceshipOpened.spaceshipInformation.bought}, Use={spaceshipOpened.spaceshipInformation.use}");
        if (spaceshipOpened.spaceshipInformation.bought) {

            buyScript.changeButtonState("selected");

            if (spaceshipOpened.spaceshipInformation.use) {

                useScript.changeButtonState("selected");

            } else {

                useScript.changeButtonState("enable");
            }

        } else {

            if (allMoneyWhatYouHave - allMoneyThatItCost >= 0) {

                buyScript.changeButtonState("enable");
                useScript.changeButtonState("disable");

            } else {

                buyScript.changeButtonState("disable");
                useScript.changeButtonState("disable");
            }
        }

    }
}