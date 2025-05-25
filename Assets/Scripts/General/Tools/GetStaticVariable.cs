using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using F = Functions;

public class GetStaticVariable : MonoBehaviour {

    [SerializeField] private SpaceshipInUseScript spaceshipInUse;

    private void Awake() {

        F.getSpaceshipInUseVariable(spaceshipInUse);
    }
    
}
