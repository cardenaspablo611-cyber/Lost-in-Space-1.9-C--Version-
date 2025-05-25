using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using F = Functions;

public class ButtonUseBuyAdmin : MonoBehaviour {

    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject useButton;
    private DisableEnableButton buyScript;
    private DisableEnableButton useScript;

    [SerializeField] private PlayerData money; // Current purse of the player

    private SpaceshipsShopAnimationScript spaceshipAnimation; // Is where the opened spaceship is located
    private InformationSpaceshipsScript spaceshipOpened; // The opened spaceship object

    private void Start () {

        spaceshipAnimation = F.GetScriptComponentName<SpaceshipsShopAnimationScript>("Spaceship Animations");

        buyScript = buyButton.GetComponent<DisableEnableButton>();
        useScript = useButton.GetComponent<DisableEnableButton>();
    }

    private void Update () {
        
        spaceshipOpened = spaceshipAnimation.openSpaceship.GetComponent<InformationSpaceshipsScript>();
            
        int allMoneyWhatYouHave = 100 * money.bills + money.coins; // Including all the money that you have in on variable to get the difference of the price
        int allMoneyThatItCost = 100 * spaceshipOpened.spaceshipInformation.bills + spaceshipOpened.spaceshipInformation.coins; // Getting all the money that it cost in one number

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