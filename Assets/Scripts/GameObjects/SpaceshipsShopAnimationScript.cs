using F = Functions;
using UnityEngine;

public class SpaceshipsShopAnimationScript : MonoBehaviour {
    
    [SerializeField] private GameObject[] SpaceshipsTypes;
    [SerializeField] private GameObject[] numbers;

    private int type = 0;
    private bool changeSpaceship = true;
    private bool theSpaceshipHasBeingChanged = false;

    [HideInInspector] public GameObject openSpaceship;
    [HideInInspector] public GameObject spaceshipInUse;

    public PlayerData currentState; // of money

    private UserInterfaceScript userInterface;

    private void Awake() {

        userInterface = F.GetScriptComponentName<UserInterfaceScript>("User Interface Object");
        selectSpaceship();
    }

    private void FixedUpdate() {

        getTypeSpaceshipshown();

        selectSpaceship();

        changeWithNumbers();
    }

    private void getTypeSpaceshipshown() {
        if (changeSpaceship) {
            if (userInterface.Right) {
                type += 1;
                changeSpaceship = false;
                theSpaceshipHasBeingChanged = true;

            } else if (userInterface.Left) {
                type -= 1;
                changeSpaceship = false;
                theSpaceshipHasBeingChanged = true;

            } 
        }     

        if (type < 0) {

            type = SpaceshipsTypes.Length -1;
        } else if (type > SpaceshipsTypes.Length - 1) {

            type = 0;
        }
    }

    private void selectSpaceship() {

        
        for (int i = 0; i < SpaceshipsTypes.Length; i++) {
            GameObject spaceship = SpaceshipsTypes[i];
            if (i == type) {
                openSpaceship = spaceship;

                spaceship.SetActive(true);
                spaceship.GetComponent<InformationSpaceshipsScript>().startEnsembleTextAnimation();

                if (theSpaceshipHasBeingChanged) {

                    numbers[i].GetComponent<NumbersSpaceshipShopScript>().appear();
                    theSpaceshipHasBeingChanged = false;
                }
                

            } else {
                AnimationScript animationScript = spaceship.GetComponent<AnimationScript>();
                animationScript.restartAnimation();

                spaceship.SetActive(false);
                spaceship.GetComponent<InformationSpaceshipsScript>().restartAnimation();

                if (numbers[i].GetComponent<TextMesh>().color.a == 1f) {
                    numbers[i].GetComponent<NumbersSpaceshipShopScript>().disappear();
                }
            }

            if (spaceship.GetComponent<InformationSpaceshipsScript>().spaceshipInformation.use) {
                spaceshipInUse = spaceship;
            }
        }
    }

    private void changeWithNumbers() {
        if (changeSpaceship) {
            if (Input.GetKey(KeyCode.Alpha1)) {
                type = 0;
                changeSpaceship = false;
                theSpaceshipHasBeingChanged = true;

            }

            if (Input.GetKey(KeyCode.Alpha2)) {
                type = 1;
                changeSpaceship = false;
                theSpaceshipHasBeingChanged = true;

            }

            if (Input.GetKey(KeyCode.Alpha3)) {
                type = 2;
                changeSpaceship = false;
                theSpaceshipHasBeingChanged = true;

            }

            if (Input.GetKey(KeyCode.Alpha4)) {
                type = 3;
                changeSpaceship = false;
                theSpaceshipHasBeingChanged = true;

            }

            if (Input.GetKey(KeyCode.Alpha5)) {
                type = 4;
                changeSpaceship = false;
                theSpaceshipHasBeingChanged = true;

            }
        }
    }

    public void changeFalse() {
        changeSpaceship = false;
    }

    public void changeTrue() {
        changeSpaceship = true;
    }

    public void addNumber(int number) {
        
        if (changeSpaceship) {
            type += number;
            changeSpaceship = false;
            theSpaceshipHasBeingChanged = true;
        }
    }
}
