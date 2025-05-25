using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameScript : MonoBehaviour {
    private static bool hasRunSetup = false;

    [SerializeField] private PlayerData Advancements;

    private void Awake() {
        if (!hasRunSetup) {
        
            Advancements.resetStaticVariables();
            
            hasRunSetup = true;
        }
    }
}
