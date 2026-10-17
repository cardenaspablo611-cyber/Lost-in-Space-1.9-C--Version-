using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SpaceshipInUseScript", menuName = "SpaceshipInUseScript")]
public class SpaceshipInUseScript : ScriptableObject {

    public string tagOfUsedSpaceship;

    public SpecificSpaceshipInformation[] informationSpaceship;

    private static bool sessionInitialized = false;

    private void OnEnable() {
        // Reset to defaults on first access in session (Editor and builds)
        // This prevents Play mode from dirtying ScriptableObject assets
        if (!sessionInitialized) {
            ResetToDefaults();
            sessionInitialized = true;
        }
    }

    public void updateUsingSpaceship() {

        foreach (SpecificSpaceshipInformation spaceship in informationSpaceship) {

            if (spaceship.use) {
                tagOfUsedSpaceship = spaceship.spaceshipTag;
                break;
            }
        }
    }

    /// <summary>
    /// Resets all spaceship states to defaults: only Normal Spaceship bought and in use.
    /// Called automatically on session start to ensure clean state in Editor and builds.
    /// </summary>
    public void ResetToDefaults() {
        foreach (SpecificSpaceshipInformation spaceship in informationSpaceship) {
            if (spaceship.spaceshipTag == "Normal Spaceship") {
                spaceship.use = true;
                spaceship.bought = true;
            } else {
                spaceship.use = false;
                spaceship.bought = false;
            }
        }
        tagOfUsedSpaceship = "Normal Spaceship";
        
        Debug.Log("[SpaceshipInUseScript] Session state reset - only Normal Spaceship available");
    }
}
  
