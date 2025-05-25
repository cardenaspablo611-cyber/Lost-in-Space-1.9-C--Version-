using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SpaceshipInUseScript", menuName = "SpaceshipInUseScript")]
public class SpaceshipInUseScript : ScriptableObject {

    public string tagOfUsedSpaceship;

    public SpecificSpaceshipInformation[] informationSpaceship;


    public void updateUsingSpaceship() {

        foreach (SpecificSpaceshipInformation spaceship in informationSpaceship) {

            if (spaceship.use) {
                tagOfUsedSpaceship = spaceship.tag;
                break;
            }
        }
    }
}
  
