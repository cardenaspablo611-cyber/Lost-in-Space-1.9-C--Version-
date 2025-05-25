using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpecificSpaceshipInformation", menuName = "SpecificSpaceshipInformation")]
public class SpecificSpaceshipInformation : ScriptableObject {

    [Header("Other Data:")]
    public string upgrade;
    public string skill;

    [Header("Information Stuff:")]
    public int lives;
    public int speed;
    public int attack;
    [HideInInspector] public string information;

    [Header("Buy and Use Stuff")]
    public string tag;
    public bool bought;
    public bool use;

    [Header("Cost:")]
    public int coins;
    public int bills;  

    [HideInInspector] public string cost; 
    public void createStrings() {

        if (coins != 0 && bills != 0) {

            cost = $"{bills}\n\n{coins}";
        } else {
            cost = "--None--";
        }

        information = $"Lives: {lives}\n\nSpeed: {speed}\n\nAttack: {attack}";

        if (tag == "Normal Spaceship") {
            bought = true;
        }
    }
}
  