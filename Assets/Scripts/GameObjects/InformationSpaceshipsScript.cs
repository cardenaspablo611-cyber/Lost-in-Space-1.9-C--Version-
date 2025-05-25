using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationSpaceshipsScript : MonoBehaviour {

    [SerializeField] private TextMesh informationText;
    [SerializeField] private TextMesh upgradeText;
    [SerializeField] private TextMesh skillText;
    [SerializeField] private TextMesh costText;

    public SpecificSpaceshipInformation spaceshipInformation;
    [SerializeField] private GameObject bill;
    [SerializeField] private GameObject coin;


    private bool start = true;

    

    public void startEnsembleTextAnimation () {

        if (start) {

            spaceshipInformation.createStrings();

            StartCoroutine(invidualTextAnimation(spaceshipInformation.information, informationText));
            StartCoroutine(invidualTextAnimation(spaceshipInformation.upgrade, upgradeText));
            StartCoroutine(invidualTextAnimation(spaceshipInformation.skill, skillText));
            StartCoroutine(invidualTextAnimation((spaceshipInformation.cost).ToString(), costText));
            start = false;

            bill.GetComponent<AppearAnimationScript>().startAppearAnimation();
            coin.GetComponent<AppearAnimationScript>().startAppearAnimation();
        }

    }

    IEnumerator invidualTextAnimation(string text, TextMesh typeOfText) {

        int numberOfMaxCharactersPerLine = 10;

        int numberOfCharacters = 0;

        typeOfText.text = "";
        foreach (char c in text) {

            if (c.ToString() == "\n") {

                typeOfText.text += c;
                numberOfCharacters = 0;

            } else if (numberOfCharacters >= numberOfMaxCharactersPerLine && c.ToString() == " ") {

                numberOfCharacters = 0;
                typeOfText.text += "\n";

            } else if (numberOfCharacters >= numberOfMaxCharactersPerLine + 5) {
            
                numberOfCharacters = 0;
                typeOfText.text += $"-\n{c}";

            } else {

                typeOfText.text += c;
                numberOfCharacters++;
            }
  
            yield return new WaitForSeconds(1 / text.Length);
        }
    }

    public void restartAnimation() {  
        start = true;
    }

    public void buyingSpaceship() {

        spaceshipInformation.bought = true;
    }
}
